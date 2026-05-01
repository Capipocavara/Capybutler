using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Capipocavara/Events/IntEvent")]
    public class IntEvent : GenericEvent<int> { }

    [Serializable]
    public class IntEventSource : GenericEventSource<IntEvent, int> { }
}