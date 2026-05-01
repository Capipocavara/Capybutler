using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "StringFloatEvent", menuName = "Capipocavara/Events/StringFloatEvent")]
    public class StringFloatEvent : GenericEvent<string, float> { }

    [Serializable]
    public class StringFloatEventSource : GenericEventSource<StringFloatEvent, string, float> { }
}