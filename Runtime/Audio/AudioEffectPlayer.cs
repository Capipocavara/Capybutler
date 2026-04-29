using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Capibutler.Audio
{
    public class AudioEffectPlayer : MonoBehaviour
    {
        public enum SpatialType
        {
            [InspectorName("2D")]
            TwoD,

            [InspectorName("3D")]
            ThreeD
        }
        
        private const byte AudioSourcePoolSize = 30;
        
        private static Queue<AudioSource> audioSources;
#if UNITY_EDITOR
        private static AudioSource previewSource;
#endif

        public AudioMixerGroup audioGroup;
        public AudioResource effect;
        
        [Header("Settings")]
        [EnumButtons]
        public SpatialType spatialType = SpatialType.ThreeD;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        

        [Range(1f, 500f)]
        public float minDistance = 1f;


        [Range(1f, 500f)]
        public float maxDistance = 500f;


        public bool loop;

        private AudioSource playingSource;

        [SerializeField]
        [HideInInspector]
        private bool looping;

#if UNITY_EDITOR
        private void OnValidate()
        {
            looping = loop && EffectIsAudioClip;
        }
#endif

        private void LateUpdate()
        {
            // clean up non looping not playing sources, looping sources must explicitly be set to !enabled to allow recovery of paused looping sources on focus loss  
            if (!playingSource || playingSource.isPlaying || (playingSource.loop && playingSource.enabled))
                return;

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }

        private void OnDestroy()
        {
            if (!playingSource)
                return;

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus || !playingSource || !playingSource.enabled || !playingSource.loop || playingSource.isPlaying)
                return;

            // unpause potentially paused looping sources
            playingSource.UnPause();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateAudioSourcePool()
        {
            if (!Application.isPlaying)
                return;

            audioSources = new Queue<AudioSource>(AudioSourcePoolSize);
            for (var i = 0; i < AudioSourcePoolSize; ++i)
            {
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
            if (!audioSources.TryDequeue(out source))
                return false;

            source.enabled = true;
            source.resource = effect;
            source.outputAudioMixerGroup = audioGroup;
            source.rolloffMode = rolloffMode;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            source.loop = looping;
            source.spatialBlend = spatialType switch
            {
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

        public void Play()
        {
            if (!effect)
                return;

            if (playingSource)
            {
                ReturnToAudioSourcePool(playingSource);
                playingSource = null;
            }

            if (!GrabFromAudioSourcePool(out var source))
                return;

            playingSource = source;
            source.Play();
        }

        public void Stop()
        {
            if (!playingSource)
                return;

            ReturnToAudioSourcePool(playingSource);
            playingSource = null;
        }


#if UNITY_EDITOR
        // [ButtonGroup()]
        // [Button(SdfIconType.PlayFill, "")]
        // [HideInPlayMode]
        private void Preview()
        {
            if (previewSource == null)
            {
                previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioEffectPreview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
                previewSource.enabled = true;
                previewSource.loop = false;
            }

            if (previewSource.isPlaying)
                previewSource.Stop();

            previewSource.resource = effect;
            previewSource.outputAudioMixerGroup = audioGroup;
            previewSource.rolloffMode = rolloffMode;
            previewSource.minDistance = minDistance;
            previewSource.maxDistance = maxDistance;
            previewSource.spatialBlend = spatialType switch
            {
                SpatialType.TwoD => 0f,
                SpatialType.ThreeD => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };

            var tf = previewSource.transform;
            tf.parent = transform;
            tf.localPosition = Vector3.zero;

            previewSource.Play();
        }

        // [ButtonGroup()]
        // [Button(SdfIconType.StopFill, "")]
        // [EnableIf(nameof(PreviewIsPlaying))]
        // [HideInPlayMode]
        private void StopPreview()
        {
            if (PreviewIsPlaying)
                previewSource.Stop();
        }

        private bool PreviewIsPlaying => previewSource != null && previewSource.isPlaying;
        private bool EffectIsAudioClip => effect is AudioClip;
#endif
    }
}