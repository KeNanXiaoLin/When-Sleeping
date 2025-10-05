using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowPanel<GameStartUI>();
        // MusicManager.Instance.PlaySound("1");
        MusicManager.Instance.PlayBKMusic("1");
    }
}
