using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Level;
using SkillActions;
using Stats;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#pragma warning disable CS4014

public class SkillAction
{
    private readonly List<IAction> sequences;
    private readonly StatRepository statRepository;
    private bool isSetup;
    public Cooldown cooldown { get; private set; }
    public bool isOnUse { get; private set; }
    private CancellationTokenSource cancellationTokenSource;

    public SkillAction(SkillActionData skillActionData)
    {
        statRepository = new StatRepository(skillActionData.abilityStats);
        sequences = skillActionData.actions.Select(a => (IAction)a).ToList();
        ResetToken();
    }

    private void ResetToken()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void Setup(GlobalClock globalClock)
    {
        isSetup = true;
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

    public async UniTask StartSkillAction(SkillActionTriggerData data, LevelManager levelManager)
    {
        if (!isSetup) Debug.LogError("Skill was not setup");
        if (!cooldown.IsUsable()) return;
        ResetToken();
        cooldown.Use();
        await StartSkillAction(data, levelManager, statRepository, cancellationTokenSource.Token);
    }

    // TODO: connect cancellation token
    private async UniTask StartSkillAction(SkillActionTriggerData data, LevelManager levelManager, StatRepository repository, CancellationToken cancellationToken)
    {
        isOnUse = true;

        try
        {
            data.owner.ToggleActions(false, "SkillAction");
            foreach (var sequence in sequences)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await sequence.StartAction(data, levelManager, repository, cancellationToken);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            data.owner?.ToggleActions(true, "SkillAction");
            isOnUse = false;
        }
    }

    public void Interrupt()
    {
        ResetToken();
        cooldown.Reset();
    }
}
