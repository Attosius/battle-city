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
    private float MovePoint = 0.5f / 2;

    private float MapWidth = 16;
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
        MoveRotation(x, y);
        MovePosition(x, y);
    }


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
        var end = start + new Vector3(x * MovePoint, y * MovePoint, 0);
        var attemptEnd = start + new Vector3(( x + 1) * MovePoint, (y + 1) * MovePoint, 0);
        var hit = Physics2D.Linecast(start, attemptEnd, layerBloking);

        Debug.Log($"end: {end }attemptEnd: { attemptEnd }");

        if (hit.transform != null)
        {
            var layerBloking2 = LayerMask.NameToLayer("Wall");
            var m = LayerMask.GetMask("Wall");
            Debug.Log($"Hit: {hit.transform.name}");
            return;
        }
        isMove = true;
        StartCoroutine(MoveSmooth(end));
    }


    private void MoveRotation(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return;
        }
        if (x != 0)
        {
            y = 0;
        }
        var angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
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
            var newPos = Vector3.MoveTowards(current, end, Time.deltaTime * MaxSpeed );
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
