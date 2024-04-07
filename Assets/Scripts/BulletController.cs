using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5;

    public Rigidbody2D rb2D;
    public bool HisDisperse = false;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Create(Vector3 faceCenterTank, Quaternion rotation)
    {
        rb2D.velocity = transform.up * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Collidered with {collision.name}");
        //var damageable = collision.GetComponent<Damagable>();
        //if (damageable != null)
        //{
        //    var damage = 1;
        //    damageable.OnHit(damage);
        //    if (!damageable.IsDisperse || this.HisDisperse)
        //    {
        //        DisableObject();
        //    }
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D into Bullet {collision.gameObject.name}");
        DisableObject();
        //ContactPoint2D contact = collision.contacts[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 pos = contact.point;
        //Instantiate(explosionPrefab, pos, rot);
        //Destroy(gameObject);
    }

    private void DisableObject()
    {
        rb2D.velocity = Vector2.zero;
        gameObject.SetActive(false); // todo set pool
        
    }

}
