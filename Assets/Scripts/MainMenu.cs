using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public MainMenuType<int> mainMenuType;
    public MainMenuType<Vector2> mainMenuType2;
    public MainMenuType<Type> mainMenuType3;
    public void StartGerrymanderingButton()
    {
        mainMenuType3.type.hello = 2;
        loadScene("Game");
    }

    private void loadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

[System.Serializable]
public class MainMenuType<T>
{
    public T type;
    public MainMenuType()
    {

    }
}

[System.Serializable]
public class Type
{
    public int hello;
    public string hippo;
}
