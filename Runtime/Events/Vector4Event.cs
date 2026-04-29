using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "Vector4Event", menuName = "Voodoo/Events/Vector4Event")]
    public class Vector4Event : GenericEvent<Vector4> { }

    [Serializable]
    public class Vector4EventSource : GenericEventSource<Vector4Event, Vector4> { }
}