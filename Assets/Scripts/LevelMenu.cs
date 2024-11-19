using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{

    public void changeLevel(int levelIndex)
    {
        SceneManager.LoadSceneAsync(2);
    }
}
