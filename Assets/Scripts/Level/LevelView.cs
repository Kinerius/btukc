using Camera;
using Game;
using Game.Level;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelView : MonoBehaviour
{
    [SerializeField] private CameraView camera;
    [SerializeField] private Entity player;
    [SerializeField] private GlobalClock globalClock;

    public InputActionReference mousePosition;
    public InputActionReference createEnemy;
    public Entity entityPrefab;

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

        createEnemy.action.Enable();
        mousePosition.action.Enable();

        createEnemy.action.started += _ =>
        {
            if (RaycastMousePosition(out var hit))
            {
                var instance = Instantiate(entityPrefab);
                instance.transform.position = hit.point;
                instance.Setup(this);
            }
        };
    }

    private bool RaycastMousePosition(out RaycastHit hit)
    {
        var pos = mousePosition.action.ReadValue<Vector2>();
        if (pos.Equals(Vector2.zero))
        {
            hit = new RaycastHit();
            return false;
        }

        var worldPoint = UnityEngine.Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(worldPoint, out hit, 100, GetLayerMask()))
            return true;

        return false;
    }

    private int GetLayerMask()
    {
        // everything but
        return ~LayerMask.GetMask(new[]
        {
            "Character",
            "Enemy",
            "Projectile"
        });
    }

}
