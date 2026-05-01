using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Capipocavara/Events/FloatEvent")]
    public class FloatEvent : GenericEvent<float> { }

    [Serializable]
    public class FloatEventSource : GenericEventSource<FloatEvent, float> { }
}