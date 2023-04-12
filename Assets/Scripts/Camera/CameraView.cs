using System;
using UnityEngine;

namespace Camera
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera cameraReference;
        [SerializeField] private float lerpSpeed = 10;
        private Transform target;
        public new UnityEngine.Camera camera => cameraReference;

        private void Update()
        {
            if (target == null) return;
            var cameraTransform = transform;
            var targetTransform = target.transform;
            var position = cameraTransform.position;
            var targetPosition = targetTransform.position;
            
            var additionalSpeed = Vector3.Distance(position, targetPosition);
            position = Vector3.MoveTowards(position, targetPosition, (lerpSpeed + additionalSpeed) * Time.deltaTime);
            cameraTransform.position = position;
        }

        public void Setup(Transform targetTransform)
        {
            target = targetTransform;
            transform.position = target.position;
        }
    }
}