using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Capibutler.Audio
{
    public class AudioEffectPlayer : MonoBehaviour
    {
        public enum SpatialType
        {
            TwoD = 0,
            ThreeD = 1
        }

        private const byte AudioSourcePoolSize = 30;
        private static Queue<AudioSource> audioSources;

        public AudioMixerGroup audioGroup;
        public AudioResource effect;

        public SpatialType spatialType = SpatialType.ThreeD;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        public Vector2 distance = new(10f, 100f);
        public bool loop;

        private AudioSource playingSource;

        [SerializeField] private bool looping;

        private void OnValidate()
        {
            looping = loop && effect is AudioClip;
            distance.y = Mathf.Max(distance.x, distance.y);
        }

        private void LateUpdate()
        {
            // clean up non looping not playing sources, looping sources must explicitly be set to !enabled to allow recovery of paused looping sources on focus loss  
            if (!playingSource || playingSource.isPlaying || (playingSource.loop && playingSource.enabled)) {
                return;
            }

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }

        private void OnDestroy()
        {
            if (!playingSource) {
                return;
            }

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus || !playingSource || !playingSource.enabled || !playingSource.loop || playingSource.isPlaying) {
                return;
            }

            // unpause potentially paused looping sources
            playingSource.UnPause();
        }

        public void Play()
        {
            if (!effect) {
                return;
            }

            if (playingSource) {
                ReturnToAudioSourcePool(playingSource);
                playingSource = null;
            }

            if (!GrabFromAudioSourcePool(out var source)) {
                return;
            }

            playingSource = source;
            source.Play();
        }

        public void Stop()
        {
            if (!playingSource) {
                return;
            }

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateAudioSourcePool()
        {
            if (!Application.isPlaying) {
                return;
            }

            audioSources = new Queue<AudioSource>(AudioSourcePoolSize);
            for (var i = 0; i < AudioSourcePoolSize; ++i) {
                var go = new GameObject("AudioSource") { hideFlags = HideFlags.HideAndDontSave };
                DontDestroyOnLoad(go);

                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.enabled = false;
                audioSources.Enqueue(source);
            }
        }

        private bool GrabFromAudioSourcePool(out AudioSource source)
        {
            if (!audioSources.TryDequeue(out source)) {
                return false;
            }

            source.enabled = true;
            source.resource = effect;
            source.outputAudioMixerGroup = audioGroup;
            source.rolloffMode = rolloffMode;
            source.minDistance = distance.x;
            source.maxDistance = distance.y;
            source.loop = looping;
            source.spatialBlend = spatialType switch {
                SpatialType.TwoD => 0f,
                SpatialType.ThreeD => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };

            var tf = source.transform;
            tf.parent = transform;
            tf.localPosition = Vector3.zero;

#if UNITY_EDITOR
            var go = source.gameObject;
            go.hideFlags &= ~HideFlags.HideInHierarchy;
            go.name += "[" + effect.name + "]";
#endif

            return true;
        }

        private static void ReturnToAudioSourcePool(AudioSource source)
        {
            source.Stop();
            source.enabled = false;
            source.resource = null;
            source.outputAudioMixerGroup = null;
            source.loop = false;
            source.volume = 1f;
            source.pitch = 1f;

            var tf = source.transform;
            tf.parent = null;
            tf.position = Vector3.zero;

#if UNITY_EDITOR
            var go = source.gameObject;
            go.hideFlags |= HideFlags.HideInHierarchy;
            go.name = go.name[..go.name.IndexOf('[')];
#endif

            audioSources.Enqueue(source);
        }
    }
}