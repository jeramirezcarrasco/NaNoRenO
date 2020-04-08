using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    GameObject Text;
    RectTransform boxBackground;
    RectTransform textBackground;

    // Start is called before the first frame update
    void Awake()
    {
        Text = gameObject.transform.GetChild(0).gameObject;
        boxBackground = GetComponent<RectTransform>();
        textBackground = Text.GetComponent<RectTransform>();
        Debug.Log(Screen.width);
        Text.GetComponent<TextMeshProUGUI>().fontSize = Screen.width * 0.009f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float MaxLength(string x)
    {
        int max = 0;
        string[] Rows = x.Split('\n');
        for (int i = 0; i < Rows.Length; i++)
        {
            if (max < Rows[i].Length)
            {
                max = Rows[i].Length;
            }
        }
        return max;
    }

    public void ChangeText(string NewText)
    {
        Text.GetComponent<TextMeshProUGUI>().text = NewText;
    }

    public void Resize()
    {
        boxBackground.sizeDelta = new Vector2((0.925f -
            (0.015f * MaxLength(Text.GetComponent<TextMeshProUGUI>().text.ToString()))) *
            MaxLength(Text.GetComponent<TextMeshProUGUI>().text.ToString()) *
            Text.GetComponent<TextMeshProUGUI>().fontSize,
           1.388f * Text.GetComponent<TextMeshProUGUI>().fontSize *
           Text.GetComponent<TextMeshProUGUI>().text.Split('\n').Length);

        textBackground.sizeDelta = new Vector2((0.925f -
            (0.015f * MaxLength(Text.GetComponent<TextMeshProUGUI>().text.ToString()))) *
            MaxLength(Text.GetComponent<TextMeshProUGUI>().text.ToString()) *
            Text.GetComponent<TextMeshProUGUI>().fontSize,
           1.388f * Text.GetComponent<TextMeshProUGUI>().fontSize *
           Text.GetComponent<TextMeshProUGUI>().text.Split('\n').Length);
    }
}
