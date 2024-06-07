using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CustomToday : MonoBehaviour
{
    public TMP_Text InitialValue, FinalValue;
    public TMP_InputField Deviation;
    public PlanList Plan;


    private void OnEnable()
    {
        Deviation.onEndEdit.AddListener(OnDeviationChange);
       
    }

    private void OnDisable()
    {
        Deviation.onEndEdit.RemoveListener(OnDeviationChange);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDeviationChange(string value)
    {
        if (value.Contains("-"))
        {
            string changedvalue = value.Replace("-", "");

            float deviationFloat = 0;
            float.TryParse(changedvalue, out deviationFloat);
            if (deviationFloat != 0)
            {
                Debug.Log("devi value " + deviationFloat);
                float InitialFloat = float.Parse(InitialValue.text);
                float FinalFloat = InitialFloat - deviationFloat;
                FinalValue.text = FinalFloat.ToString("F1");
            }
            else
            {
                FinalValue.text = InitialValue.text;
            }

        }
        else if (value.Contains("+"))
        {
            string changedvalue = value.Replace("+", "");

            float deviationFloat = 0;
            float.TryParse(value, out deviationFloat);
            if (deviationFloat != 0)
            {
                float InitialFloat = float.Parse(InitialValue.text);
                float FinalFloat = InitialFloat + deviationFloat;
                FinalValue.text = FinalFloat.ToString("F1");
            }
            else
            {
                FinalValue.text = InitialValue.text;
            }
        }
        else if (value == "")
        {
            Deviation.text = "0";
            FinalValue.text = InitialValue.text;
        }
        else
        {
            FinalValue.text = InitialValue.text;
        }

        int SrNo = (int.Parse(Plan.SrNo.text) -1);
        Debug.Log("Sr no" + SrNo);
        EventManager.Instance.OnInputValueCahnge(SrNo);

        DailyPlanManager.instance.UpdateMaterialSum();

        
    }


}
