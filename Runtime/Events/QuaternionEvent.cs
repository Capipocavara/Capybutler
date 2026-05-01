using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "QuaternionEvent", menuName = "Capipocavara/Events/QuaternionEvent")]
    public class QuaternionEvent : GenericEvent<Quaternion> { }

    [Serializable]
    public class QuaternionEventSource : GenericEventSource<QuaternionEvent, Quaternion> { }
}