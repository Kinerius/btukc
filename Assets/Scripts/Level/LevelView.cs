using Camera;
using Game;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private CameraView camera;
    [SerializeField] private Entity player;
    void Start()
    {
        camera.Setup(player.transform);
    }

}
