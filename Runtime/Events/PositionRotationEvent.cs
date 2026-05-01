using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "PositionRotationEvent", menuName = "Capipocavara/Events/PositionRotationEvent")]
    public class PositionRotationEvent : GenericEvent<Vector3, Quaternion> { }

    [Serializable]
    public class PositionRotationEventSource : GenericEventSource<PositionRotationEvent, Vector3, Quaternion> { }
}