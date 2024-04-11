using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovingObject : MonoBehaviour
{

    public bool IsMove;

    public float MaxSpeed = 1f;

    public float MapTankWidth = 0.5f;

    public float MovePoint = 0.5f / 2; // part of move to move smooth 1/8 square

    [SerializeField]
    public LayerMask layerBloking;

    public Animator animator;

    public BoxCollider2D boxCollider2D;
    public GameObject ShadowPrefab;
    public GameObject _shadowRef;

    public void MovePosition()
    {

        animator.enabled = true;
        var centerTank = transform.position;
        var boundsTank = boxCollider2D.bounds;
        var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        var distance = MovePoint;
        var end = centerTank + direction.normalized * distance;


        var hits = Physics2D.BoxCastAll(boundsTank.center, boundsTank.size, transform.eulerAngles.z, direction, distance, layerBloking);

        if (hits.Length > 1)
        {
                return;
        }
        IsMove = true;
        _shadowRef = Instantiate(ShadowPrefab, end, this.transform.rotation);
        StartCoroutine(MoveSmooth(end));
    }

    public IEnumerator MoveSmooth(Vector3 end)
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
            transform.position = newPos;
            current = transform.position;
            remaining = current - end;
            yield return null;
        }
        Destroy(_shadowRef);
        IsMove = false;
    }
}
