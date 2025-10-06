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
            
            player = this.GetComponentInParent<Player>();
        }

        private void Start()
        {
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
            anim = this.GetComponent<Animator>();

            if (player.FlipToRight == true)
                anim.Play("Attack");
            else
            {
                anim.Play("AttackLeft");
            }

            MusicManager.Instance.PlaySound("我左键击打的音效3");
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
