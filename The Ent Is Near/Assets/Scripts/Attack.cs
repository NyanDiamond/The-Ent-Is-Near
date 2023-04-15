using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] bool ranged;
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.GetComponent<Collider2D>(), true);
            enemy.Damaged(damage, ranged);
        }
    }
}
