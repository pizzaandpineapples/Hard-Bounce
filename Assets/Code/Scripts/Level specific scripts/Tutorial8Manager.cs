using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial8Manager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NewGame());
        PlayerPrefs.SetString("isTutorialComplete", "true");
        PlayerPrefs.Save();
    }

    IEnumerator NewGame()
    {
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
