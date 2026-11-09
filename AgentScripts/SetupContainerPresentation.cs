using System.Text;
using InventoryCode.UI;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SetupContainerPresentation
{
	public static string Run()
	{
		const string prefabPath = "Assets/Prefabs/Container.prefab";
		const string scenePath = "Assets/Scenes/SampleScene.unity";

		GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);
		try
		{
			GridLayoutGroup grid = root.GetComponent<GridLayoutGroup>();
			ContainerView view = root.GetComponent<ContainerView>();
			if (view == null)
				view = root.AddComponent<ContainerView>();

			ItemStackView[] slots = root.GetComponentsInChildren<ItemStackView>(true);
			SerializedObject serializedView = new SerializedObject(view);
			serializedView.FindProperty("gridLayoutGroup").objectReferenceValue = grid;

			SerializedProperty slotList = serializedView.FindProperty("itemStackViews");
			slotList.ClearArray();
			for (int i = 0; i < slots.Length; i++)
			{
				slotList.InsertArrayElementAtIndex(i);
				slotList.GetArrayElementAtIndex(i).objectReferenceValue = slots[i];
			}

			serializedView.ApplyModifiedPropertiesWithoutUndo();
			PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
		}
		finally
		{
			PrefabUtility.UnloadPrefabContents(root);
		}

		AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
		string guid = AssetDatabase.AssetPathToGUID(prefabPath);
		AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
		entry.address = "Container";
		settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true, true);
		AssetDatabase.SaveAssets();

		Scene scene = EditorSceneManager.OpenScene(scenePath);
		Bootstrap bootstrap = Object.FindFirstObjectByType<Bootstrap>();
		Transform containersList = GameObject.Find("Layers").transform.Find("HUD/ContainersList");

		SerializedObject serializedBootstrap = new SerializedObject(bootstrap);
		serializedBootstrap.FindProperty("_containersList").objectReferenceValue = containersList;
		serializedBootstrap.ApplyModifiedPropertiesWithoutUndo();

		int hiddenSlots = 0;
		for (int i = 0; i < containersList.childCount; i++)
		{
			Transform child = containersList.GetChild(i);
			if (child.GetComponent<ItemStackView>() == null)
				continue;

			child.gameObject.SetActive(false);
			hiddenSlots++;
		}

		EditorSceneManager.MarkSceneDirty(scene);
		EditorSceneManager.SaveScene(scene);

		StringBuilder report = new StringBuilder();
		report.Append("prefab=Container");
		report.Append(" address=Container");
		report.Append(" parent=").Append(containersList.name);
		report.Append(" hiddenOldSlots=").Append(hiddenSlots);
		return report.ToString();
	}
}
