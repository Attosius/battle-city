using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class BulletController : MonoBehaviour
    {
        //public float Speed = 2;

        public Rigidbody2D Rb2D;
        public bool HisDisperse = false;

        public GameObject ExplosionPrefab;

        public LayerMask LayerBlocking;
        public string ParentTagName;
        public GameObject Parent;
        public UnityEvent<string> Hit = new UnityEvent<string>();
        public static int Count = 0;
        public int Id = 0;
        public static string Tag = "Bullet";
        void Awake()
        {
            Count++;
            Id = Count;
            Rb2D = GetComponent<Rigidbody2D>();
        }

        public GameObject GetCurrentObject()
        {
            return gameObject;
        }

        public void MoveBullet(float speed)
        {
            Rb2D.velocity = transform.up * speed;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"Collidered with {collision.name}");
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log($"OnCollisionEnter2D from Bullet to {collision.gameObject.name}");
            // todo bullet to bullet with different tags
            var target = collision.collider.gameObject;
            if (target.tag == "Shadow" || Parent == null)
            {
                return;
            }
            //var targetEnemyController = target.GetComponent<EnemyController>();
            //var parentEnemyController = Parent.GetComponent<EnemyController>();
            //if (targetEnemyController != null && parentEnemyController != null)
            //{
            //    if (targetEnemyController.Id != parentEnemyController.Id)
            //    {

            //    }
            //}
            if (target == Parent)
            {
                // self hit (ignore collision already check it)
                return;
            }

            if (target.CompareTag(Parent.tag))
            {
                // player hit player, enemy hit enemy
                DisableObject(Parent.tag);
            }

            if (collision.gameObject.CompareTag(Tag))
            {
                // player hit player, enemy hit enemy
                DisableObject(Tag);
            }

            string layerName = LayerMask.LayerToName(collision.gameObject.layer);
            if ((LayerBlocking & (1 << collision.gameObject.layer)) != 0)
            {
                //Debug.Log("LayerMask contains the layer: " + layerName);
                DisableObject(collision.gameObject.tag);
                Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            }

            //ContactPoint2D contact = collision.contacts[0];
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            //Vector3 pos = contact.point;
            //Instantiate(explosionPrefab, pos, rot);
            //Destroy(gameObject);
        }

        private void DisableObject(string collisionTag)
        {
            Rb2D.velocity = Vector2.zero;
            gameObject.SetActive(false);
            Hit?.Invoke(collisionTag);
            Destroy(this.gameObject);

        }

    }
}
