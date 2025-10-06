using System.Collections;
using System.Collections.Generic;
using GJ;
using UnityEngine;  

public class BlackShadow : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;

    void FixedUpdate()
    {
        this.transform.Translate(new Vector2(1, 0) * MoveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == true)
        {
            EventListener.GameLose();
        }
    }
}
