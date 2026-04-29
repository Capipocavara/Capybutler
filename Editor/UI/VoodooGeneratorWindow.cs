using Capibutler.Editor.CodeGenerator;
using Capibutler.Events.Base;
using Capibutler.Utils;
using Capibutler.Values.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capibutler.Editor.UI
{
    public class VoodooGeneratorWindow : EditorWindow
    {
        private const string TabClassname = "tab";
        private const string TabSuffix = "Tab";
        private const string ContentSuffix = "Content";
        private const string SelectedTab = "selectedTab";
        private const string UnselectedTab = "unselectedTab";
        private const string UnselectedContent = "unselectedContent";

        private readonly VoodooEventTab eventTab = new();
        private readonly VoodooValueTab valueTab = new();
        private readonly VoodooSettingsTab settingsTab = new();

        private void OnEnable()
        {
            GenericReference.InlineEditorStateChanged += settingsTab.OnReferenceInlineEditorStateChanged;
            BaseEventListener.InlineEditorStateChanged += settingsTab.OnEventListenerInlineEditorStateChanged;
        }

        private void OnDisable()
        {
            GenericReference.InlineEditorStateChanged -= settingsTab.OnReferenceInlineEditorStateChanged;
            BaseEventListener.InlineEditorStateChanged -= settingsTab.OnEventListenerInlineEditorStateChanged;
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Voodoo.VoodooPackagePath("de.capipocavara.voodoovara", "Editor/UI/Voodoo.uss")));
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Voodoo.VoodooPackagePath("de.capipocavara.voodoovara", "Editor/UI/VoodooGenerator.uxml"));
            root.Add(visualTree.Instantiate());

            eventTab.CreateGUI(root.Q<VisualElement>("eventTypeContent"));
            valueTab.CreateGUI(root.Q<VisualElement>("valueTypeContent"));
            settingsTab.CreateGUI(root.Q<VisualElement>("settingsContent"));

            root.Query<Label>(className: TabClassname).ForEach(tab => tab.RegisterCallback<ClickEvent>(OnTabClicked));
            root.Query<Button>("create").ForEach(tab => tab.clicked += Close);
        }

        [MenuItem("Capipocavara/Create/Custom VoodooVara Event", false, priority = 100)]
        private static void OpenEventWindow()
        {
            CreateWindow("eventTypeTab");
        }

        [MenuItem("Capipocavara/Create/Custom VoodooVara Value", false, priority = 101)]
        private static void OpenValueWindow()
        {
            CreateWindow("valueTypeTab");
        }

        [MenuItem("Capipocavara/Settings", false, priority = 102)]
        private static void OpenSettingsWindow()
        {
            CreateWindow("settingsTab");
        }

        private static void CreateWindow(string activeAtStart)
        {
            var window = GetWindow<VoodooGeneratorWindow>(false, "Custom Voodoo Events & Values");
            window.minSize = new Vector2(470f, 200f);
            window.maxSize = new Vector2(800f, 200f);
            window.SetStartTab(activeAtStart);
            window.Show();

            CodeGeneratorUtils.InitTypeCache();
        }

        private void SetStartTab(string tabName)
        {
            UpdateTabBar(rootVisualElement.Q<Label>(tabName));
        }

        private void OnTabClicked(ClickEvent click)
        {
            UpdateTabBar(click.target as Label);
        }

        private void UpdateTabBar(Label tabLabel)
        {
            if (tabLabel?.ClassListContains(SelectedTab) ?? true)
                return;

            rootVisualElement
                .Query<Label>(className: TabClassname)
                .Where(tab => tab != tabLabel && tab.ClassListContains(SelectedTab))
                .ForEach(UnselectTab);
            SelectTab(tabLabel);
        }

        private void UnselectTab(VisualElement tab)
        {
            tab.RemoveFromClassList(SelectedTab);
            tab.AddToClassList(UnselectedTab);
            var tabContent = rootVisualElement.Q(tab.name.Replace(TabSuffix, ContentSuffix));
            tabContent.AddToClassList(UnselectedContent);
        }

        private void SelectTab(VisualElement tab)
        {
            tab.RemoveFromClassList(UnselectedTab);
            tab.AddToClassList(SelectedTab);
            var tabContent = rootVisualElement.Q(tab.name.Replace(TabSuffix, ContentSuffix));
            tabContent.RemoveFromClassList(UnselectedContent);
        }
    }
}