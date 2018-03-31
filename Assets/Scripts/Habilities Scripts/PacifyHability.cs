﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Pacify")]
public class PacifyHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Pacificar no disponible.");
                return;
            }
        }

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.controller.combatController.UpdatePlayerLog("¡Has usado Pacificar!");

            enemy.ChangeState(stateToChange);
        }

        WaitForCooldown();
    }
}