using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "UlongEvent", menuName = "Capipocavara/Events/UlongEvent")]
    public class UlongEvent : GenericEvent<ulong> { }

    [Serializable]
    public class UlongEventSource : GenericEventSource<UlongEvent, ulong> { }
}