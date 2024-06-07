using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance = null;

    public delegate void InputValueChange(int SrNo);
    public static event InputValueChange inputValueChange;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void OnInputValueCahnge(int SrNo)
    {
        inputValueChange?.Invoke(SrNo);
    }


}
