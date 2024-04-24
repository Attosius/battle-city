using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseFireController : MonoBehaviour
    {
        public GameObject bullet;
        public float ReloadDelay = 1.5f; //sec
        public float CurrentReloadDelay = 0f;
        public bool CanShoot = true;

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
            if (CanShoot && Input.GetKeyDown(KeyCode.Space))
            {
                CanShoot = false;
                CurrentReloadDelay = ReloadDelay;

                var bulletPrefab = CreateBullet();

                Physics2D.IgnoreCollision(bulletPrefab.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());


                var controller = bulletPrefab.GetComponent<BulletController>();
                controller.ParentTagName = gameObject.tag;
                controller.Parent = gameObject;
                controller.MoveBullet();
            }
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
