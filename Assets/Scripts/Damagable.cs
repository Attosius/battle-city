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

        //spriteRenderer.sprite = sprite;
        //boxCollider2D.bounds.
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
        
    }

    public Vector3 contact = Vector3.zero;
    public int count = 0;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D {collision.otherCollider.name}");
        int i = 0;
        foreach (var contact2 in collision.contacts)
        {
            contact = contact2.point;
            //Gizmos.DrawSphere(contact2.point, 0.02f);
            Debug.Log($"{i++} {contact2.point}");
            Debug.DrawRay(contact2.point, contact2.normal, Color.white, 20, true);
        }

        //ContactPoint2D contact = collision.contacts[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 pos = contact.point;
        //Instantiate(explosionPrefab, pos, rot);
        //Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        if (count > 0)
        {
            Gizmos.color = Color.red;
        }
        if (count > 1)
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawSphere(contact, 0.02f);
        if (contact != Vector3.zero)
        {

            count++;
        }
    }
}
