using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManagerManager : MonoBehaviour
{
    [SerializeField] bool Debugger;
    [SerializeField] GameObject DialogObjectLeft;
    [SerializeField] GameObject DialogObjectRight;
    [SerializeField] GameObject ChatBox;
    [SerializeField] GameObject Thing1;
    [SerializeField] GameObject Thing2;
    [SerializeField] ChatScript[] Script;
    [SerializeField] GameObject Button;
    List<GameObject> CurrChat;
    int index;
    bool begun;
    bool Bussy;

    private void Start()
    {
        CurrChat = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Debugger && Input.GetKeyDown("1"))
        {
            NewChat("1234567890", false);
        }

        if (Debugger && Input.GetKeyDown("2"))
        {
            NewChat("1234567890 " +
                '\n' + "asdas", true);
        }
        if (Debugger && Input.GetKeyDown("f"))
        {
            StartChat();
        }
        if (begun && !Bussy && index < Script.Length && !Script[index].UserInput)
        {
            Button.SetActive(false);
            Bussy = true;
            StartCoroutine(NextChat(Script[index].WaitTime));
        }
        if(begun && index < Script.Length && Script[index].UserInput && !Bussy)
        {
            Button.SetActive(true);
        }

    }

    public void StartChat()
    {
        begun = true;
        NewChat(Script[index].TextDialog, Script[index].Left);
    }

    public void NewChat(string dialog, bool Left)
    {
        if(Left)
        {
            CurrChat.Add(Instantiate(DialogObjectLeft, Thing2.transform.position, Quaternion.identity, ChatBox.transform));
            CurrChat[CurrChat.Count - 1].GetComponent<ChatManager>().ChangeText(dialog);
            CurrChat[CurrChat.Count - 1].GetComponent<ChatManager>().Resize();
            CurrChat[CurrChat.Count - 1].transform.position = new Vector2(Thing2.transform.position.x + CurrChat[CurrChat.Count - 1].GetComponent<RectTransform>().rect.width / 2, Thing1.transform.position.y);
            MoveUp();

        }
        else
        {
            CurrChat.Add(Instantiate(DialogObjectRight, Thing1.transform.position, Quaternion.identity, ChatBox.transform));
            CurrChat[CurrChat.Count - 1].GetComponent<ChatManager>().ChangeText(dialog);
            CurrChat[CurrChat.Count - 1].GetComponent<ChatManager>().Resize();
            CurrChat[CurrChat.Count - 1].transform.position = new Vector2(Thing1.transform.position.x - CurrChat[CurrChat.Count - 1].GetComponent<RectTransform>().rect.width / 2, Thing1.transform.position.y);
            MoveUp();
        }
    }

    public void MoveUp()
    {
        
        for (int i = 0; i < CurrChat.Count - 1; i++)
        {
            
            CurrChat[i].transform.position = new Vector2(CurrChat[i].transform.position.x, CurrChat[i].transform.position.y + CurrChat[CurrChat.Count - 1].GetComponent<RectTransform>().rect.height + CurrChat[CurrChat.Count - 2].GetComponent<RectTransform>().rect.height);

        }
    }

    public void UserInput()
    {
        index++;
        NewChat(Script[index].TextDialog, Script[index].Left);
    }

    IEnumerator NextChat(float time)
    {
        yield return new WaitForSeconds(time);
        index++;
        if(index < Script.Length)
        {
            NewChat(Script[index].TextDialog, Script[index].Left);
        }
        
        Bussy = false;
    }



}

[System.Serializable]
public class ChatScript
{
    [TextArea(10, 14)] public string TextDialog;
    public bool Left;
    public float WaitTime;
    public bool UserInput;
}
