using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerAttackRate : MonoBehaviour
    {

        List<Collider2D> EnteredEnemys = new List<Collider2D>();
        private float PlayerDamage;

        private void Start()
        {
            PlayerDamage = this.GetComponentInParent<Player>().GetAttackRate_Player();

            EventListener.OnPlayerDamage += OnPlayerAttack;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                EnteredEnemys.Add(collision);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                EnteredEnemys.Remove(collision);
            }
        }


        void OnPlayerAttack()
        {
            foreach (Collider2D enemy in EnteredEnemys)
            {
                enemy.gameObject.GetComponent<Enemy>().EnemyGetDamage(PlayerDamage);              
            }
        }


    }
}
