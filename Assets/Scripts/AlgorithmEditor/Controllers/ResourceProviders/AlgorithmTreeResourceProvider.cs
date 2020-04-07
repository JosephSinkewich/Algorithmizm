using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using UnityEngine;

namespace Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders
{
    [CreateAssetMenu(fileName = "AlgorithmTreeResourcesProvider", menuName = "AlgorithmEditor")]
    public class AlgorithmTreeResourceProvider : ScriptableObject
    {
        [SerializeField] private AlgorithmBlockUI _algorithmBlockPrefab;

        public AlgorithmBlockUI AlgorithmBlockPrefab => _algorithmBlockPrefab;
    }
}
