using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rd { get; private set; }
    public Animator anim { get; private set; }


    [SerializeField] private float MoveSpeed;

    [Header("攻击相关")]
    [SerializeField] private float PlayerDamage;
    [SerializeField] private float PlayerAttackTime;

    [Header("跳跃相关")]
    [SerializeField] private float PlayerJumpForce;
    [SerializeField] private float PlayerGroundDetectDistence;
}
}
