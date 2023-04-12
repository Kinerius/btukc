using UnityEngine;

namespace Entities.Views
{
    public class Anchor : MonoBehaviour
    {
        [SerializeField] private string anchorName;

        public string GetAnchorName() => anchorName;
    }
}