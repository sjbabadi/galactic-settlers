using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    [SerializeField] float duration;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
