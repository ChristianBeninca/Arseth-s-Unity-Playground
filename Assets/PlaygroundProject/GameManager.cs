using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

struct PortalInfo
{
    public Scene Scene;

    public PortalInfo(Scene Scene) 
    {
        this.Scene = Scene;
    }
}

public static class PortalManager
{
    static Dictionary<string, PortalInfo> PortalList;

    public static void AddPortalToList(string key, Scene scene)
    {
        PortalList.Add(key, new PortalInfo(scene));
    }

    public static Scene FindDimension(string portalKey)
    {
        return PortalList[portalKey].Scene;
    }

    public static void Setup()
    {
        AddPortalToList("StartRoom", SceneManager.GetSceneByName("MainScene"));
        AddPortalToList("Load1", SceneManager.GetSceneByName("CorridorScene"));
        AddPortalToList("Load2", SceneManager.GetSceneByName("CorridorScene"));
    }
}

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        PortalManager.Setup();
    }
}