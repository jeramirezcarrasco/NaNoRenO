using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI SpeechBox;
    [SerializeField] public TextMeshProUGUI SpeechNameBox;
    bool nextSentenceActive = false;
    [NonSerialized] public bool writing = false;
    [SerializeField] bool Debugger;
    [SerializeField] VNScript[] DialogScript;
    [SerializeField] CharacterPopOut characterPopOut;
    [SerializeField] Sprite[] backGrounds;
    [SerializeField] Image BackGround;
    float ReadinSpeed = 0.01f;
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
        StopAllCoroutines();
        nextSentenceActive = true;
        StartCoroutine(TypeSentence(DialogScript[ScriptIndex].TextDialog, DialogScript[ScriptIndex].fmodEvent));
    }

    public void DisplayNextSentence()
    {
        if (ScriptIndex > DialogScript.Length)
        {
            EndDialod();
            return;
        }
        else
        {
            ScriptIndex++;
            StopAllCoroutines();
            ScriptActions();
            StartCoroutine(TypeSentence(DialogScript[ScriptIndex].TextDialog, DialogScript[ScriptIndex].fmodEvent));
        }
    }

    IEnumerator TypeSentence(string Sentence, string fmodEvent)
    {
        writing = true;
        SpeechNameBox.text = DialogScript[ScriptIndex].Name;
        yield return new WaitForSeconds(0.1f);
        SpeechBox.text = "";
        if (fmodEvent != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(fmodEvent);
        }
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

    public void ScriptActions()
    {
        SpeechNameBox.text = DialogScript[ScriptIndex].Name;

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
        }
        if(DialogScript[ScriptIndex].ChangeBackGround)
        {
            StartCoroutine(ChangeBackGround(DialogScript[ScriptIndex].BackGroundToChange));
        }
    }

    IEnumerator ChangeBackGround(int nextBackGround)
    {
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            BackGround.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(0.01f);
        }
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
}
