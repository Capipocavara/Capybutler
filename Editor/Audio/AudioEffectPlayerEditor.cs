using System;
using Capibutler.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capibutler.Editor.Audio
{
    [CustomEditor(typeof(AudioEffectPlayer))]
    public class AudioEffectPlayerEditor : UnityEditor.Editor
    {
        private AudioEffectPlayer inspectedAudioEffectPlayer;
        public VisualTreeAsset audioEffectPlayerUxml;

        private VisualElement previewControls;
        private Button previewButton;
        private Button stopButton;

        private void OnEnable()
        {
            inspectedAudioEffectPlayer = (AudioEffectPlayer)target;
        }

        private void OnSceneGUI()
        {
            var position = inspectedAudioEffectPlayer.transform.position;

            Handles.color = Color.yellow;

            EditorGUI.BeginChangeCheck();
            var minDistanceValue = Handles.RadiusHandle(Quaternion.identity, position, inspectedAudioEffectPlayer.distance.x);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed minDistance");
                inspectedAudioEffectPlayer.distance.x = minDistanceValue;
                inspectedAudioEffectPlayer.distance.y = Mathf.Max(minDistanceValue, inspectedAudioEffectPlayer.distance.y);
            }

            EditorGUI.BeginChangeCheck();
            var maxDistanceValue = Handles.RadiusHandle(Quaternion.identity, position, inspectedAudioEffectPlayer.distance.y);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed maxDistance");
                inspectedAudioEffectPlayer.distance.y = maxDistanceValue;
                inspectedAudioEffectPlayer.distance.x = Mathf.Min(maxDistanceValue, inspectedAudioEffectPlayer.distance.x);
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement audioEffectPlayerInspector = new VisualElement();

            VisualElement uxmlContent = audioEffectPlayerUxml.CloneTree();
            audioEffectPlayerInspector.Add(uxmlContent);


            previewButton = audioEffectPlayerInspector.Q<Button>("Play");
            stopButton = audioEffectPlayerInspector.Q<Button>("Stop");
            previewControls = audioEffectPlayerInspector.Q<VisualElement>("Preview");

            previewButton.clickable.clicked += Preview;
            stopButton.clickable.clicked += StopPreview;
            stopButton.SetEnabled(PreviewIsPlaying);

            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
            previewControls.visible = !EditorApplication.isPlaying;

            return audioEffectPlayerInspector;
        }

        public override void OnInspectorGUI()
        {
            if (PreviewIsPlaying && !stopButton.enabledSelf) {
                stopButton.SetEnabled(true);
            }

            if (!PreviewIsPlaying && stopButton.enabledSelf) {
                stopButton.SetEnabled(false);
            }

            previewControls.visible = !Application.isPlaying;
        }

        private static AudioSource previewSource;

        private void Preview()
        {
            if (previewSource == null) {
                previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioEffectPreview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
                previewSource.enabled = true;
                previewSource.loop = false;
            }

            if (previewSource.isPlaying) {
                previewSource.Stop();
            }

            previewSource.resource = inspectedAudioEffectPlayer.effect;
            previewSource.outputAudioMixerGroup = inspectedAudioEffectPlayer.audioGroup;
            previewSource.rolloffMode = inspectedAudioEffectPlayer.rolloffMode;
            previewSource.minDistance = inspectedAudioEffectPlayer.distance.x;
            previewSource.maxDistance = inspectedAudioEffectPlayer.distance.y;
            previewSource.spatialBlend = inspectedAudioEffectPlayer.spatialType switch {
                AudioEffectPlayer.SpatialType.TwoD => 0f,
                AudioEffectPlayer.SpatialType.ThreeD => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };

            previewSource.transform.position = inspectedAudioEffectPlayer.transform.position;


            previewSource.Play();
            stopButton.SetEnabled(true);
        }

        private void StopPreview()
        {
            if (!PreviewIsPlaying) {
                return;
            }

            previewSource.Stop();
            stopButton.SetEnabled(false);
        }

        private void OnPlayModeStateChange(PlayModeStateChange change)
        {
            switch (change) {
                case PlayModeStateChange.EnteredEditMode:
                    previewControls.visible = true;
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    previewControls.visible = false;
                    StopPreview();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    previewControls.visible = false;
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    previewControls.visible = true;
                    break;
            }
        }

        private static bool PreviewIsPlaying => previewSource != null && previewSource.isPlaying;
    }
}