using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{

    public int MaxHealth;

    public int Health;

    public int MaxArmor;
    public int Armor;
    public bool IsDisperse = false;
    public UnityEvent Death = new UnityEvent();

    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;
    public Sprite sprite;
    private float MapTankWidth = 0.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        Health = MaxHealth;
        Armor = MaxArmor;
    }

    public void OnHit(int damage)
    {
        if (Armor > 0)
        {
            Armor -= damage;
        }
        else
        {
            Health -= damage;
        }

        if (Health < 1)
        {
            Debug.Log($"Death {gameObject.name}");
            Death?.Invoke();
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collidered with {collision.name}");
        //var center = gameObject.transform.position;
        //var perpendicular = Vector2.Perpendicular(collision.attachedRigidbody.velocity.normalized);
        //var perpendicular2 = Vector2.Perpendicular(collision.attachedRigidbody.velocity.normalized) * -1;
        ////Physics.SyncTransforms();

        //int results;
        //var p1 = perpendicular * 0.125f;
        //var p2 = perpendicular2 * 0.125f;
        //var hit1 = Physics2D.Raycast(center, perpendicular, 0.125f);
        //var hit2 = Physics2D.Raycast(center, perpendicular2, 0.125f);
    }
    
    //public Vector3 contactHit = Vector3.zero;
    //public static int count = 0;
    //public int i = 0;
    void OnCollisionEnter2D(Collision2D collision)
    {
        var bulletController = collision.gameObject.GetComponent<BulletController>();
        if (bulletController == null)
        {
            return;
        }
        Debug.Log($"OnCollisionEnter2D {collision.otherCollider.name}");
        foreach (var contact2 in collision.contacts)
        {
            var center = contact2.otherCollider.gameObject.transform.position;
            var perpendicular = Vector2.Perpendicular(contact2.relativeVelocity.normalized);
            var perpendicular2 = Vector2.Perpendicular(contact2.relativeVelocity.normalized) * -1;
            //Physics.SyncTransforms();

            //int results;
            //var p1 = perpendicular * 0.125f;
            //var p2 = perpendicular2 * 0.125f;
            //if (count % 2 == 0)
            //{
            //    Debug.DrawRay(center, p1, Color.white, 10);
            //    Debug.DrawRay(center, p2, Color.red, 10);
            //}
            //else
            //{
            //    Debug.DrawRay(center, p1, Color.blue, 20);
            //    Debug.DrawRay(center, p2, Color.green, 20);
            //}


            // wall right + wall left damage
            var hit1 = Physics2D.Raycast(center, perpendicular, MapTankWidth / 4f);
            var hit2 = Physics2D.Raycast(center, perpendicular2, MapTankWidth / 4f);

            if (hit1.transform != null)
            {
                
                Debug.Log($"Hit damage {hit1.transform}");
                //contactHit = hit1.transform.position;
                var damageable = hit1.transform.gameObject.GetComponent<Damagable>();
                if (damageable != null)
                {
                    var damage = 1;
                    damageable.OnHit(damage);
                }
            }
            if (hit2.transform != null)
            {
                var damageable = hit2.transform.gameObject.GetComponent<Damagable>();
                if (damageable != null)
                {
                    var damage = 1;
                    damageable.OnHit(damage);
                }
            }
            OnHit(1);
            return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(contactHit, 0.03f);
    }
}
