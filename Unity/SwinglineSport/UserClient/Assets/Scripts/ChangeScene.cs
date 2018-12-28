using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // scene to change to
    public string scene;

    public void Changescene()
    {
        SceneManager.LoadScene(scene);
    }
}
