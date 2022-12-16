using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKiller : ObjectiveHandler
{

    protected override void OnEnable() {
        base.OnEnable();
        EnemiesManager.OnEnemyAliveChange += setCompleted;
    }

    public void setCompleted(int enemiesAlive)
    {
        if(enemiesAlive == 0) SetToCompleted();
    }

    protected override void OnDisable() {
        base.OnDisable();
        EnemiesManager.OnEnemyAliveChange -= setCompleted;
    }
}
