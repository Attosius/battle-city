using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : BaseMovingObject
{
    public SpriteRenderer spriteRenderer;

    public GameObject bullet;

    private UnityEvent OnEndMove = new UnityEvent();
    private readonly List<Vector2> _directions = new() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public int EstimateMoves = 0;
    public Vector2 CurrentDirection = Vector2.up;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator.StopPlayback();

    }

    private bool MoveRotation(Vector2 direction)
    {

        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        if (transform.rotation == rotation)
        {
            return false;
        }
        IsMove = true;
        transform.rotation = rotation;
        IsMove = false;
        return true;
    }

    void Update()
    {
        if (IsMove)
        {
            return;
        }

        if (EstimateMoves == 0)
        {
            GeneratePath();
        }

        EstimateMoves--;

        base.MovePosition();
    }
 

    private void GeneratePath()
    {
        EstimateMoves = Random.Range(1, 12);
        CurrentDirection = _directions[Random.Range(0, _directions.Count)];
        //CurrentDirection = Vector2.left; 
        MoveRotation(CurrentDirection);
    }


}
