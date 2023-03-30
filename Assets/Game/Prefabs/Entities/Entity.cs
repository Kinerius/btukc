using System;
using DefaultNamespace;
using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private EntityController entityController;

        private void Start()
        {
            entityController.Initialize(this);
        }

        private void Update()
        {
            entityController.Update();
        }
    }
}


