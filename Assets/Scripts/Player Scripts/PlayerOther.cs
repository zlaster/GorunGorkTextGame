﻿using UnityEngine;

/// <summary>
/// "Otros" parametros del jugador. Cosas que se usan usualmente en combate.
/// </summary>
public class PlayerOther : MonoBehaviour {

    [SerializeField] private float defaultCriticalHitProbability = 0;
    [HideInInspector] public float currentCriticalHitProbability;

    [SerializeField] private float defaultCooldownReduction = 0;
	[HideInInspector] public float currentCooldownReduction;

    [SerializeField] private float defaultHealthRegenPerSecond = 2;
	[HideInInspector] public float currentHealthRegenPerSecond;

    [SerializeField] private float defaultTurnRegenPerSecond = 5;
	[HideInInspector] public float currentTurnRegenPerSecond;

    [SerializeField] private float defaultEvasion = 0;
	[HideInInspector] public float currentEvasion;

    private float escapeProbability = 0;


    public float DefaultCriticalHitProbability
    {
        get
        {
            return defaultCriticalHitProbability;
        }
    }

    public float DefaultCooldownReduction
    {
        get
        {
            return defaultCooldownReduction;
        }
    }

    public float DefaultHealthRegenPerSecond
    {
        get
        {
            return defaultHealthRegenPerSecond;
        }
    }

    public float DefaultTurnRegenPerSecond
    {
        get
        {
            return defaultTurnRegenPerSecond;
        }
    }

    public float DefaultEvasion
    {
        get
        {
            return defaultEvasion;
        }
    }

    public void BunnyEvasion(float newEvasion)
    {
        defaultEvasion = newEvasion;
        currentEvasion = defaultEvasion;
    }


    /// <summary>
    /// Inicia las variables extras del jugador según sus defaults.
    /// </summary>
    public void InitializeOthers()
    {
        currentCooldownReduction = defaultCooldownReduction;
        currentEvasion = defaultEvasion;
        currentHealthRegenPerSecond = defaultHealthRegenPerSecond;
        currentTurnRegenPerSecond = defaultTurnRegenPerSecond;
    }

    public float EscapeProbability(PlayerManager player, EnemyNPC enemy)
    {
        //escapeProbability =
        //    (((enemy.myTemplate.MaxHealth - enemy.currentHealth) / enemy.myTemplate.MaxHealth) * 100) +
        //    (player.characteristics.currentDexterity / 10) +
        //    (player.currentState.magnitude) -
        //    (enemy.currentState.magnitude);

		return 100;
    }

    public float EscapeProbability(PlayerManager player, PlayerInstance enemy)
    {
        //escapeProbability =
        //    (((enemy.enemyStats.maxHealth - enemy.currentHealth) / enemy.enemyStats.maxHealth) * 100) +
        //    (player.characteristics.currentDexterity / 10) +
        //    (player.currentState.magnitude) -
        //    (enemy.playerState.magnitude);

        return 100;
     
    }

    public float CriticalHitProbability(PlayerManager player)
    {
        currentCriticalHitProbability = (defaultCriticalHitProbability +
            (player.characteristics.currentDexterity * 0.01f));

        return currentCriticalHitProbability;
    }

    public void ReduceEvasionBySecond()
    {
        if (currentEvasion > defaultEvasion)
        {
            currentEvasion--;
        }
        else
        {
            CancelInvoke();
        }
    }

	public void modifyCritBy(float amount) {
		currentCriticalHitProbability += amount;
	}

	public void modifyCoolDownBy(float amount) {
		currentCooldownReduction += amount;
	}

	public void modifyHealthRegenBy(float amount) {
		currentHealthRegenPerSecond += amount;
	}

	public void modifyTurnRegenBy(float amount) {
		currentTurnRegenPerSecond += amount;
	}

	public void modifyEvasionBy(float amount) {
		currentEvasion += amount;
	}

}
