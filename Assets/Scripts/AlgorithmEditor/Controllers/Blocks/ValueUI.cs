using UnityEngine;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using AlgorithmizmModels.Math;

namespace Algorithmizm
{
    public class ValueUI : MonoBehaviour
    {
        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private IValue _value;

        public IValue Value
        {
            get => _value;
            set
            {
                _value = value;
                RefreshValueView();
            }
        }

        private void RefreshValueView()
        {

        }
    }
}
