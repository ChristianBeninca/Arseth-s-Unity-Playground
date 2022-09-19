using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(PortalLinks))]
public class PortalLinksEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Use this to automatically populate portals o  dictionary");
        DrawDefaultInspector();

        PortalLinks portalLinks = (PortalLinks)target;
        if (GUILayout.Button("Populate All Portals To Lists"))
        {
            portalLinks.PopulateAllPortalsToLists();
        }
    }
}

public class PortalLinks : MonoBehaviour
{
    public static PortalLinks Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    [ReadOnly] [SerializeField] List<string> inputKeysList;
    [ReadOnly] List<Portal> portalsList;
    [ReadOnly] [SerializeField] List<int> scenesList;

    [ExecuteInEditMode] void ClearAllLists()
    {
        inputKeysList.Clear();
        portalsList.Clear();
        scenesList.Clear();
    }

    [ExecuteInEditMode]
    public void PopulateAllPortalsToLists()
    {
        //ClearAllLists();

        inputKeysList.Clear();
        portalsList.Clear();
        scenesList.Clear();

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        Scene mainScene = SceneManager.GetSceneByName("MainScene");

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i), OpenSceneMode.Additive);
        }

        GameObject[] portalsArray = GameObject.FindGameObjectsWithTag("Portal");

        foreach (GameObject portalObj in portalsArray)
        {
            Portal portal = portalObj.GetComponent<Portal>();
            inputKeysList.Add(portal.InputKey);
            portalsList.Add(portal);
            scenesList.Add(portal.gameObject.scene.buildIndex);
        }

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(i);

            if (scene != mainScene)
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }

    public int GetSceneByKey(string key)
    {
        int i = 0;
        while (i <= inputKeysList.Count)
        {
          
            if (inputKeysList[i] == key)
                return scenesList[i];

            i++;
        }
        Debug.LogError("Scene Not Found");
        return -1;
    }
    
    public Portal GetPortalByKey(string key)
    {
        int i = 0;
        while (i <= inputKeysList.Count)
        {
          
            if (inputKeysList[i] == key)
                return portalsList[i];

            i++;
        }
        Debug.LogError("Scene Not Found");
        return null;
    }
}