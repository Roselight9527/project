using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,Iinteractable
{
    public void ExitAction()
    {
        
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest");
    }
}
