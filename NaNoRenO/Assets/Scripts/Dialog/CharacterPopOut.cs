using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterPopOut : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] int grid;
    [SerializeField] float FadeSpeed;
    [SerializeField] GameObject MinX;
    [SerializeField] GameObject MaxX;
    [SerializeField] GameObject YLocation;
    [SerializeField] GameObject CharParent;
    [SerializeField] bool Debugger;
    [SerializeField] GameObject gameCanvas1;
    [SerializeField] GameObject gameCanvas2;
    [SerializeField] GameObject gameCanvas3;
    bool[] Positions;
    float[] GridPositions;
    GameObject[] CharGrid;
    bool busy;
    // Start is called before the first frame update
    void Start()
    {
        CharGrid = new GameObject[grid];
        Positions = new bool[grid];
        GridPositions = new float[grid];
        float offset =  (MaxX.transform.position.x - MinX.transform.position.x) / grid;
        GridPositions[0] = MinX.transform.position.x;
        for(int i = 1; i < grid; i++)
        {
            GridPositions[i] = GridPositions[i - 1] + offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Debugger && Input.GetKeyDown("1"))
        {
            //GameObject NewChar = Instantiate(characters[0], new Vector2(GridPositions[2], YLocation.transform.position.y ), Quaternion.identity, CharParent.transform);
            //CharGrid[0] = NewChar;
            //StartCoroutine(FadeIN(CharGrid[0], FadeSpeed));
            CharacterFadeIn(0, 3, 0.1f);
        }
        if (Debugger && Input.GetKeyDown("q"))
        {
            CharacterFadeOut(3, FadeSpeed);
        }
        if (Debugger && Input.GetKeyDown("y"))
        {
            SlideIn(0, 3, 50f, false);
        }
        if (Debugger && Input.GetKeyDown("w"))
        {
            ScreenShake(1, 3);
        }
    }

    public void ScreenShake(float shakeTime, float shakeRange)
    {
        StartCoroutine(RuTScreenShake(shakeTime, shakeRange));
    }

    IEnumerator RuTScreenShake(float shakeTime, float shakeRange)
    {
        float elapsed = 0.0f;
        Quaternion originalRotation1 = gameCanvas1.transform.rotation;
        Quaternion originalRotation2 = gameCanvas2.transform.rotation;
        Quaternion originalRotation3 = gameCanvas3.transform.rotation;

        while (elapsed < shakeTime)
        {

            elapsed += Time.deltaTime;
            float z = Random.value * shakeRange - (shakeRange / 2);
            gameCanvas1.transform.eulerAngles = new Vector3(originalRotation1.x, originalRotation1.y, originalRotation1.z + z);
            gameCanvas2.transform.eulerAngles = new Vector3(originalRotation2.x, originalRotation2.y, originalRotation2.z + z);
            gameCanvas3.transform.eulerAngles = new Vector3(originalRotation3.x, originalRotation3.y, originalRotation3.z + z);
            yield return null;
        }
        gameCanvas1.transform.rotation = originalRotation1;
        gameCanvas2.transform.rotation = originalRotation2;
        gameCanvas3.transform.rotation = originalRotation3;

    }

    public void SlideIn(int Character, int position, float speed, bool Left)
    {
        StartCoroutine(RuTSlideIn(Character, position, speed, Left));
    }

    IEnumerator RuTSlideIn(int Character, int position, float speed, bool Left)
    {
        if (CharGrid[position] != null)
        {
            StartCoroutine(FadeOUT(CharGrid[position], FadeSpeed - 0.05f));
            yield return new WaitForSeconds((FadeSpeed - 0.05f) * 19);
        }
        GameObject NewChar;
        if (Left)
        {
             NewChar = Instantiate(characters[Character], new Vector2((GridPositions[0] - (Screen.width * 0.15f)), YLocation.transform.position.y), Quaternion.identity, CharParent.transform);
        }
        else
        {
             NewChar = Instantiate(characters[Character], new Vector2((GridPositions[GridPositions.Length -1] + (Screen.width * 0.3f)), YLocation.transform.position.y), Quaternion.identity, CharParent.transform);
        }
        CharGrid[position] = NewChar;
        yield return new WaitForSeconds(1);
        CharGrid[position].GetComponent<MoveOut>().Move(GridPositions[position],speed,Left);

    }

    public void CharacterFadeIn(int Character, int position, float speed)
    {
        StartCoroutine(RuTCharacterFadeIn(Character, position, speed));
    }

    IEnumerator RuTCharacterFadeIn(int Character, int position,float speed)
    {
        if(CharGrid[position] != null)
        {
            StartCoroutine(FadeOUT(CharGrid[position], speed - 0.05f));
            yield return new WaitForSeconds((speed - 0.05f) * 19);
        }
        GameObject NewChar = Instantiate(characters[Character], new Vector2(GridPositions[position], YLocation.transform.position.y), Quaternion.identity, CharParent.transform);
        CharGrid[position] = NewChar;
        StartCoroutine(FadeIN(CharGrid[position], speed));
    }
    public void CharacterFadeIn(int Character, int position)
    {
        StartCoroutine(RuTCharacterFadeIn(Character, position));
    }
    IEnumerator RuTCharacterFadeIn(int Character, int position)
    {
        if (CharGrid[position] != null)
        {
            StartCoroutine(FadeOUT(CharGrid[position],FadeSpeed - 0.05f));
            yield return new WaitForSeconds((FadeSpeed - 0.05f) * 19);
        }
        GameObject NewChar = Instantiate(characters[Character], new Vector2(GridPositions[position], YLocation.transform.position.y), Quaternion.identity, CharParent.transform);
        CharGrid[position] = NewChar;
        StartCoroutine(FadeIN(CharGrid[position], FadeSpeed));
    }

    public void CharacterFadeOut(int position, float speed)
    {
        StartCoroutine(FadeOUT(CharGrid[position], speed));
    }
    public void CharacterFadeOut(int position)
    {
        StartCoroutine(FadeOUT(CharGrid[position], FadeSpeed));
    }

    IEnumerator FadeOUT(GameObject Character, float fadeSpeed)
    {
        Image rend = Character.GetComponent<Image>();
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            rend.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Destroy(Character);
        CharGrid[0] = null;
    }

    IEnumerator FadeIN(GameObject Character, float fadeSpeed)
    {
        Image rend = Character.GetComponent<Image>();
        rend.color = new Color(1, 1, 1, 0);
        for (float f = 0f; f <= 1.05; f += 0.05f)
        {
            rend.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
