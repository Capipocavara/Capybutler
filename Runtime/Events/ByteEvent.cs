using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "ByteEvent", menuName = "Capipocavara/Events/ByteEvent")]
    public class ByteEvent : GenericEvent<byte> { }

    [Serializable]
    public class ByteEventSource : GenericEventSource<ByteEvent, byte> { }
}