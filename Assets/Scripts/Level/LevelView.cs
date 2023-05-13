using Camera;
using Game;
using Game.Level;
using Handlers;
using SkillEffects;
using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelView : MonoBehaviour
{
    [SerializeField] private CameraView camera;
    [SerializeField] private Entity player;
    [SerializeField] private GlobalClock globalClock;
    [SerializeField] private UIManager uiManager;

    public InputActionReference mousePosition;
    public InputActionReference createEnemy;
    public Entity entityPrefab;
    private SkillEffectManager skillEffectManager;

    public LevelManager LevelManager { get; private set; }

    private void Start()
    {
        skillEffectManager = new SkillEffectManager(globalClock);
        LevelManager = new LevelManager(globalClock, skillEffectManager);
        camera.Setup(player.transform);

        uiManager.Setup(LevelManager.poolManager, globalClock);
        SetupEntitiesInScene();

        createEnemy.action.Enable();
        mousePosition.action.Enable();

        createEnemy.action.started += OnCreateEnemy;
    }

    private void OnDestroy()
    {
        createEnemy.action.started -= OnCreateEnemy;
        skillEffectManager.Dispose();
    }

    private void OnCreateEnemy(InputAction.CallbackContext _)
    {
        if (RaycastMousePosition(out var hit))
        {
            var instance = Instantiate(entityPrefab);
            instance.transform.position = hit.point;
            instance.Setup(this);
        }
    }

    private void SetupEntitiesInScene()
    {
        var entities = FindObjectsOfType<Entity>();
        foreach (var entity in entities)
            entity.Setup(this);
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

    public void RegisterEntity(Entity entity)
    {
        EntityColliderRegistry.Register(entity, entity.GetComponent<Collider>());
        uiManager.RegisterEntity(entity);
    }

    public void UnRegisterEntity(Entity entity)
    {
        // todo: I dont like this
        skillEffectManager.DisposeAll(entity);
        EntityColliderRegistry.UnRegister(entity.GetComponent<Collider>());
        uiManager.UnRegisterEntity(entity);
    }
}
