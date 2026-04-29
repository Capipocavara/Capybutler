using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Voodoo/Events/BoolEvent")]
    public class BoolEvent : GenericEvent<bool> { }

    [Serializable]
    public class BoolEventSource : GenericEventSource<BoolEvent, bool> { }
}