using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlayerInput : MonoBehaviour
{
    public bool IsMove = false;
    [SerializeField]
    private float MaxSpeed = 1f;
    //[SerializeField]
    private float MapTankWidth = 0.5f;

    private float MovePoint = 0.5f / 2; // part of move to move smooth 1/8 square

    [SerializeField]
    public LayerMask layerBloking;

    public Animator animator;

    public GameObject bullet;
    private BoxCollider2D boxCollider2D;



    private SpriteRenderer spriteRenderer;
    public Vector3 centerLeft;
    public Vector3 centerRight;
    public Vector3 moveVector;
    public Vector3 frontCenterTank;
    public Vector3 nextCenterTank;
    public Rect nextRect;
    public Vector2 sphereCenter;


    public Rect Rect;
    public Vector2 CenterRect = Vector2.up;
    public Rect RectTo;
    public float Rotation = 0;
    public Coroutine coroutine;



    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator.StopPlayback();
    }
    
    void Update()
    {
        HandleMoving();
        HandleFire();
    }

    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var start = gameObject.transform.position;
            var z = transform.localEulerAngles.z;
            frontCenterTank = start + Quaternion.Euler(0, 0, z) * Vector3.up * MapTankWidth/2;
            var bulletPrefab = Instantiate(bullet, frontCenterTank, Quaternion.Euler(0, 0, z));
            var controller = bulletPrefab.GetComponent<BulletController>();
            controller.Create(frontCenterTank, Quaternion.Euler(0, 0, z));
        }
    }

    private void HandleMoving()
    {
        if (IsMove)
        {
            return;
        }

        var x = (int)Input.GetAxisRaw("Horizontal");
        var y = (int)Input.GetAxisRaw("Vertical");
        var wasRotation = MoveRotation(x, y);
        if (!wasRotation)
        {

            MovePosition(x, y);
        }
    }

    private void MovePosition(int x, int y)
    {
        var movementVector = new Vector2(x, y);
        

        if (movementVector == Vector2.zero)
        {
            animator.enabled = false;
            return;
        }
        animator.enabled = true;
        var centerTank = transform.position;
        var boundsTank = boxCollider2D.bounds;
        var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        var distance = MovePoint;
        var end = centerTank + direction.normalized * distance;

        // gizmoz
        //

        Rect = UnityCustomExtensions.CreateRectFromCenter(centerTank, boundsTank.size.x, boundsTank.size.y);
        CenterRect = Rect.center;
        var to = Rect.center + (Vector2)direction.normalized * distance; //Quaternion.Euler(0, 0, Rotation) *
        RectTo = UnityCustomExtensions.CreateRectFromCenter(to, Rect.size.x, Rect.size.y);
        Rotation = transform.localEulerAngles.z;

        ///


        var hits = Physics2D.BoxCastAll(boundsTank.center, boundsTank.size, transform.eulerAngles.z, direction, distance, layerBloking);
        //if (hits.Length > 0)
        //{
        //    foreach (var h in hits)
        //    {
        //        h.transform.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        //        Debug.Log($"BoxCastAll hit: {h.transform.gameObject.name}");
        //    }
        //}

        if (hits.Length > 0)
        {
            return;
        }
        IsMove = true;
        coroutine = StartCoroutine(MoveSmooth(end));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var thickness = 10;

        //Handles.DrawBezier(Rect.center, RectTo.center, Rect.center, RectTo.center, Color.blue, null, thickness);
        //Gizmos.DrawLine(Rect.center, RectTo.center);

        //Gizmos.DrawSphere(nextRect.center, 0.03f);
        ////////////////////
        //UnityCustomExtensions.DrawRect(Rect, Color.yellow, Rotation);
        //UnityCustomExtensions.DrawRect(RectTo, Color.green, Rotation);
    }

    private bool MoveRotation(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return false;
        }
        if (x != 0)
        {
            y = 0;
        }
        var angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        //Debug.Log($"Quaternion.AngleAxis(-angle, Vector3.forward);: {Quaternion.AngleAxis(-angle, Vector3.forward);} transform.rotation: {transform.rotation}");
        var rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        if (transform.rotation == rotation)
        {
            return false;
        }
        IsMove = true;
        StartCoroutine(RotateSmooth());
        transform.rotation = rotation;
        //transform.LookAt();
        return true;
    }

    private IEnumerator RotateSmooth()
    {
        yield return new WaitForSeconds(0.050f);
        IsMove = false;
    }
    private IEnumerator MoveSmooth(Vector3 end)
    {
        var current = transform.position;
        var remaining = current - end;
        while (true)
        {
            if (remaining.sqrMagnitude < float.Epsilon)
            {
                break;
            }
            var newPos = Vector3.MoveTowards(current, end, Time.deltaTime * MaxSpeed);
            //Debug.Log($"newPos {newPos}");
            transform.position = newPos;
            current = transform.position;
            remaining = current - end;
            yield return null;
        }
        //yield return new WaitForSeconds(1/3f);
        //transform.position = end;
        IsMove = false;
    }
    


}
