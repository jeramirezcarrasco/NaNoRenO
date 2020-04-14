using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAutoSize : MonoBehaviour
{
    [SerializeField] bool Scale;
    // Start is called before the first frame update
    void Awake()
    {
        if(Scale)
        {
            Debug.Log((gameObject.GetComponent<TextMeshProUGUI>().fontSize / Screen.width) + gameObject.GetComponent<TextMeshProUGUI>().fontSize);
            gameObject.GetComponent<TextMeshProUGUI>().fontSize = (gameObject.GetComponent<TextMeshProUGUI>().fontSize / Screen.width) + gameObject.GetComponent<TextMeshProUGUI>().fontSize;
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontSize = Screen.width * 0.009f;
        }
        
        

    }
}
