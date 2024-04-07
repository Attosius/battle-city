using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
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
    public SpriteRenderer spriteRenderer;

    public GameObject bullet;
    private BoxCollider2D boxCollider2D;

    public Coroutine coroutine;
    public UnityEvent OnEndMove = new UnityEvent();
    private readonly List<Vector2> _directions = new() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public int EstimateMoves = 0;
    public Vector2 CurrentDirection = Vector2.up;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        OnEndMove.AddListener(OnEndMoveListener);
        animator.StopPlayback();

    }

    private void OnEndMoveListener()
    {
        Debug.Log($"End move {gameObject.name}");
        //var wasRotation = MoveRotation(1, 0);
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
        //StartCoroutine(RotateSmooth());
        transform.rotation = rotation;
        IsMove = false;
        return true;
    }
    private IEnumerator RotateSmooth()
    {
        yield return new WaitForSeconds(0.050f);
        IsMove = false;
    }
    // Update is called once per frame
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
        //var movementVector = Path.Dequeue();


        //if (movementVector == Vector2.zero)
        //{
        //    animator.enabled = false;
        //    return;
        //}
        animator.enabled = true;
        var centerTank = transform.position;
        var boundsTank = boxCollider2D.bounds;
        var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
        var distance = MovePoint;
        var end = centerTank + direction.normalized * distance;


        var hits = Physics2D.BoxCastAll(boundsTank.center, boundsTank.size, transform.eulerAngles.z, direction, distance, layerBloking);
        


        if (hits.Length > 1)
        {
            foreach (var h in hits)
            {
                //h.transform.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                Debug.Log($"BoxCastAll hit: {h.transform.gameObject.name}");
            }
            return;
        }
        IsMove = true;
        coroutine = StartCoroutine(MoveSmooth(end));


    }
 

    private void GeneratePath()
    {
        EstimateMoves =  Random.Range(1, 12);
        CurrentDirection = _directions[Random.Range(0, _directions.Count)];
        //CurrentDirection = Vector2.left; 
        MoveRotation(CurrentDirection);
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
        OnEndMove?.Invoke();
    }

}
