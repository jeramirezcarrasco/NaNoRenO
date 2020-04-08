using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideChat : MonoBehaviour
{
    [SerializeField] GameObject TargetPoint;
    [SerializeField] GameObject HidePoint;
    [SerializeField] GameObject Chat;
    bool IsHiden;

    public void HideOrShow()
    {
        if(IsHiden)
        {
            Chat.transform.position = new Vector2(Chat.transform.position.x, TargetPoint.transform.position.y);
            IsHiden = false;
        }
        else
        {
            Chat.transform.position = new Vector2(Chat.transform.position.x, HidePoint.transform.position.y);
            IsHiden = true;
        }
    }
}
