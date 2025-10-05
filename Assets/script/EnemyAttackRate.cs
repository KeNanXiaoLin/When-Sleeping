using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GJ
{

    public class EnemyAttackRate : MonoBehaviour
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Enemy enemy;

        void Awake()
        {
            enemy = GetComponentInParent<Enemy>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            colliders.Add(collision);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            colliders.Remove(collision);
        }

        public void Anim_EnemyAttack()
        {
            foreach (Collider2D i in colliders)
            {
                if (i.GetComponent<Player>() == true)
                    EventListener.EnemyDamage(enemy.EnemyDamage);
            }

        }

    }

}