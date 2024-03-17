using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{

    public int MaxHealth;

    public int Health;

    public int MaxArmor;
    public int Armor;
    public bool IsDisperse = false;
    public UnityEvent Death { get; set; }

    void Awake()
    {
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
        }
    }

}
