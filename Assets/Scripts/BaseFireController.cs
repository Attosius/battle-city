using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseFireController : MonoBehaviour
    {
        public GameObject bullet;
        public float ReloadDelay = 1.5f; //sec
        public float CurrentReloadDelay = 0f;
        public bool CanShoot = false;
        public bool IsPlayer = false;

        private BulletController _lastBulletController = new ();

        public void Awake()
        {
            CurrentReloadDelay = ReloadDelay;
            IsPlayer = gameObject.CompareTag(PlayerInput.Tag);
        }
        public void Update()
        {
            if (!CanShoot)
            {
                CurrentReloadDelay -= Time.deltaTime;
                if (CurrentReloadDelay < 0f)
                {
                    CanShoot = true;
                    //Debug.Log($"4 Update CurrentReloadDelay CanShoot = {CanShoot}");
                }
            }
        }

        public void HandleFire()
        {
            if (!CanShoot )
            {
                return;
            }
            if (IsPlayer && Input.GetKeyDown(KeyCode.Space) || !IsPlayer)
            {
                CanShoot = false;
                CurrentReloadDelay = ReloadDelay;
                _lastBulletController.Hit.RemoveAllListeners();
                var bulletPrefab = CreateBullet();

                Physics2D.IgnoreCollision(bulletPrefab.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());


                _lastBulletController = bulletPrefab.GetComponent<BulletController>();
                _lastBulletController.Hit.AddListener(delegate { OnBulletHit(_lastBulletController); });
                _lastBulletController.ParentTagName = gameObject.tag;
                _lastBulletController.Parent = gameObject;
                _lastBulletController.MoveBullet();
            }
        }



        private void OnBulletHit(BulletController controller)
        {
            controller.Hit.RemoveAllListeners();
            if (CurrentReloadDelay > 0.5f)
            {

                CurrentReloadDelay = IsPlayer ? 0.1f : 0.5f;
            }
            //CanShoot = true;
            //CurrentReloadDelay = ReloadDelay;
        }

        private GameObject CreateBullet()
        {
            var start = gameObject.transform.position;
            var z = transform.localEulerAngles.z;
            var frontCenterTank = start + Quaternion.Euler(0, 0, z) * Vector3.up * BaseMovingObject.MapTankWidth / 2;
            var bulletPrefab = Instantiate(bullet, frontCenterTank, Quaternion.Euler(0, 0, z));
            return bulletPrefab;
        }
    }
}
