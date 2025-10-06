using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener
{
    #region 游戏主要运行环节中会触发的相关事件
    public delegate void GameStatsChange();

    public static event GameStatsChange OnGameWin;
    public static event GameStatsChange OnGameLose;

    public static void GameWin() => OnGameWin?.Invoke();
    public static void GameLose() => OnGameLose?.Invoke();
    #endregion


    #region 玩家和敌人伤害相关事件
    public delegate void Damage();
    public static event Damage OnPlayerDamage;

    public delegate void Damage_Enemy(float _Damage);
    public static event Damage_Enemy OnEnemyDamage;

    public static void PlayerDamage() => OnPlayerDamage?.Invoke();
    public static void EnemyDamage(float _damage) => OnEnemyDamage?.Invoke(_damage);
    #endregion

    #region 村民对话相关事件
    public delegate void Dialogue();
    public static event Dialogue OnDialogueStart;
    public static event Dialogue OnDialogueEnd;

    public static void DialogueStart() => OnDialogueStart?.Invoke();
    public static void DialogueEnd() => OnDialogueEnd?.Invoke();
    #endregion

    #region 场景切换相关事件 
    public delegate void SceneSwitch(string _SceneName);
    public static event SceneSwitch OnVilliageSceneChange;
    public static event SceneSwitch OnBattleSceneChange;
    public static event SceneSwitch OnSceneReload;

    public static void VilliageSceneChange(string _SceneName) => OnVilliageSceneChange?.Invoke(_SceneName);
    public static void BattleSceneChange(string _SceneName) => OnBattleSceneChange?.Invoke(_SceneName);
    public static void ReloadScene(string _SceneName) => OnSceneReload?.Invoke(_SceneName);

    #endregion

    public delegate void GetItem();

    public static event GetItem OnItemGot;
    public static event GetItem OnCheckedItemGot;

    public static void ItemGot() => OnItemGot.Invoke();
    public static void CheckedItemGot() => OnCheckedItemGot.Invoke();
}
