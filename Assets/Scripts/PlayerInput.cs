using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour
{
    public bool isMove = false;
    [SerializeField]
    private float MaxSpeed = 1f;
    //[SerializeField]
    private float MapTankWidth = 0.5f;

    private float MovePoint = 0.5f / 4; // part of move to move smooth 1/8 square

    [SerializeField]
    public LayerMask layerBloking;

    public Animator animator;

    public GameObject bullet;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.StopPlayback();
        //layerBloking = new LayerMask
        //{
        //    value = LayerMask.GetMask("Wall")
        //};
    }

    // Update is called once per frame
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
            //var nextCenterTank = start + new Vector3(MapTankWidth / 4, MapTankWidth / 4);
            //var rotation = transform.rotation;
            var z = transform.localEulerAngles.z;
            faceCenterTank = start + Quaternion.Euler(0, 0, z) * Vector3.up * MapTankWidth/2;
            var bulletPrefab = Instantiate(bullet, faceCenterTank, Quaternion.Euler(0, 0, z));
            var controller = bulletPrefab.GetComponent<BulletController>();
            controller.Create(faceCenterTank, Quaternion.Euler(0, 0, z));
            //Instantiate(bullet, faceCenterTank, Quaternion.Euler(0, 0, z));
            //float angle = Quaternion.Angle(transform.rotation, target.rotation);
        }
    }

    private void HandleMoving()
    {
        if (isMove)
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

    [SerializeField]
    private Vector3 faceLeft;
    [SerializeField]
    private Vector3 faceRight;
    [SerializeField]
    private Vector3 moveVector;
    [SerializeField]
    private Vector3 faceCenterTank;
    private Vector3 nextCenterTank;

    private void MovePosition(int x, int y)
    {
        var movementVector = new Vector2(x, y);
        if (movementVector == Vector2.zero)
        {
            animator.enabled = false;
            //animator.StopPlayback();
            return;
        }
        animator.enabled = true;
        //animator.StartPlayback();

        if (x != 0)
        {
            y = 0;
        }

        var prevFaceCenterTank = faceCenterTank;
        var center = gameObject.transform.position;
        faceCenterTank = center + Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up * MapTankWidth / 2;
        moveVector = new Vector3(x * (MovePoint), y * MovePoint, 0);
        var end = center + moveVector;
        nextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        var faceNextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        //var attemptEnd = start + new Vector3(( x + 1) * MovePoint, (y + 1) * MovePoint, 0);
        //var hit = Physics2D.Linecast(start, nextCenterTank, layerBloking);
        //faceTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        faceLeft = faceCenterTank - new Vector3(y * MapTankWidth / 4, x * MapTankWidth / 4, 0);
        faceRight = faceCenterTank + new Vector3(y * MapTankWidth / 4, x * MapTankWidth / 4, 0);

        var hitLeft = Physics2D.Linecast(faceLeft, faceLeft + moveVector, layerBloking);
        var hitRight = Physics2D.Linecast(faceRight, faceRight + moveVector, layerBloking);

        //Debug.Log($"start: {start } end: {end } faceLeft: { faceLeft }faceRight: { faceRight }, faceLeft + moveVector {faceLeft + moveVector}");

        if (hitLeft.transform != null || hitRight.transform != null)
        {
            //var layerBloking2 = LayerMask.NameToLayer("Wall");
            //var m = LayerMask.GetMask("Wall");
            Debug.Log($"Hit: {hitLeft.transform?.name} Hit2: {hitRight.transform?.name}");
            faceCenterTank = prevFaceCenterTank;
            return;
        }
        isMove = true;
        StartCoroutine(MoveSmooth(end));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(faceLeft, faceLeft + moveVector);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(faceCenterTank, 0.02f);



        Gizmos.color = Color.blue;
        Gizmos.DrawLine(faceRight, faceRight + moveVector);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, faceCenterTank);
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
        isMove = true;
        StartCoroutine(RotateSmooth());
        transform.rotation = rotation;
        //transform.LookAt();
        return true;
    }

    private IEnumerator RotateSmooth()
    {
        yield return new WaitForSeconds(0.050f);
        isMove = false;
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
        isMove = false;
    }

}
