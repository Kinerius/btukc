using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Level;
using SkillActions;
using Stats;
using UnityEngine;

#pragma warning disable CS4014

public class SkillAction
{
    private readonly List<IAction> sequences;
    private readonly StatRepository statRepository;
    public Cooldown cooldown { get; private set; }
    public bool isOnUse { get; private set; }

    public SkillAction(SkillActionData skillActionData)
    {
        statRepository = new StatRepository(skillActionData.abilityStats);
        sequences = skillActionData.actions.Select(a => (IAction)a).ToList();
    }

    public void Setup(GlobalClock globalClock)
    {
        var cooldownStat = statRepository.GetStat("cooldown");
        var rateOfFireStat = statRepository.GetStat("rateoffire");

        if (cooldownStat != null)
        {
            cooldown = new Cooldown(globalClock, TimeSpan.FromSeconds(cooldownStat.Value));
        }
        else if (rateOfFireStat != null)
        {
            cooldown = new Cooldown(globalClock, TimeSpan.FromSeconds(1 / rateOfFireStat.Value));
        }
        else
        {
            Debug.LogWarning("This skill has no cooldown");
        }
    }

    public void StartSkillAction(SkillActionTriggerData data, LevelManager levelManager)
    {
        if (!cooldown.IsUsable()) return;

        cooldown.Use();
        StartSkillAction(data, levelManager, statRepository);
    }

    private async UniTask StartSkillAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository repository)
    {
        isOnUse = true;

        try
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                var sequence = sequences[i];

                await sequence.StartAction(data, levelManager, repository);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            isOnUse = false;
        }

    }

    
}
