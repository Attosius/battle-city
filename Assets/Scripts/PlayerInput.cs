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
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    public Vector3 centerLeft;
    public Vector3 centerRight;
    public Vector3 moveVector;
    public Vector3 frontCenterTank;
    public Vector3 nextCenterTank;
    public Rect nextRect;
    public Vector2 sphereCenter;

    public float rectMove = 0.2f;
    public float rectSizeY = 0.46875f;



    public Rect Rect;
    public Vector2 CenterRect = Vector2.up;
    public Rect RectTo;
    public float Rotation = 0;



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

    private Vector3 moveForwardDelta;

    private void MovePosition(int x, int y)
    {
        var movementVector = new Vector2(x, y);
        var centerTank = gameObject.transform.position;
        var boundsTank = boxCollider2D.bounds;
        var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        var distance = MovePoint;
        var end = centerTank + direction.normalized * distance;

        // gizmoz
        //

        Rect = CreateRectFromCenter(centerTank, boundsTank.size.x, boundsTank.size.y);
        CenterRect = Rect.center;
        var to = Rect.center + (Vector2)direction.normalized * distance; //Quaternion.Euler(0, 0, Rotation) *
        RectTo = CreateRectFromCenter(to, Rect.size.x, Rect.size.y);
        Rotation = transform.localEulerAngles.z;

        ///


        var hits = Physics2D.BoxCastAll(boundsTank.center, boundsTank.size, transform.eulerAngles.z, direction, distance, layerBloking);
        if (hits.Length > 0)
        {
            foreach (var h in hits)
            {
                h.transform.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                Debug.Log($"BoxCastAll hit: {h.transform.gameObject.name}");
            }
            //Rect.position = hitLeft[0].point;
        }

        //moveForwardDelta = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up * (MapTankWidth * 15 / 16);
        //var euler = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up * (MapTankWidth / 2);


        //frontCenterTank = center + euler;
        //centerLeft = center + Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.left * (MapTankWidth * 3/8);
        //centerRight = center + Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.right * (MapTankWidth * 3/8);
        //moveVector = euler;

        ////var sizex = (centerLeft + moveForwardDelta - centerLeft).x; //-0,46875
        ////var sizey = (centerRight - centerLeft).y; //y: 0,375
        //var size = new Vector3((centerRight - centerLeft).magnitude, moveForwardDelta.magnitude); //y: 0,375
        ////var sizex = (centerRight - centerLeft).magnitude; //y: 0,375
        ////var sizey = (centerLeft + moveForwardDelta - centerLeft).magnitude; //* Quaternion.Euler(0, 0, transform.localEulerAngles.z; //-0,46875
        
        //size = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * size;
        //var sizex = size.x;
        //var sizey = size.y;
        ////sizex = 0.375f;
        ////sizey = 0.46875f;
        //// y: 0,375
        ////Debug.Log($"x: {sizex} y: {sizey}");
        ////nextRect = new Rect(centerLeft, size);
        //var size2 = new Vector2(0.45f, rectSizeY);
        ////nextRect = new Rect(frontCenterTank, size2);
        //nextRect = CreateRectFromCenter(frontCenterTank, size2.x, size2.y);
        //sphereCenter = frontCenterTank + Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up * (MapTankWidth * 3 / 32);
       
        ////
        //direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
        //var hitLeft = Physics2D.BoxCast(nextRect.center, nextRect.size/2, transform.eulerAngles.z, direction, rectMove);
        ////Debug.Log($"A: {moveForwardDelta.magnitude} {nextRect.center}, {nextRect.size}," +
        ////    $" {transform.eulerAngles.z}, {direction}");

        //GameObject otherObject = GameObject.Find("all-tanks--w411");
        //BoxCollider2D boxCollider = otherObject.GetComponent<BoxCollider2D>();


        //Rect otherRect = new Rect(boxCollider.bounds.min, boxCollider.bounds.size);

        // Check if the existingRect overlaps with the otherRect
        //if (nextRect.Overlaps(otherRect))
        //{
        //    Debug.Log("Overlap detected with: " + otherObject.name);
        //}
        //else
        //{
        //    Debug.Log("No overlap with: " + otherObject.name);
        //}

        //if (hitLeft.transform != null)
        //{
        //    Debug.Log($"Hit A: {hitLeft.transform?.name}");
        //    return;
        //}

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

        //var prevFaceCenterTank = frontCenterTank;
        //moveVector = new Vector3(x * (MovePoint), y * MovePoint, 0);
        //var end = center + moveVector;
        //nextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);
        //var faceNextCenterTank = center + new Vector3(x * MovePoint, y * MovePoint, 0);

        //var hitLeft = Physics2D.OverlapCircle(centerLeft, 0.05f, layerBloking);


        //Debug.Log($"start: {start } end: {end } centerLeft: { centerLeft }centerRight: { centerRight }, centerLeft + moveVector {centerLeft + moveVector}");

        //var hitLeft = Physics2D.Linecast(centerLeft, centerLeft + moveForwardDelta, layerBloking);
        //var hitRight = Physics2D.Linecast(centerRight, centerRight + moveForwardDelta, layerBloking);
        //var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        //var pozSize = new Vector2(Math.Abs(nextRect.size.x), Math.Abs(nextRect.size.y));

        //sizex = 0.375f;
        //sizey = 0.46875f;
        //var angle = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
        //var hitLeft = Physics2D.BoxCast(nextRect.position, nextRect.size,
        //    transform.eulerAngles.z, direction, 0.125f);

        //Debug.Log($"{nextRect.position}, {nextRect.size}, {transform.eulerAngles.z}, {direction}, 0.125f");
        //var hitLeft = Physics2D.BoxCast(nextRect.center, nextRect.size, transform.eulerAngles.z, direction, 0.2f);
        //var hitRight = Physics2D.BoxCast(nextRect.center, pozSize, 0, direction, 0.001f, layerBloking.value);
        //var hitRight = Physics2D.Linecast(centerRight, centerRight + moveForwardDelta, layerBloking);

        if (hits.Length > 0)
        {
            return;
        }
        IsMove = true;
        StartCoroutine(MoveSmooth(end));
    }
    private void CreateRects()
    {
    }

    public Rect CreateRectFromCenter(Vector2 center, float width, float height)
    {
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        float xMin = center.x - halfWidth;
        float yMin = center.y - halfHeight;

        return new Rect(xMin, yMin, width, height);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var thickness = 10;

        Handles.DrawBezier(Rect.center, RectTo.center, Rect.center, RectTo.center, Color.blue, null, thickness);
        Gizmos.DrawLine(Rect.center, RectTo.center);

        //Gizmos.DrawSphere(nextRect.center, 0.03f);
        ////////////////////
        DrawRect(Rect, Color.yellow);
        DrawRect(RectTo, Color.green);
    }

    void DrawRect(Rect rect, Color color)
    {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(rect.center, Quaternion.Euler(0, 0, Rotation), Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, rect.size);



        Gizmos.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity, Vector3.one);
        Gizmos.DrawSphere(rect.center, 0.03f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rect.position, 0.03f);
        //Debug.Log($"center rect: {rect.center}");
        //Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0.01f), new Vector3(rect.size.x, rect.size.y, 0.01f));
        //Gizmos.DrawSphere(new Vector3(rect.center.x, rect.center.y, 0.01f), 0.03f);
    }

    //private void OnDrawGizmos()
    //{

    //    //Gizmos.color = Color.magenta;
    //    //Gizmos.DrawSphere(frontCenterTank, 0.03f);

    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawSphere(centerLeft, 0.04f);

    //    //Gizmos.color = Color.blue;
    //    //Gizmos.DrawLine(centerLeft, centerLeft + moveForwardDelta);

    //    //Gizmos.color = Color.magenta;
    //    ////Gizmos.DrawSphere(centerRight, 0.03f);

    //    //Gizmos.color = Color.magenta;
    //    //Gizmos.DrawLine(centerRight, centerRight + moveForwardDelta);


    //    //Gizmos.color = Color.red;
    //    //var rect = new Rect(0, 0, 1, 1.5f);
    //    //Gizmos.matrix = Matrix4x4.TRS(rect.center, this.transform.rotation, Vector3.one);
    //    //Gizmos.DrawWireCube(rect.center, rect.size);
    //    //Gizmos.DrawSphere(rect.center, 0.03f);

    //    //Gizmos.color = Color.cyan;
    //    //Gizmos.matrix = Matrix4x4.TRS(rect.position, this.transform.rotation, Vector3.one);
    //    //Gizmos.DrawWireCube(rect.center, rect.size);

    //    //Gizmos.DrawSphere(rect.position, 0.06f);


    //    //Gizmos.color = Color.white;
    //    //Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

    //    //Gizmos.DrawSphere(nextRect.position, 0.03f);
    //    //Gizmos.DrawSphere(nextRect.center, 0.03f);
    //    ////////////////////
    //    DrawRect(nextRect);
    //    //Gizmos.color = Color.yellow;
    //    //Gizmos.matrix = Matrix4x4.TRS(nextRect.center, this.transform.rotation, Vector3.one);
    //    //Gizmos.DrawWireCube(Vector2.zero, nextRect.size);
    //    //////////////
    //    //Gizmos.color = Color.yellow;
    //    ////DrawRect(nextRect);
    //    //Gizmos.DrawWireSphere(sphereCenter, 3/16f);

    //    Gizmos.color = Color.blue;
    //    //var direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
    //    Gizmos.DrawLine(nextRect.position, nextRect.position + new Vector2(0, rectMove));

    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawLine(transform.position, frontCenterTank);
    //    Gizmos.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity, Vector3.one);
    //}

    //void DrawRect(Rect rect)
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.matrix = Matrix4x4.TRS(rect.center, this.transform.rotation, Vector3.one);
    //    Gizmos.DrawWireCube(Vector2.zero, rect.size);



    //    Gizmos.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity, Vector3.one);
    //    Gizmos.DrawSphere(rect.center, 0.03f);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(rect.position, 0.03f);
    //    //Debug.Log($"center rect: {rect.center}");
    //    //Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0.01f), new Vector3(rect.size.x, rect.size.y, 0.01f));
    //    //Gizmos.DrawSphere(new Vector3(rect.center.x, rect.center.y, 0.01f), 0.03f);
    //}
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
