using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

public class DailyPlanManager : MonoBehaviour
{
    public static DailyPlanManager instance = null;

    [SerializeField]
    TMP_InputField _ImportPath;
    private int CurrentIndex;
    private DateTime _HeighestTime;
    public GameObject _NextItemObject;

    [Space]
    [Header("Instance Objects")]
    [SerializeField]
    GameObject _ParentObject;
    [SerializeField]
    GameObject _InstanceObj;

    [Space]
    [Header("Display1 Objects")]
    [SerializeField]
    TMP_InputField _CastingGrade;
    [SerializeField]
    TMP_InputField _TappingTemp, _PouringTempFirst, _PouringTempLast, _Srno;
    public GameObject DisplayCanvas;

    [Space]
    [Header("Diplay2 Objects")]
    [SerializeField]
    TMP_Text _DCastGrade;
    [SerializeField]
    TMP_Text _DTappingTemp, _DPouringTemp1st, _DPouringTemplast, _DSrno, _DItemName, _DTappingTemp2, _DCastGrade2, _DPouringTemp1st2, _DPouringTemplast2, _DSrno2, _DItemName2, Crc, C, Si, Mn, Cu, FM, TotalCharg, Ci, PigIron;
    [SerializeField]
    List<TMP_Text> _DmatNameList = new List<TMP_Text>();
    [SerializeField]
    RawImage _Dimage, _Dimage2;
    public List<TMP_Text> _DMat = new List<TMP_Text>();

    [Space]
    [Header("Import Items extras")]
    public string[] _dataList;
    public List<PlanData> _plandata = new List<PlanData>();
    public int CurrentItem = 0;
    public UnityWinTTS _VoiceManager;

    [Space]
    [Header("Daily Plan Searchterm")]
    public TMP_InputField _SearchDailyPlan;

    [Space]
    [Header("Today's Plan List")]
    public List<PlanData> _TodaysPlanData = new List<PlanData>();
    public GameObject TodayPlanParent;
    public GameObject TodaysPlanPrefeb;

    [Space]
    [Header("Sum of charges")]
    public List<TMP_Text> _sumMat = new List<TMP_Text>();
    public List<TMP_Text> _sumValue = new List<TMP_Text>();



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        OnGetPlan();
        OnTodayPlan();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Get Master Plan
    public void OnGetPlan()
    {
        if (UIManager.Instance.prefDailyPlan == "")
            return;

        PlanlistClass _list = JsonUtility.FromJson<PlanlistClass>(UIManager.Instance.prefDailyPlan);

        _plandata.Clear();
        _plandata = _list._data;
        createPlanList();
    }

    //Get Todays Plan
    public void OnTodayPlan()
    {
        if (UIManager.Instance.prefTodayPlan == "")
            return;

        PlanlistClass _list = JsonUtility.FromJson<PlanlistClass>(UIManager.Instance.prefTodayPlan);

        _TodaysPlanData.Clear();
        _TodaysPlanData = _list._data;
        OnTodaysPlanListCreate();

    }

