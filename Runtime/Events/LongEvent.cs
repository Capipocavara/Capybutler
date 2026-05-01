using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "LongEvent", menuName = "Capipocavara/Events/LongEvent")]
    public class LongEvent : GenericEvent<long> { }

    [Serializable]
    public class LongEventSource : GenericEventSource<LongEvent, long> { }
}