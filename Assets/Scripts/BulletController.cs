using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 2;

    public Rigidbody2D rb2D;
    public bool HisDisperse = false;

    public GameObject ExplosionPrefab;

    public LayerMask LayerBlocking;
    public string ParentTagName;
    public GameObject Parent;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public GameObject GetCurrentObject()
    {
        return gameObject;
    }

    public void Create(Vector3 faceCenterTank, Quaternion rotation)
    {
        rb2D.velocity = transform.up * speed;
        //rb2D.isKinematic = false;
        //EditorApplication.isPaused = true;
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
        Debug.Log($"OnCollisionEnter2D from Bullet to {collision.gameObject.name}");
        // todo bullet to bullet with different tags
        var target = collision.collider.gameObject;
        if (target.tag == "Shadow" || Parent == null)
        {
            return;
        }
        var targetEnemyController = target.GetComponent<EnemyController>();
        var parentEnemyController = Parent.GetComponent<EnemyController>();
        if (targetEnemyController != null && parentEnemyController != null)
        {
            if (targetEnemyController.Id != parentEnemyController.Id)
            {

            }
        }
        if (target == Parent)
        {
            // self hit
            return;
        }

        if (target.tag == Parent.tag)
        {
            // player hit player, enemy hit enemy
            DisableObject();
        }
        if (collision.gameObject.tag == "Bullet")
        {
            // player hit player, enemy hit enemy
            DisableObject();
        }

        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if ((LayerBlocking & (1 << collision.gameObject.layer)) != 0)
        {
            //Debug.Log("LayerMask contains the layer: " + layerName);
            DisableObject();
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        }

        //ContactPoint2D contact = collision.contacts[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 pos = contact.point;
        //Instantiate(explosionPrefab, pos, rot);
        //Destroy(gameObject);
    }

    private void DisableObject()
    {
        rb2D.velocity = Vector2.zero;
        gameObject.SetActive(false);
        //Destroy(this.gameObject);

    }

}
