using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "DoubleEvent", menuName = "Capipocavara/Events/DoubleEvent")]
    public class DoubleEvent : GenericEvent<double> { }

    [Serializable]
    public class DoubleEventSource : GenericEventSource<DoubleEvent, double> { }
}