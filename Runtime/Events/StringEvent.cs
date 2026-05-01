using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "Capipocavara/Events/StringEvent")]
    public class StringEvent : GenericEvent<string> { }

    [Serializable]
    public class StringEventSource : GenericEventSource<StringEvent, string> { }
}