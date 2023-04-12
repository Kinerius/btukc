using UnityEngine;

public class GlobalClock : MonoBehaviour
{
    private GlobalClock instance;
    private float _time;
    public bool Enabled = true;
    public float time => _time;

    public float deltaTime
    {
        get
        {
            if (Enabled)
            {
                return Time.deltaTime;
            }

            return 0;
        }
    }
    
    public float fixedDeltaTime
    {
        get
        {
            if (Enabled)
            {
                return Time.fixedDeltaTime;
            }

            return 0;
        }
    }

    private void Update()
    {
        if (Enabled)
        {
            _time += Time.deltaTime;
        }
    }
}