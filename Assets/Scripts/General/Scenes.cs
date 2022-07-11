using UnityEngine;

public static class Scenes
{
    private static int currentScene;
    
    public static void SetScene(int scene)
    {
        currentScene = scene;
    }

    public static int GetScene()
    {
        return currentScene;
    }

}
