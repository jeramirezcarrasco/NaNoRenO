using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOut : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float position, float speed, bool Left)
    {
        StartCoroutine(RuTMove(position, speed, Left));
    }

    IEnumerator RuTMove(float position, float speed,bool Left)
    {
        if (Left)
        {
            
            while (gameObject.transform.position.x < position)
            {
                transform.position += transform.right * ((Screen.width / speed) * speed) * Time.deltaTime;
                yield return new WaitForSeconds(0.0001f);
            }
        }
        else
        {
            while (gameObject.transform.position.x > position)
            {
                transform.position += transform.right * ((Screen.width / speed) * -speed) * Time.deltaTime;
                yield return new WaitForSeconds(0.0001f);
            }
        } 
    }
}
