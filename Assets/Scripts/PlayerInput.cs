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
    private float MapWidth = 0.5f;

    private float MovePoint = 0.5f / 4; // part of move to move smooth 1/8 square

    [SerializeField]
    public LayerMask layerBloking;

    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.StopPlayback();
        layerBloking = new LayerMask
        {
            value = LayerMask.GetMask("Wall")
        };
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

        var start = gameObject.transform.position;
        moveVector = new Vector3(x * (MovePoint), y * MovePoint, 0);
        var end = start + moveVector;
        var faceTank = start + new Vector3(x * MovePoint, y * MovePoint, 0);
        //var attemptEnd = start + new Vector3(( x + 1) * MovePoint, (y + 1) * MovePoint, 0);
        //var hit = Physics2D.Linecast(start, faceTank, layerBloking);

        faceLeft = faceTank - new Vector3(y * MapWidth / 4, x * MapWidth / 4, 0);
        faceRight = faceTank + new Vector3(y * MapWidth / 4, x * MapWidth / 4, 0);

        var hitLeft = Physics2D.Linecast(faceLeft, faceLeft + moveVector, layerBloking);
        var hitRight = Physics2D.Linecast(faceRight, faceRight + moveVector, layerBloking);

        Debug.Log($"start: {start } end: {end } faceLeft: { faceLeft }faceRight: { faceRight }, faceLeft + moveVector {faceLeft + moveVector}");

        if (hitLeft.transform != null || hitRight.transform != null)
        {
            //var layerBloking2 = LayerMask.NameToLayer("Wall");
            //var m = LayerMask.GetMask("Wall");
            Debug.Log($"Hit: {hitLeft.transform?.name} Hit2: {hitRight.transform?.name}");
            return;
        }
        isMove = true;
        StartCoroutine(MoveSmooth(end));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(faceLeft, faceLeft+moveVector);
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
