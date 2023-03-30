using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Controllers/PlayerEntityController", fileName = "PlayerEntityController", order = 0)]
    public class PlayerEntityController : EntityController
    {
        public float tempSpeed = 5;
        private CharacterController characterController;
        private CollisionFlags lastFlags;

        public override void Initialize(Entity entity)
        {
            characterController = entity.GetComponent<CharacterController>();
        }

        public override void Update()
        {
            Vector3 direction = new Vector3();

            if (Input.GetKey(KeyCode.W))
            {
                direction.z = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction.z = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction.x = 1;
            }

            direction = direction.normalized * tempSpeed * Time.deltaTime;
            direction.y = Physics.gravity.y * Time.deltaTime;
            lastFlags = characterController.Move(direction);
        }
    }
}