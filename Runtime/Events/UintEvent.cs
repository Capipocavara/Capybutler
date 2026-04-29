using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "UintEvent", menuName = "Voodoo/Events/UintEvent")]
    public class UintEvent : GenericEvent<uint> { }

    [Serializable]
    public class UintEventSource : GenericEventSource<UintEvent, uint> { }
}