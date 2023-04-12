using System.Collections.Generic;
using System.Linq;
using Character;
using Entities.Animation;
using Entities.Views;
using JetBrains.Annotations;
using UnityEngine;

namespace Entities
{
    public abstract class EntityView : MonoBehaviour
    {
        [SerializeField] private EntityAnimationDataPack animationDataPack;
        [SerializeField] private UnityEngine.Animation animationComponent;
        
        private Dictionary<string,Transform> anchors;
        private EntityAnimationController animationController;

        public void Awake()
        {
            anchors = GetComponentsInChildren<Anchor>().ToDictionary(a => a.GetAnchorName(), a => a.transform);
            animationController = new EntityAnimationController(animationComponent, animationDataPack);
            OnAwake();
        }

        [UsedImplicitly]
        public void AnimationEvent(string eventName)
        {
            animationController.RaiseAnimationEvent(eventName);
        }

        public EntityAnimationController GetAnimationController() => animationController;
        
        public Transform GetAnchor(string anchorTag)
        {
            if (anchors.ContainsKey(anchorTag)) return anchors[anchorTag];
            Debug.LogWarning($"This EntityView has no anchor named <b>{anchorTag}</b>", this);

            return transform;
        }

        public abstract void OnAwake();
        public abstract void StopMoving();
        public abstract void LookAt(Vector3 targetPosition);

        public abstract void Teleport(Vector3 targetPosition);
        public abstract void SetDestination(Vector3 position);
    }
}