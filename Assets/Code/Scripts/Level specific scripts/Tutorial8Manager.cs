using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial8Manager : MonoBehaviour
{
    [SerializeField] private float timeTillNextScene = 5f;
    [SerializeField] private string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NewGame());
        PlayerPrefs.SetString("isTutorialComplete", "true");
        PlayerPrefs.Save();
    }

    IEnumerator NewGame()
    {
        yield return new WaitForSecondsRealtime(timeTillNextScene);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
}
