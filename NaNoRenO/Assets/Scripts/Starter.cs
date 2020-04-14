using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Starter : MonoBehaviour
{
    [SerializeField] float startTime;
    [SerializeField] public UnityEvent FunctionsToCall;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(startTime);
        FunctionsToCall.Invoke();
    }
}
