using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool isMove = false;
    public float MaxSpeed = 1f;
    public float MovePoint = 0.5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Move();
    }

    private void Move()
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

    private void MoveRotation(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return;
        }
        float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(x, y);
        Debug.LogWarning($"angle: {angle}, an2: {angle2}");
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    private void MovePosition(int x, int y)
    {
        var movementVector = new Vector2(x, y);
        if (movementVector == Vector2.zero)
        {
            return;
        }

        if (x != 0)
        {
            y = 0;
        }

        isMove = true;
        var end = gameObject.transform.position + new Vector3(x * MovePoint, y * MovePoint, 0);
        Debug.Log($"Time {Time.deltaTime} End: {end}");
        StartCoroutine(MoveSmooth(end));
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
            Debug.Log($"newPos {newPos}");
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
