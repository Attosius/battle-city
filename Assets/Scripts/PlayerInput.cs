using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    private SpriteRenderer spriteRenderer;

    public Vector3 centerLeft;
    public Vector3 centerRight;
    public Vector3 moveVector;
    public Vector3 frontCenterTank;
    public Vector3 nextCenterTank;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        var center = gameObject.transform.position;
        var euler = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up * MapTankWidth / 2;
        //var euler2 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.down * MapTankWidth / 2;
        //var euler3 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.left * MapTankWidth / 2;
        //var euler4 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        //var euler5 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.forward;
        //var euler6 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.down;
        //var euler7 = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.back;


        frontCenterTank = center + euler;
        centerLeft = center + Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.left * MapTankWidth * 3/8;
        centerRight = center + Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.right * MapTankWidth * 3/8;
        moveVector = euler;
        if (movementVector == Vector2.zero)
        {
            animator.enabled = false;
            return;
        }
        animator.enabled = true;

        if (x != 0)
        {
            y = 0;
        }

        var prevFaceCenterTank = frontCenterTank;
        //moveVector = new Vector3(x * (MovePoint), y * MovePoint, 0);
        var end = center + moveVector;
        nextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        var faceNextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        
        //var hitLeft = Physics2D.OverlapCircle(centerLeft, 0.05f, layerBloking);

        var hitLeft = Physics2D.Linecast(centerLeft, centerLeft + moveVector, layerBloking);
        var hitRight = Physics2D.Linecast(centerRight, centerRight + moveVector, layerBloking);

        //Debug.Log($"start: {start } end: {end } centerLeft: { centerLeft }centerRight: { centerRight }, centerLeft + moveVector {centerLeft + moveVector}");

        if (hitLeft.transform != null || hitRight.transform != null)
        {
            //var layerBloking2 = LayerMask.NameToLayer("Wall");
            //var m = LayerMask.GetMask("Wall");
            Debug.Log($"Hit: {hitLeft.transform?.name} Hit2: {hitRight.transform?.name}");
            frontCenterTank = prevFaceCenterTank;
            return;
        }
        IsMove = true;
        StartCoroutine(MoveSmooth(end));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(centerLeft, centerLeft + moveVector);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(frontCenterTank, 0.03f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerLeft, 0.03f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerRight, 0.03f);



        Gizmos.color = Color.blue;
        Gizmos.DrawLine(centerRight, centerRight + moveVector);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, frontCenterTank);
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
