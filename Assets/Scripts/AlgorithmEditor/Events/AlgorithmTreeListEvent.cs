using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.AlgorithmEditor.Events
{
    public class AlgorithmTreeListEvent : UnityEvent<IReadOnlyCollection<AlgorithmBlockUI>>
    {
    }
}
