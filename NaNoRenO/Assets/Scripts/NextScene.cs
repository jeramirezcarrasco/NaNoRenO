using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public Animator transition;
    public float TransitionTIme;
    public bool Debugger;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("m"))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTIme);
        SceneManager.LoadScene(index);
    }
}
