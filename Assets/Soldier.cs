using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private GameState gs;
    [SerializeField] private Map map;

    public int cost;
    public float power;
    public float health;
    public string faction;

    public int moveX;
    public int moveZ;

    private void Attack()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                Debug.Log("i: " + i.ToString() + "; k: " + k.ToString());
                GameObject rival = map.pieces[(int)transform.position.x + i, (int)transform.position.z + k];
                if (rival != null && rival.GetComponent<Soldier>().faction != faction)
                {
                    rival.GetComponent<Soldier>().TakeDamage(power);
                }
            }
        }
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

    void MoveSoldier(int x, int z)
    {
        float step = 10 * Time.deltaTime;
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 newPosition = currentPosition + new Vector3((float)x, 0f, (float)z);
        map.movePiece(currentPosition, newPosition);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, newPosition, step);
    }

    public void TakeAction()
    {
        if (moveX != 0 && moveZ != 0)
        {
            MoveSoldier(moveX, moveZ);
        }
        else
        {
            Attack();
        }
    }
}
