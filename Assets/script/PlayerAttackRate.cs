using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

    public class PlayerAttackRate : MonoBehaviour
    {

        List<Collision2D> EnteredEnemys;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                EnteredEnemys.Add(collision);
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                EnteredEnemys.Remove(collision);
            }
        }

        void OnPlayerAttack()
        {
            foreach (Collision2D enemy in EnteredEnemys)
            {
                Debug.Log("Emeny Get Attack");
            }
        }


    }
}
