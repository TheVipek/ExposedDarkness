using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKiller : ObjectiveHandler
{

    private void OnEnable() {
        EnemiesManager.onEnemyAliveChange += setCompleted;
    }

    public void setCompleted()
    {
        if(!EnemiesManager.Instance.isAnyEnemyAlive()) gameObject.SetActive(false);
    }
    public override void OnDisable() {
        base.OnDisable();
        EnemiesManager.onEnemyAliveChange -= setCompleted;
    }
}
