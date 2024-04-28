using System.Collections;
using Assets.Scripts.TanksData;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseMovingObject : MonoBehaviour
    {

        public TankPropertiesData TankProperties;

        public bool IsMove;

        //public float MaxSpeed = 1f;

        //public static float MapTankWidth = 0.5f;

        //public float MovePoint = 0.5f / 2 ; // part of move to move smooth 1/8 square

        public LayerMask LayerBlocking;
        public GameObject ShadowPrefab;

        protected BoxCollider2D boxCollider2D;
        public Animator animator;
        protected GameObject _shadowRef;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            animator.StopPlayback();
        }

        public virtual void SetPropsData(TankPropertiesData tankProperties)
        {
            TankProperties = tankProperties;
        }

        public void MovePosition()
        {

            animator.enabled = true;
            var centerTank = transform.position;
            var boundsTank = boxCollider2D.bounds;
            var direction = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * Vector3.up;
            var distance = TankProperties.MovePoint;
            var end = centerTank + direction.normalized * distance;


            var hits = Physics2D.BoxCastAll(boundsTank.center, boundsTank.size, transform.eulerAngles.z, direction, distance, LayerBlocking);

            if (hits.Length > 1)
            {
                foreach (var raycastHit2D in hits)
                {
                    var overlapBox = raycastHit2D.collider.bounds.Intersects(boundsTank); // if spawn together, not detect each other
                    if (overlapBox)
                    {
                        continue;
                    }
                    return;
                }
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
                var newPos = Vector3.MoveTowards(current, end, Time.deltaTime * TankProperties.MaxSpeed);
                transform.position = newPos;
                current = transform.position;
                remaining = current - end;
                yield return null;
            }
            Destroy(_shadowRef);
            IsMove = false;
        }
    }
}
