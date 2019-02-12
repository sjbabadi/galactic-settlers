using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Character
{
    [SerializeField] private GameState gs;
    [SerializeField] private Tilemap map;

    public int cost;
    public float power;
    public float health;
    public string faction;

    public int moveX;
    public int moveZ;

    private void Start()
    {
        Init();

        health = 100;
        power = 15;

        
        gs.allies.Add(gameObject.GetComponent<Soldier>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
    }


    void CheckMouse()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tilemap t = hit.collider.GetComponent<Tilemap>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }


    }


    private void Attack()
    {
        foreach (Enemy enemy in gs.enemies)
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
