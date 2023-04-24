using Camera;
using Game;
using Game.Level;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private CameraView camera;
    [SerializeField] private Entity player;
    [SerializeField] private GlobalClock globalClock;
    public LevelManager LevelManager { get; private set; }

    private void Start()
    {
        LevelManager = new LevelManager(globalClock);
        camera.Setup(player.transform);
        var entities = FindObjectsOfType<Entity>();

        foreach (var entity in entities)
        {
            entity.Setup(this);
        }
    }

}
