using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI SpeechBox;
    [SerializeField] public TextMeshProUGUI SpeechNameBox;
    bool nextSentenceActive = false;
    [NonSerialized] public bool writing = false;
    [SerializeField] bool Debugger;
    [SerializeField] CharacterPopOut characterPopOut;
    [SerializeField] Sprite[] backGrounds;
    [SerializeField] Image BackGround;
    [SerializeField] GameObject Panel_Speech;
    [SerializeField] GameObject NameSpeechBox;
    [SerializeField] GameObject SpeechBoxSpeech;
    [SerializeField] VNScript[] DialogScript;

    float ReadinSpeed = 0.02f;
    float CurrReadingSpeed = 0.01f;
    int ScriptIndex;

    private void Start()
    {
        ScriptIndex = 0;
    }

    private void Update()
    {
        if (Debugger)
        {
            if (Input.GetKeyDown("f"))
            {
                StartDialog();
            }
        }
        if ((Input.GetKey("e") || Input.GetMouseButton(0))&& nextSentenceActive && writing)
        {
            CurrReadingSpeed = ReadinSpeed / 2;
        }
        else
        {
            CurrReadingSpeed = ReadinSpeed;
        }
        if ((Input.GetKeyDown("e") || Input.GetMouseButtonDown(0)) && nextSentenceActive && !writing)
        {
            DisplayNextSentence();
        }
        if (Debugger && Input.GetKeyDown("x"))
        {
            StartDialog();
        }
        if (Debugger && Input.GetKeyDown("l"))
        {
            StartCoroutine(ChangeBackGround(1));
        }
    }

    public void StartDialog()
    {
        Panel_Speech.SetActive(true);
        StopAllCoroutines();
        nextSentenceActive = true;
        if (DialogScript[ScriptIndex].Name == "")
        {
            NameSpeechBox.SetActive(false);
        }
        else
        {
            SpeechNameBox.text = DialogScript[ScriptIndex].Name;
        }
        ScriptActions();
        StartCoroutine(TypeSentence(DialogScript[ScriptIndex].TextDialog));
    }

    public void DisplayNextSentence()
    {
        if (ScriptIndex + 1 >= DialogScript.Length)
        {
            EndDialod();
            return;
        }
        else
        {
            ScriptIndex++;
            StopAllCoroutines();
            ScriptActions();
            StartCoroutine(TypeSentence(DialogScript[ScriptIndex].TextDialog));
        }
    }

    IEnumerator TypeSentence(string Sentence)
    {
        if(Sentence == "")
        {
            SpeechBox.text = "";
            SpeechBoxSpeech.SetActive(false);
            yield return new WaitForSeconds(1);
            DisplayNextSentence();
        }
        else
        {
            SpeechBoxSpeech.SetActive(true);
            writing = true;
            yield return new WaitForSeconds(0.1f);
            SpeechBox.text = "";
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance("event:/FX/SpeechMid");
            instance.start();
            foreach (char letter in Sentence.ToCharArray())
            {
                if (writing == false)
                {
                    SpeechBox.text = Sentence;
                    break;
                }
                SpeechBox.text += letter;
                yield return new WaitForSeconds(ReadinSpeed);
            }
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            writing = false;
        }
        
    }

    public void ScriptActions()
    {
        if(DialogScript[ScriptIndex].Name == "")
        {
            NameSpeechBox.SetActive(false);
        }
        else
        {
            NameSpeechBox.SetActive(true);
            SpeechNameBox.text = DialogScript[ScriptIndex].Name;
        }
        

        if (DialogScript[ScriptIndex].CharacterFadeIn)
        {
            characterPopOut.CharacterFadeIn(
                DialogScript[ScriptIndex].CharacterToFaceIn,
                DialogScript[ScriptIndex].FadeInLocation,
                DialogScript[ScriptIndex].FadeInSpeed);
        }
        if (DialogScript[ScriptIndex].CharacterSlideIn)
        {
            characterPopOut.SlideIn(
                DialogScript[ScriptIndex].CharacterToSlideIn,
                DialogScript[ScriptIndex].SlideInLocation,
                DialogScript[ScriptIndex].SlideInSpeed,
                DialogScript[ScriptIndex].SlideInLeft);
        }
        if (DialogScript[ScriptIndex].CharacterFadeOut)
        {
            characterPopOut.CharacterFadeOut(
                DialogScript[ScriptIndex].FadeOutPosition,
                DialogScript[ScriptIndex].FadeOutSpeed);
        }
        if (DialogScript[ScriptIndex].ScreenShake)
        {
            characterPopOut.ScreenShake(
                DialogScript[ScriptIndex].ShakeTime,
                DialogScript[ScriptIndex].ShakeRange);
            FMODUnity.RuntimeManager.PlayOneShot("event:/FX/ScreenShake");
        }
        if(DialogScript[ScriptIndex].ChangeBackGround)
        {
            StartCoroutine(ChangeBackGround(DialogScript[ScriptIndex].BackGroundToChange));
        }
        if (DialogScript[ScriptIndex].fmodEvent != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(DialogScript[ScriptIndex].fmodEvent);
        }
        if(DialogScript[ScriptIndex].CallFunction)
        {
            Debug.Log("boop");
            DialogScript[ScriptIndex].FunctionsToCall.Invoke();
        }
    }

    IEnumerator ChangeBackGround(int nextBackGround)
    {
        Debug.Log(nextBackGround);
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            BackGround.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log(backGrounds[nextBackGround]);
        BackGround.sprite = backGrounds[nextBackGround];
        for (float f = 0f; f <= 1; f += 0.05f)
        {
            BackGround.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(0.01f);
        }



    }

    private void EndDialod()
    {
        SpeechBox.text = "";
        SpeechNameBox.text = "";
        nextSentenceActive = false;
        StopAllCoroutines();
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1);
    }
}

[System.Serializable]
public class VNScript
{
    public String Name;
    [TextArea(10,20)] public String TextDialog;

    [FMODUnity.EventRef]
    public string fmodEvent;

    public bool CharacterFadeIn;
    public int CharacterToFaceIn;
    public int FadeInLocation;
    [SerializeField] public float FadeInSpeed = 0.1f;

    public bool CharacterSlideIn;
    public int CharacterToSlideIn;
    public int SlideInLocation;
    public float SlideInSpeed = 50f;
    public bool SlideInLeft;

    public bool CharacterFadeOut;
    public int FadeOutPosition;
    public float FadeOutSpeed = 0.1f;

    public bool ScreenShake;
    public float ShakeTime = 1f;
    public float ShakeRange = 3f;

    public bool ChangeBackGround;
    public int BackGroundToChange;

    public bool CallFunction;
    public UnityEvent FunctionsToCall;
}
