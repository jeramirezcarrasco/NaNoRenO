using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    Vector2 CurrFreeze;

    // Start is called before the first frame update
    void Start()
    {
        CurrFreeze = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = CurrFreeze;
    }
}
