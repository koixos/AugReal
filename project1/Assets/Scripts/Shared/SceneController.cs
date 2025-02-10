using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        ChangeRotation(sceneName);
    }

    private void ChangeRotation(string sceneName)
    {
        if (sceneName.Equals("Dragon") || sceneName.Equals("Furnitures"))
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        } else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
