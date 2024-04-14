using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EnemyController : BaseMovingObject
    {
        public SpriteRenderer spriteRenderer;

        public GameObject bullet;
        public int Id;

        private UnityEvent OnEndMove = new UnityEvent();
        private readonly List<Vector2> _directions = new() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        public int EstimateMoves = 0;
        public Vector2 CurrentDirection = Vector2.up;
        public Damagable damagable;
        public GameObject ExplosionPrefab;
        public float ReloadDelay = 0.5f;
        public float CurrentReloadDelay = 0f;
        public bool canShoot = true;
        public bool rotating = false;

        protected override void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
            damagable = GetComponent<Damagable>();
            damagable.Death.AddListener(OnDeath);
            StartCoroutine(Reload());

        }

        private void OnDeath()
        {
            Destroy(_shadowRef);
            // explosion
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        }

        //void Start()
        //{
        //    animator = GetComponent<Animator>();
        //    spriteRenderer = GetComponent<SpriteRenderer>();
        //    boxCollider2D = GetComponent<BoxCollider2D>();
        //    animator.StopPlayback();

        //}


        void Update()
        {
            if (canShoot && !rotating)
            {
                Shoot();
            }
            HandleMove();
        }

        private void Shoot()
        {
            canShoot = false;

            var start = gameObject.transform.position;
            var z = transform.localEulerAngles.z;
            var frontCenterTank = start + Quaternion.Euler(0, 0, z) * Vector3.up * MapTankWidth / 2;
            var bulletPrefab = Instantiate(bullet, frontCenterTank, Quaternion.Euler(0, 0, z));
            var controller = bulletPrefab.GetComponent<BulletController>();
            controller.Create(frontCenterTank, Quaternion.Euler(0, 0, z));
            //controller.ParentTagName = LayerMask.LayerToName(gameObject.layer);
            controller.Parent = gameObject;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(ReloadDelay);
            canShoot = true;
        }

        private IEnumerator OnRotate()
        {
            yield return new WaitForSeconds(0.1f);
            rotating = false;
        }
        private void HandleMove()
        {
            //return;
            if (IsMove)
            {
                return;
            }

            if (EstimateMoves == 0)
            {
                rotating = true;
                GeneratePath();
                StartCoroutine(OnRotate());
            }

            EstimateMoves--;

            base.MovePosition();
        }


        private void GeneratePath()
        {
            EstimateMoves = Random.Range(3, 12);
            CurrentDirection = _directions[Random.Range(0, _directions.Count)];
            //CurrentDirection = Vector2.left; 
            MoveRotation(CurrentDirection);
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
    }
}