    #region ImportItems
    public void ImportPlan()
    {
        if (_ImportPath.text == "")
        {
            UIManager.Instance.ShowNotification("please insert path !!");
            return;
        }

        string path = _ImportPath.text;

        FileInfo info = new FileInfo(path);

        _plandata.Clear();

        string textdata = "";

        if (info.Extension != ".csv")
        {
            UIManager.Instance.ShowNotification("Please import .csv file only !!!");
            return;
        }

        try
        {
            textdata = System.IO.File.ReadAllText(path);
        }
        catch
        {
            UIManager.Instance.ShowNotification("File is open!!");
            return;
        }

        _dataList = null;
        _dataList = textdata.Split('\n');
        _plandata.Clear();





        for (int i = 1; i < _dataList.Length - 1; i++)
        {
            PlanData _data = new PlanData();

            string[] rawlist = _dataList[i].Split(',');

            //Import Material Name 
            string[] rawMatList = _dataList[0].Split(',');

            _data._MaterialName.Add(rawMatList[7]);
            _data._MaterialName.Add(rawMatList[8]); _data._MaterialName.Add(rawMatList[9]); _data._MaterialName.Add(rawMatList[10]);
            _data._MaterialName.Add(rawMatList[11]); _data._MaterialName.Add(rawMatList[12]); _data._MaterialName.Add(rawMatList[13]);
            _data._MaterialName.Add(rawMatList[14]); _data._MaterialName.Add(rawMatList[15]); _data._MaterialName.Add(rawMatList[16]);
            _data._MaterialName.Add(rawMatList[17]); _data._MaterialName.Add(rawMatList[18]);


            _data.SrNo = rawlist[0];
            _data.ItemName = rawlist[1];
            _data.TappingTemp = rawlist[2];
            _data.PourTempFirst = rawlist[3];
            _data.PourTempLast = rawlist[4];
            _data.GradOf = rawlist[5];
            _data.image = rawlist[6];
            _data.CRCV = rawlist[7];
            _data.FMV = rawlist[8];
            _data.CiV = rawlist[9];
            _data.FeV = rawlist[10];
            _data.CV = rawlist[11];
            _data.SiV = rawlist[12];
            _data.MnV = rawlist[13];
            _data.CuV = rawlist[14];

            Debug.Log(rawlist[7] + " " + rawlist[8] + " " + rawlist[9] + " " + rawlist[10]);

            _data.ChrV = (float.Parse(rawlist[7]) + float.Parse(rawlist[8]) + float.Parse(rawlist[9]) + float.Parse(rawlist[10])).ToString();


            for (int j = 7; j < 19; j++)
            {
                _data._Materials.Add(rawlist[j]);
            }

            _plandata.Add(_data);


        }

        PlanlistClass _list = new PlanlistClass();
        _list._data = _plandata;

        string json = JsonUtility.ToJson(_list);
        UIManager.Instance.prefDailyPlan = json;

        createPlanList();

        UIManager.Instance.ShowNotification("Plan imported succesfully");

    }
    #endregion

    // Create entire planlist
    public void createPlanList()
    {
        foreach (Transform child in _ParentObject.transform)
        {
            Destroy(child.gameObject);
        }


        for (int i = 0; i < _plandata.Count; i++)
        {
            GameObject _Instance = Instantiate(_InstanceObj, _ParentObject.transform);
            PlanList _list = _Instance.GetComponent<PlanList>();

            _list.SrNo.text = _plandata[i].SrNo;
            _list.ItemName.text = _plandata[i].ItemName;
            _list.TappingTemp.text = _plandata[i].TappingTemp;
            _list.PourtempFirst.text = _plandata[i].PourTempFirst;
            _list.PouringtempLast.text = _plandata[i].PourTempLast;
            _list.Gradcast.text = _plandata[i].GradOf;
            _list.Imagepath = _plandata[i].image;

            _list.CRC.text = _plandata[i].CRCV;
            _list.C.text = _plandata[i].CV;
            _list.Si.text = _plandata[i].SiV;
            _list.Mn.text = _plandata[i].MnV;
            _list.Cu.text = _plandata[i].CuV;
            _list.FM.text = _plandata[i].FMV;
            _list.Chr.text = _plandata[i].ChrV;
            _list.Ci.text = _plandata[i].CiV;
            _list.Fe.text = _plandata[i].FeV;

            for (int j = 0; j < _plandata[i]._Materials.Count; j++)
                _list._TMaterials[j].text = _plandata[i]._Materials[j];

            //create material name list
            for (int k = 0; k < _plandata[i]._MaterialName.Count; k++)
                _list._MatNameList[k].text = _plandata[i]._MaterialName[k];
        }

   

    }

    //Update Material Sum
    public void UpdateMaterialSum()
    {
        //Debug.Log("todays length" + _TodaysPlanData[0]._MaterialName.Count);
        //  Debug.Log("sum length " + _sumMat.Count);

        if (_TodaysPlanData.Count > 0)
        {
            for (int i = 0; i < _TodaysPlanData[0]._MaterialName.Count; i++)
            {
                _sumMat[i].text = _TodaysPlanData[0]._MaterialName[i];

            }

            for (int j = 0; j < _TodaysPlanData[0]._MaterialName.Count; j++)
            {
                float sumvalue = 0;
                for (int i = 0; i < _TodaysPlanData.Count; i++)
                {
                    sumvalue = sumvalue + float.Parse(_TodaysPlanData[i]._FinalMaterials[j]);
                }
                _sumValue[j].text = sumvalue.ToString();
            }
        }



    }


