using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "Vector3IntEvent", menuName = "Voodoo/Events/Vector3IntEvent")]
    public class Vector3IntEvent : GenericEvent<Vector3Int> { }

    [Serializable]
    public class Vector3IntEventSource : GenericEventSource<Vector3IntEvent, Vector3Int> { }
}