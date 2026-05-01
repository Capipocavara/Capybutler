using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "Vector2IntEvent", menuName = "Capipocavara/Events/Vector2IntEvent")]
    public class Vector2IntEvent : GenericEvent<Vector2Int> { }

    [Serializable]
    public class Vector2IntEventSource : GenericEventSource<Vector2IntEvent, Vector2Int> { }
}