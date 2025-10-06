using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerAttackRate : MonoBehaviour
    {

        List<Collider2D> EnteredEnemys = new List<Collider2D>();
        private Player player;
        private float PlayerDamage;
        private Animator anim;

        void Awake()
        {
        }

        private void Start()
        {
            anim = this.GetComponent<Animator>();
            player = this.GetComponentInParent<Player>();
            PlayerDamage = player.GetAttackRate_Player();

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


        void OnPlayerAttack_Anim()
        {
            if (player.FlipToRight == true)
                anim.Play("Attack");
            else
            {
                anim.Play("AttackLeft");
            }
        }

        void AnimEnd()
        {
            anim.Play("AttackIdle");
        }

        void OnPlayerAttack()
        {
            foreach (Collider2D enemy in EnteredEnemys)
            {
                enemy.gameObject.GetComponent<Enemy>().EnemyGetDamage(PlayerDamage);
            }
            OnPlayerAttack_Anim();
        }


    }
}
