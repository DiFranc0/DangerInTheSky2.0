using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBehaviour : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
