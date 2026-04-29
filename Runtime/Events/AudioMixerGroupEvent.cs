using System;
using Capibutler.Events.Base;
using UnityEngine;
using UnityEngine.Audio;

namespace Capibutler.Events
{
    [CreateAssetMenu(fileName = "AudioMixerGroupEvent", menuName = "Voodoo/Events/AudioMixerGroupEvent")]
    public class AudioMixerGroupEvent : GenericEvent<AudioMixerGroup> { }

    [Serializable]
    public class AudioMixerGroupEventSource : GenericEventSource<AudioMixerGroupEvent, AudioMixerGroup> { }
}