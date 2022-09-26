using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial8Handler : MonoBehaviour
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
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
