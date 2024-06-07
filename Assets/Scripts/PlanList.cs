using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PlanList : MonoBehaviour
{

    public TMP_Text SrNo, ItemName, TappingTemp, PourtempFirst, PouringtempLast, Gradcast, CRC, C, Si, Mn, Cu, FM, Chr, Ci, Fe;
    public List<TMP_Text> _TMaterials;
    public RawImage _image;
    public string Imagepath;

    [Space]
    [Header("For Todays Planonly")]
    public TMP_InputField InputTappingTemp;
    public TMP_InputField InputPourtempFirst, InputPouringtempLast, InputGradcast, InputCRC, InputC, InputSi, InputMn, InputCu, InputFM, InputChr, InputCi, InputFe;
    public List<TMP_InputField> _IMat = new List<TMP_InputField> ();

    [Space]
    [Header("Final Values")]
    public TMP_Text FCRC;
    public TMP_Text FC, FSi, FMn, FCu, FFM, FChr, FCi, FFe;
    public List<TMP_Text> _FMat = new List<TMP_Text>();

    [Space]
    [Header("Material Name List")]
    public List<TMP_Text> _MatNameList = new List<TMP_Text> ();



    private void OnEnable()
    {
        EventManager.inputValueChange += UpdateTodaysPlanOnInput;
    }

    private void OnDisable()
    {
        EventManager.inputValueChange -= UpdateTodaysPlanOnInput;
    }



    public void OnAddDatatoToday()
    {
        PlanData _thisdata = new PlanData();
        _thisdata.SrNo = (DailyPlanManager.instance._TodaysPlanData.Count + 1).ToString();
        _thisdata.ItemName = ItemName.text;
        _thisdata.TappingTemp = _thisdata.TappingTempInput = TappingTemp.text;
        _thisdata.PourTempFirst = _thisdata.PourTempFirstinput = PourtempFirst.text;
        _thisdata.PourTempLast = _thisdata.PourTempLastInput = PouringtempLast.text;
        _thisdata.GradOf = _thisdata.GradOfInput = Gradcast.text;
        _thisdata.CRCV = _thisdata.FCRC = CRC.text;
        _thisdata.CV = _thisdata.FC = C.text;
        _thisdata.SiV = _thisdata.FSi = Si.text;
        _thisdata.MnV = _thisdata.FMn = Mn.text;
        _thisdata.CuV = _thisdata.FCu = Cu.text;
        _thisdata.FMV = _thisdata.FFM = FM.text;
        _thisdata.ChrV = _thisdata.FChr = Chr.text;
        _thisdata.CiV = _thisdata.FCi = Ci.text;
        _thisdata.FeV = _thisdata.FFe = Fe.text;
        _thisdata.image = Imagepath;

        _thisdata.CRCVInput = _thisdata.CVInput = _thisdata.SiVInput = _thisdata.MnVInput = _thisdata.CuVInput = _thisdata.FMVInput = _thisdata.ChrVInput = _thisdata.CiVInput = _thisdata.FeVInput = "0";

        for (int i = 0; i < _TMaterials.Count; i++)
            _thisdata._Materials.Add(_TMaterials[i].text);

        _thisdata._InputMaterials = new List<string>(_thisdata._Materials.Count);

        for (int i = 0; i < _TMaterials.Count; i++)
            _thisdata._InputMaterials.Add("0");

        for (int i = 0; i < _TMaterials.Count; i++)
            _thisdata._FinalMaterials.Add(_TMaterials[i].text);


        for (int i = 0; i < _MatNameList.Count; i++)
            _thisdata._MaterialName.Add(_MatNameList[i].text);


        DailyPlanManager.instance._TodaysPlanData.Add(_thisdata);

        UIManager.Instance.ShowNotificationTwo("Item added successfully !!");
    }

    //update todays list based on input change
    public void UpdateTodaysPlanOnInput(int number)
    {
        if (number != int.Parse(SrNo.text) - 1)
            return;

        DailyPlanManager.instance._TodaysPlanData[number].TappingTempInput = InputTappingTemp.text;
        DailyPlanManager.instance._TodaysPlanData[number].PourTempFirstinput = InputPourtempFirst.text;
        DailyPlanManager.instance._TodaysPlanData[number].PourTempLastInput = InputPouringtempLast.text;
        DailyPlanManager.instance._TodaysPlanData[number].GradOfInput = InputGradcast.text;

        DailyPlanManager.instance._TodaysPlanData[number].CRCVInput = InputCRC.text;
        DailyPlanManager.instance._TodaysPlanData[number].CVInput = InputC.text;
        DailyPlanManager.instance._TodaysPlanData[number].SiVInput = InputSi.text;
        DailyPlanManager.instance._TodaysPlanData[number].MnVInput = InputMn.text;
        DailyPlanManager.instance._TodaysPlanData[number].CuVInput = InputCu.text;
        DailyPlanManager.instance._TodaysPlanData[number].FMVInput = InputFM.text;
        DailyPlanManager.instance._TodaysPlanData[number].ChrVInput = InputChr.text;
        DailyPlanManager.instance._TodaysPlanData[number].CiVInput = InputCi.text;
        DailyPlanManager.instance._TodaysPlanData[number].FeVInput = InputFe.text;

        DailyPlanManager.instance._TodaysPlanData[number].FCRC = FCRC.text;
        DailyPlanManager.instance._TodaysPlanData[number].FC = FC.text;
        DailyPlanManager.instance._TodaysPlanData[number].FSi = FSi.text;
        DailyPlanManager.instance._TodaysPlanData[number].FMn = FMn.text;
        DailyPlanManager.instance._TodaysPlanData[number].FCu = FCu.text;
        DailyPlanManager.instance._TodaysPlanData[number].FFM = FFM.text;
        DailyPlanManager.instance._TodaysPlanData[number].FChr = FChr.text;
        DailyPlanManager.instance._TodaysPlanData[number].FCi = FCi.text;
        DailyPlanManager.instance._TodaysPlanData[number].FFe = FFe.text;

        for(int i = 0; i < _TMaterials.Count; i++)
        {
            DailyPlanManager.instance._TodaysPlanData[number]._InputMaterials[i] = _IMat[i].text;
            DailyPlanManager.instance._TodaysPlanData[number]._FinalMaterials[i] = _FMat[i].text;
        }



        DailyPlanManager.instance.SaveTodaysPlanToPlayerPref();

    }

    //changes done related to temp and other valuse 
    public void OnOtherValueChange()
    {
        int number = int.Parse(SrNo.text) - 1;
        DailyPlanManager.instance._TodaysPlanData[number].TappingTempInput = InputTappingTemp.text;
        DailyPlanManager.instance._TodaysPlanData[number].PourTempFirstinput = InputPourtempFirst.text;
        DailyPlanManager.instance._TodaysPlanData[number].PourTempLastInput = InputPouringtempLast.text;
        DailyPlanManager.instance._TodaysPlanData[number].GradOfInput = InputGradcast.text;

        DailyPlanManager.instance.SaveTodaysPlanToPlayerPref();
    }

    //on Item remove from TodaysPlan

    public void OnItemRemove()
    {
        int number = int.Parse(SrNo.text) - 1;
        DailyPlanManager.instance._TodaysPlanData.RemoveAt(number);
        DailyPlanManager.instance.SaveTodaysPlanToPlayerPref();

        Destroy(this.gameObject);
        DailyPlanManager.instance.OnTodaysPlanListCreate();
    }

}
