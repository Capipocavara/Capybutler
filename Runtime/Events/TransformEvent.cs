using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "TransformEvent", menuName = "Capipocavara/Events/TransformEvent")]
    public class TransformEvent : GenericEvent<Transform> { }

    [Serializable]
    public class TransformEventSource : GenericEventSource<TransformEvent, Transform> { }
}