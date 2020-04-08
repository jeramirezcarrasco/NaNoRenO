using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAutoSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<TextMeshProUGUI>().fontSize = Screen.width * 0.009f;

    }
}
