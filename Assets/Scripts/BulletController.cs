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
        //Instantiate(gameObject, faceCenterTank, rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collidered with {collision.name}");
        var damageable = collision.GetComponent<Damagable>();
        if (damageable != null)
        {
            var damage = 1;
            damageable.OnHit(damage);
            if (!damageable.IsDisperse || this.HisDisperse)
            {
                DisableObject();
            }
        }
    }

    private void DisableObject()
    {
        rb2D.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
