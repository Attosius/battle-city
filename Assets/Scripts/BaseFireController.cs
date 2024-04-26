using Assets.Scripts.TanksData;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseFireController : MonoBehaviour
    {
        public TurretPropertiesData TurretProperties;
        public GameObject bullet;
        //public float ReloadDelay = 1.5f; //sec
        //public float ReloadDelayAfterHitPlayerSec = 0.25f;
        //public float ReloadDelayAfterHitEnemySec = 0.5f;
        //public float ReloadDelayAfterHitBulletSec = 0.05f;

        public float CurrentReloadDelay = 0f;
        public bool CanShoot = false;
        public bool IsPlayer = false;

        private BulletController _lastBulletController;

        public void Awake()
        {
            CurrentReloadDelay = TurretProperties.ReloadDelay;
            IsPlayer = gameObject.CompareTag(PlayerInput.Tag);
        }
        public void Start()
        {
            //CurrentReloadDelay = TurretProperties.ReloadDelay;
            //IsPlayer = gameObject.CompareTag(PlayerInput.Tag);
        }

        public void Update()
        {
            if (!CanShoot)
            {
                CurrentReloadDelay -= Time.deltaTime;
                if (CurrentReloadDelay < 0f)
                {
                    CanShoot = true;
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
                CurrentReloadDelay = TurretProperties.ReloadDelay;
                if (_lastBulletController != null)
                {
                    _lastBulletController.Hit.RemoveAllListeners();
                }
                var bulletPrefab = CreateBullet();

                Physics2D.IgnoreCollision(bulletPrefab.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

                var newBulletController = bulletPrefab.GetComponent<BulletController>();
                newBulletController.Hit.AddListener(collisionTag => { OnBulletHit(newBulletController, collisionTag); });
                newBulletController.ParentTagName = gameObject.tag;
                newBulletController.Parent = gameObject;
                newBulletController.MoveBullet();
                _lastBulletController = newBulletController;
            }
        }


        private void OnBulletHit(BulletController controller, string collisionTag)
        {
            Debug.Log($"From {controller.Parent.tag} to = {collisionTag}");
            controller.Hit.RemoveAllListeners();
            if (collisionTag == BulletController.Tag)
            {
                if (CurrentReloadDelay > TurretProperties.ReloadDelayAfterHitBulletSec)
                {
                    CurrentReloadDelay = TurretProperties.ReloadDelayAfterHitBulletSec;
                }
                return;
            }

            
            var decreasedDelay = IsPlayer ? TurretProperties.ReloadDelayAfterHitPlayerSec : TurretProperties.ReloadDelayAfterHitEnemySec;
            if (CurrentReloadDelay > decreasedDelay)
            {
                CurrentReloadDelay = decreasedDelay;
            }
        }

        private GameObject CreateBullet()
        {
            var start = gameObject.transform.position;
            var z = transform.localEulerAngles.z;
            var frontCenterTank = start + Quaternion.Euler(0, 0, z) * Vector3.up * TankPropertiesData.MapTankWidth / 2;
            var bulletPrefab = Instantiate(bullet, frontCenterTank, Quaternion.Euler(0, 0, z));
            return bulletPrefab;
        }
    }
}
