using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "Vector3Event", menuName = "Voodoo/Events/Vector3Event")]
    public class Vector3Event : GenericEvent<Vector3> { }

    [Serializable]
    public class Vector3EventSource : GenericEventSource<Vector3Event, Vector3> { }
}