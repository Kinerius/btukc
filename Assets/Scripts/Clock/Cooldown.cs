using System;

public class Cooldown
{
    private TimeSpan cooldown;
    private float lastUsageTime;
    private GlobalClock globalClock;
    
    // TODO: refactor this class so its managed by GlobalClock, something like GlobalClock.GetCooldown(TimeSpan) sounds nice
    // This would make the class less lazy and could have events, controlled progress and modificable durations for skills
    // A cooldown should also have an entity as a target so we can get their stats
    public Cooldown(GlobalClock globalClock, TimeSpan cooldown, bool startOnCooldown = false)
    {
        this.globalClock = globalClock;
        this.cooldown = cooldown;

        if (startOnCooldown)
        {
            Use();
        }
        else
        {
            Reset();
        }
        
    }

    public void Reset()
    {
        lastUsageTime = globalClock.time - (float)cooldown.TotalSeconds;
    }

    public bool IsUsable()
    {
        return globalClock.time - lastUsageTime > cooldown.TotalSeconds;
    }

    public void Use()
    {
        lastUsageTime = globalClock.time;
    }
}