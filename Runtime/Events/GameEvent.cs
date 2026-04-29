using System;
using Capibutler.Events.Base;
using UnityEngine;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Voodoo/Events/GameEvent")]
    public class GameEvent : GenericEvent { }

    [Serializable]
    public class GameEventSource : GenericEventSource<GameEvent> { }
}