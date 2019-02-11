using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameState gs;
    [SerializeField] private Map map;

    public float health;
    public float power;

    private void Start()
    {
        health = 100;
        power = 30;
        gs.enemies.Add(gameObject.GetComponent<Enemy>());
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            map.deletePiece(transform.position);
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        foreach (Soldier enemy in gs.allies)
        {
            if (enemy != null)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) < 2f)
                {
                    enemy.TakeDamage(power);
                }
            }
        }
    }

    public void TakeAction()
    {
        Attack();
    }
}
