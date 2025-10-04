using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener
{
    #region 游戏主要运行环节中会触发的相关事件
    public delegate void GameStatsChange();

    public static event GameStatsChange GameStart;
    public static event GameStatsChange GamePause;
    public static event GameStatsChange GameContiune;
    public static event GameStatsChange GameStop;
    public static event GameStatsChange GameRestart;
    #endregion


    #region 玩家和敌人伤害相关事件
    public delegate void Damage();
    public static event Damage OnPlayerDamage;

    //TODO 完成敌人对玩家造成伤害的逻辑
    public static event Damage OnEnemyDamage;

    public static void PlayerDamage() => OnPlayerDamage?.Invoke();
    public static void EnemyDamage() => OnEnemyDamage?.Invoke();
    #endregion

    #region 村民对话相关事件
    public delegate void Dialogue();
    public static event Dialogue OnDialogueStart;
    public static event Dialogue OnDialogueEnd;

    public static void DialogueStart() => OnDialogueStart?.Invoke();
    public static void DialogueEnd() => OnDialogueEnd?.Invoke();
    #endregion
}