    public void OnNextItemClick()
    {
        if (CurrentItem >= _TodaysPlanData.Count)
            return;

        CurrentItem++;
        DataReader.Instance._OnNextClick = true;
        DisplayCanvas.SetActive(true);
        _CastingGrade.text = _DCastGrade.text = _DCastGrade2.text = _TodaysPlanData[CurrentItem - 1].GradOfInput;
        _TappingTemp.text = _DTappingTemp.text = _DTappingTemp2.text = _TodaysPlanData[CurrentItem - 1].TappingTempInput;
        _PouringTempFirst.text = _DPouringTemp1st.text = _DPouringTemp1st2.text = _TodaysPlanData[CurrentItem - 1].PourTempFirstinput;
        _PouringTempLast.text = _DPouringTemplast.text = _DPouringTemplast2.text = _TodaysPlanData[CurrentItem - 1].PourTempLastInput;
        _Srno.text = _DSrno.text = _DSrno2.text = _TodaysPlanData[CurrentItem - 1].SrNo;
        _DItemName.text = _DItemName2.text = _TodaysPlanData[CurrentItem - 1].ItemName;

        Crc.text = _TodaysPlanData[CurrentItem - 1].FCRC;
        C.text = _TodaysPlanData[CurrentItem - 1].FC;
        Si.text = _TodaysPlanData[CurrentItem - 1].FSi;
        Mn.text = _TodaysPlanData[CurrentItem - 1].FMn;
        Cu.text = _TodaysPlanData[CurrentItem - 1].FCu;
        FM.text = _TodaysPlanData[CurrentItem - 1].FFM;
        TotalCharg.text = _TodaysPlanData[CurrentItem - 1].FChr;

        Ci.text = _TodaysPlanData[CurrentItem - 1].FCi;
        PigIron.text = _TodaysPlanData[CurrentItem - 1].FFe;

        for (int i = 0; i < _DMat.Count; i++)
            _DMat[i].text = _TodaysPlanData[CurrentItem - 1]._FinalMaterials[i];

        for (int i = 0; i < _DmatNameList.Count; i++)
            _DmatNameList[i].text = _TodaysPlanData[CurrentItem - 1]._MaterialName[i];

        SetImage(_TodaysPlanData[CurrentItem - 1].image);
        _NextItemObject.SetActive(true);

        StartCoroutine(WaitForActive());

    }


    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(1.0f);
        _VoiceManager.ButtonPress(_DItemName.text.ToString());
    }

    IEnumerator WaitForActivePrevious()
    {
        yield return new WaitForSeconds(1.0f);
        _VoiceManager.ButtonPressPrevious(_DItemName.text.ToString());
    }

    public void OnPreviousItemClick()
    {
        if (CurrentItem == 0 || CurrentItem == 1)
            return;
        CurrentItem = CurrentItem - 1;
        DataReader.Instance._OnNextClick = true;
        DisplayCanvas.SetActive(true);
        _CastingGrade.text = _DCastGrade.text = _DCastGrade2.text = _TodaysPlanData[CurrentItem - 1].GradOfInput;
        _TappingTemp.text = _DTappingTemp.text = _DTappingTemp2.text = _TodaysPlanData[CurrentItem - 1].TappingTempInput;
        _PouringTempFirst.text = _DPouringTemp1st.text = _DPouringTemp1st2.text = _TodaysPlanData[CurrentItem - 1].PourTempFirstinput;
        _PouringTempLast.text = _DPouringTemplast.text = _DPouringTemplast2.text = _TodaysPlanData[CurrentItem - 1].PourTempLastInput;
        _Srno.text = _DSrno.text = _DSrno2.text = _TodaysPlanData[CurrentItem - 1].SrNo;
        _DItemName.text = _DItemName2.text = _TodaysPlanData[CurrentItem - 1].ItemName;

        Crc.text = _TodaysPlanData[CurrentItem - 1].FCRC;
        C.text = _TodaysPlanData[CurrentItem - 1].FC;
        Si.text = _TodaysPlanData[CurrentItem - 1].FSi;
        Mn.text = _TodaysPlanData[CurrentItem - 1].FMn;
        Cu.text = _TodaysPlanData[CurrentItem - 1].FCu;
        FM.text = _TodaysPlanData[CurrentItem - 1].FFM;
        TotalCharg.text = _TodaysPlanData[CurrentItem - 1].FChr;
        Ci.text = _TodaysPlanData[CurrentItem - 1].FCi;
        PigIron.text = _TodaysPlanData[CurrentItem - 1].FFe;

        SetImage(_TodaysPlanData[CurrentItem - 1].image);

        for (int i = 0; i < _DMat.Count; i++)
            _DMat[i].text = _TodaysPlanData[CurrentItem - 1]._FinalMaterials[i];


        for (int i = 0; i < _DmatNameList.Count; i++)
            _DmatNameList[i].text = _TodaysPlanData[CurrentItem - 1]._MaterialName[i];

        if (CurrentItem == 0)
            CurrentItem = 1;
        _NextItemObject.SetActive(true);
        StartCoroutine(WaitForActivePrevious());

    }

    public void onSetItemClick()
    {
        if (_Srno.text == "")
            return;

        for (int i = 0; i < _TodaysPlanData.Count; i++)
        {
            if (_Srno.text.ToLower() == _TodaysPlanData[i].SrNo.ToLower())
            {
                DataReader.Instance._OnNextClick = true;
                DisplayCanvas.SetActive(true);
                CurrentItem = i;
                _CastingGrade.text = _DCastGrade.text = _DCastGrade2.text = _TodaysPlanData[CurrentItem].GradOfInput;
                _TappingTemp.text = _DTappingTemp.text = _DTappingTemp2.text = _TodaysPlanData[CurrentItem].TappingTempInput;
                _PouringTempFirst.text = _DPouringTemp1st.text = _DPouringTemp1st2.text = _TodaysPlanData[CurrentItem].PourTempFirstinput;
                _PouringTempLast.text = _DPouringTemplast.text = _DPouringTemplast2.text = _TodaysPlanData[CurrentItem].PourTempLastInput;
                _Srno.text = _DSrno.text = _DSrno2.text = _TodaysPlanData[CurrentItem].SrNo;
                _DItemName.text = _DItemName2.text = _TodaysPlanData[CurrentItem].ItemName;

                Crc.text = _TodaysPlanData[CurrentItem].FCRC;
                C.text = _TodaysPlanData[CurrentItem].FC;
                Si.text = _TodaysPlanData[CurrentItem].FSi;
                Mn.text = _TodaysPlanData[CurrentItem].FMn;
                Cu.text = _TodaysPlanData[CurrentItem].FCu;
                FM.text = _TodaysPlanData[CurrentItem].FFM;
                TotalCharg.text = _TodaysPlanData[CurrentItem].FChr;
                Ci.text = _TodaysPlanData[CurrentItem].FCi;
                PigIron.text = _TodaysPlanData[CurrentItem].FFe;

                for (int j = 0; j < _DMat.Count; j++)
                    _DMat[j].text = _TodaysPlanData[CurrentItem]._FinalMaterials[j];


                for (int k = 0; k < _DmatNameList.Count; k++)
                    _DmatNameList[k].text = _TodaysPlanData[CurrentItem]._MaterialName[k];

                SetImage(_TodaysPlanData[CurrentItem].image);
                CurrentItem++;

                _NextItemObject.SetActive(true);

                StartCoroutine(WaitForActive());
                return;
            }

        }

        UIManager.Instance.ShowNotification("Item not found");
    }

    void SetImage(string Imagepath)
    {
        Debug.Log("set images");
        if (Imagepath != "")
        {
            Debug.Log("set images22");
            try
            {
                byte[] byteArray = File.ReadAllBytes(@Imagepath);
                //create a texture and load byte array to it
                // Texture size does not matter 
                Texture2D sampleTexture = new Texture2D(2, 2);
                // the size of the texture will be replaced by image size
                bool isLoaded = sampleTexture.LoadImage(byteArray);
                // apply this texure as per requirement on image or material

                _Dimage.texture = sampleTexture;
                _Dimage2.texture = sampleTexture;
            }
            catch
            {
                return;
            }

        }

    }

    // Find result from daily plan and display them
    public void OnSearchItemClick()
    {
        if (_SearchDailyPlan.text == "")
        {
            createPlanList();
            return;
        }

        List<PlanData> SearchDataList = new List<PlanData>();

        for (int i = 0; i < _plandata.Count; i++)
        {
            if (_plandata[i].ItemName.Contains(_SearchDailyPlan.text))
            {
                SearchDataList.Add(_plandata[i]);
            }
        }

        if (SearchDataList.Count == 0)
            return;

        foreach (Transform child in _ParentObject.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < SearchDataList.Count; i++)
        {
            GameObject _Instance = Instantiate(_InstanceObj, _ParentObject.transform);
            PlanList _list = _Instance.GetComponent<PlanList>();

            _list.SrNo.text = SearchDataList[i].SrNo;
            _list.ItemName.text = SearchDataList[i].ItemName;
            _list.TappingTemp.text = SearchDataList[i].TappingTemp;
            _list.PourtempFirst.text = SearchDataList[i].PourTempFirst;
            _list.PouringtempLast.text = SearchDataList[i].PourTempLast;
            _list.Gradcast.text = SearchDataList[i].GradOf;
            _list.Imagepath = SearchDataList[i].image;
            _list.CRC.text = SearchDataList[i].CRCV;
            _list.C.text = SearchDataList[i].CV;
            _list.Si.text = SearchDataList[i].SiV;
            _list.Mn.text = SearchDataList[i].MnV;
            _list.Cu.text = SearchDataList[i].CuV;
            _list.FM.text = SearchDataList[i].FMV;
            _list.Chr.text = SearchDataList[i].ChrV;
            _list.Ci.text = SearchDataList[i].CiV;
            _list.Fe.text = SearchDataList[i].FeV;
        }
    }

    //Create Todays Plan List
    public void OnTodaysPlanListCreate()
    {
        SaveTodaysPlanToPlayerPref();

        foreach (Transform child in TodayPlanParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _TodaysPlanData.Count; i++)
        {
            GameObject _Instance = Instantiate(TodaysPlanPrefeb, TodayPlanParent.transform);
            PlanList _list = _Instance.GetComponent<PlanList>();

            _list.SrNo.text = (i + 1).ToString();
            _list.ItemName.text = _TodaysPlanData[i].ItemName;
            _list.TappingTemp.text = _TodaysPlanData[i].TappingTemp;
            _list.PourtempFirst.text = _TodaysPlanData[i].PourTempFirst;
            _list.PouringtempLast.text = _TodaysPlanData[i].PourTempLast;
            _list.Gradcast.text = _TodaysPlanData[i].GradOf;
            _list.Imagepath = _TodaysPlanData[i].image;
            _list.CRC.text = _TodaysPlanData[i].CRCV;
            _list.C.text = _TodaysPlanData[i].CV;
            _list.Si.text = _TodaysPlanData[i].SiV;
            _list.Mn.text = _TodaysPlanData[i].MnV;
            _list.Cu.text = _TodaysPlanData[i].CuV;
            _list.FM.text = _TodaysPlanData[i].FMV;
            _list.Chr.text = _TodaysPlanData[i].ChrV;
            _list.Ci.text = _TodaysPlanData[i].CiV;
            _list.Fe.text = _TodaysPlanData[i].FeV;

            for (int j = 0; j < _TodaysPlanData[i]._Materials.Count; j++)
                _list._TMaterials[j].text = _TodaysPlanData[i]._Materials[j];

            _list.InputTappingTemp.text = _TodaysPlanData[i].TappingTempInput;
            _list.InputPourtempFirst.text = _TodaysPlanData[i].PourTempFirstinput;
            _list.InputPouringtempLast.text = _TodaysPlanData[i].PourTempLastInput;
            _list.InputGradcast.text = _TodaysPlanData[i].GradOfInput;
            _list.InputCRC.text = _TodaysPlanData[i].CRCVInput;
            _list.InputC.text = _TodaysPlanData[i].CVInput;
            _list.InputSi.text = _TodaysPlanData[i].SiVInput;
            _list.InputMn.text = _TodaysPlanData[i].MnVInput;
            _list.InputCu.text = _TodaysPlanData[i].CuVInput;
            _list.InputFM.text = _TodaysPlanData[i].FMVInput;
            _list.InputChr.text = _TodaysPlanData[i].ChrVInput;
            _list.InputCi.text = _TodaysPlanData[i].CiVInput;
            _list.InputFe.text = _TodaysPlanData[i].FeVInput;

            for (int j = 0; j < _TodaysPlanData[i]._Materials.Count; j++)
                _list._IMat[j].text = _TodaysPlanData[i]._InputMaterials[j];

            _list.FCRC.text = _TodaysPlanData[i].FCRC;
            _list.FC.text = _TodaysPlanData[i].FC;
            _list.FSi.text = _TodaysPlanData[i].FSi;
            _list.FMn.text = _TodaysPlanData[i].FMn;
            _list.FCu.text = _TodaysPlanData[i].FCu;
            _list.FFM.text = _TodaysPlanData[i].FFM;
            _list.FChr.text = _TodaysPlanData[i].FChr;
            _list.FCi.text = _TodaysPlanData[i].FCi;
            _list.FFe.text = _TodaysPlanData[i].FFe;


            for (int j = 0; j < _TodaysPlanData[i]._Materials.Count; j++)
            {

                _list._FMat[j].text = _TodaysPlanData[i]._FinalMaterials[j];
            }

            //create material name list
            for (int k = 0; k < _TodaysPlanData[i]._MaterialName.Count; k++)
                _list._MatNameList[k].text = _TodaysPlanData[i]._MaterialName[k];


        }

        UpdateMaterialSum();

    }

    //saving todays plan in playerpref
    public void SaveTodaysPlanToPlayerPref()
    {
        Debug.Log("saving plan");
        PlanlistClass _listClass = new PlanlistClass();
        _listClass._data = _TodaysPlanData;

        string json = JsonUtility.ToJson(_listClass);
        UIManager.Instance.prefTodayPlan = json;

        Debug.Log("saving plan 2");
    }

    //Create Entire Todays Plan
    public void OnTodayPlanClear()
    {
        foreach (Transform child in TodayPlanParent.transform)
            Destroy(child.gameObject);

        _TodaysPlanData.Clear();

        SaveTodaysPlanToPlayerPref();
    }

    //set next screen

    public void SetNextScreen(float timeValue)
    {
        StartCoroutine(IenumSetCoroutine(timeValue));
    }

    //coroutine for next time
    IEnumerator IenumSetCoroutine(float timeValue)
    {
        yield return new WaitForSeconds(timeValue);
        OnNextItemClick();

    }



}


[System.Serializable]
public class PlanData
{
    public string SrNo, ItemName, TappingTemp, PourTempFirst, PourTempLast, GradOf, image, CRCV, CV, SiV, MnV, CuV, FMV, ChrV, CiV, FeV;
    public string TappingTempInput, PourTempFirstinput, PourTempLastInput, GradOfInput, imageInput, CRCVInput, CVInput, SiVInput, MnVInput, CuVInput, FMVInput, ChrVInput, CiVInput, FeVInput;
    public string FCRC, FC, FSi, FMn, FCu, FFM, FChr, FCi, FFe;
    public List<string> _Materials = new List<string>();
    public List<string> _InputMaterials = new List<string>();
    public List<string> _FinalMaterials = new List<string>();
    public List<string> _MaterialName = new List<string>();

}

[System.Serializable]
public class PlanlistClass
{
    public List<PlanData> _data = new List<PlanData>();
}
