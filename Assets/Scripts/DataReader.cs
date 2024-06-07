using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Drawing;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using Image = UnityEngine.UI.Image;
using Color = UnityEngine.Color;

[System.Serializable]
public class ImportedMaterial
{
    public string name;
    public float value;
}

public class DataReader : MonoBehaviour
{
    public static DataReader Instance = null;


    public TMP_Text _DummyText1, _Dummytext2;
    public bool _OnNextClick;
    public FileInfo[] info;
    public FileInfo _CurrentFile;
    public GameObject _NextItemObject, _NextItemObject2;
    public string _name;
    public string[] _dataList;

    [SerializeField]
    List<rawList> _rawlist;


    [SerializeField]
    List<int> _columnNumber, _MatColumnNumber, _MatNumber;


    [Space]
    [Header("For Muttha Only")]
    [Space]
    [SerializeField]
    List<string> _importedMat = new List<string>();
    [SerializeField]
    List<float> _importedvalue = new List<float>();
    [SerializeField]
    List<string> _tmpArray2 = new List<string>();
    [SerializeField]
    TMP_InputField _Timer;



    [Space]
    [Header("Melting Detail")]
    [SerializeField]
    Text _HeatNumber;
    [SerializeField]
    TMP_Dropdown _ItemName, _batchName, _PouringStatus, _SteelValues;
    [SerializeField]
    TMP_InputField _furnaceSize, _FurnaceNumber, _TappingTemp, _PouringTempFst, _PouringTempLst, _srNo;
    [SerializeField]
    Text _CastingGrade;

    [Space]
    [Header("material input")]
    [SerializeField]
    List<MaterialList> _MatList = new List<MaterialList>();

    [Space]
    [Header("Previous Material")]
    [SerializeField]
    List<MaterialList> _PreMatList = new List<MaterialList>();
    [SerializeField]
    TMP_InputField _PreItemName;

    [Tooltip("Suggest Mat")]
    [Space]
    [Header("Material Suggestion C,SI,MN,P,S,Cr,Cu,Fe")]
    [SerializeField]
    List<TMP_Text> Ilist = new List<TMP_Text>();
    [SerializeField]
    List<TMP_Text> IMatlist = new List<TMP_Text>();



    [Space]
    [Header("After Steel Addition S,Cr,Cu,Mn,P")]
    [SerializeField]
    List<TMP_Text> Alist;

    [Space]
    [Header("Image List")]
    [SerializeField]
    Image ImSi;
    [SerializeField]
    List<SuggestionStyle> _D1SuggestionStyle = new List<SuggestionStyle>();


    [Space]
    [Header("Display List")]
    [SerializeField]
    List<MaterialList> _MatDisplayList = new List<MaterialList>();
    [SerializeField]
    List<SuggestionStyle> _D2SuggestionStyle = new List<SuggestionStyle>();


    [Space]
    [Header("Display Header")]
    [SerializeField]
    Text _DHeatNum, _DGradeofcast;
    [SerializeField]
    TMP_Text _DItemName, _DFurnaceNumber, _DPoiringStatus;

    [Space]
    [Header("Display footer suggestion C,SI,MN,P,S,Cr,Cu,Fe")]
    [SerializeField]
    List<TMP_Text> Dlist = new List<TMP_Text>();

    [Space]
    [Header("Manager")]
    [SerializeField]
    ItemManager _ItemManager;
    [SerializeField]
    BatchesManager _BatchManager;


    [Space]
    [Header("data as per selection")]
    [SerializeField]
    ItemDetail _CItemDetail;
    [SerializeField]
    BatchesList _CBatchDetail;
    [SerializeField]
    BatchesList _SteelList;

    [Space]
    [Header("Color selection")]
    [SerializeField]
    Color _low;
    [SerializeField]
    Color _low2, _high, _high2;

    [Space]
    [Header("History")]
    public HistoryList _ListHistory;
    public HistoryList _DailyHistoryList;
    public GameObject _HistoryParent, _HistoryChild;
    public TMP_Dropdown _DropMonth, _DropYear;
    public TMP_InputField _ExportPath;

    [Space]
    [Header("Random History")]
    public TMP_Dropdown _FromDay;
    public TMP_Dropdown _FromMonth, _FromYear, _ToDay, _ToMonth, _ToYear;
    public TMP_InputField _RandomExportPath;

    [Space]
    [Header("Carbon changed")]
    public TMP_InputField _carbonChanged;

    private int CurrentIndex;
    private DateTime _HeighestTime;
    private DirectoryInfo dir;

    private DateTime _InitDirTime, _CurrentTime;
    private bool once;
    public AudioSource _AudioSource;
    public AudioClip _clip, _CorrectClip;
    private string textdata;


    Coroutine _coroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        _furnaceSize.text = "500";
        _ExportPath.text = UIManager.Instance.PrefWriteHistoryPath;
        _AudioSource = this.GetComponent<AudioSource>();
        // AudioClip clip1 = (AudioClip)Resources.Load("1.wav");
        // _AudioSource.clip = clip1;
        GetHistoryOfThisMonth();
        _AudioSource.PlayOneShot(_clip);
        _Timer.text = PlayerPrefs.GetString("TIMER");

    }


    // Update is called once per frame
    void Update()
    {



    }

    //Get record of this month 
    public void GetHistoryOfThisMonth()
    {
        string key = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();
        string dailykey = "DailyKey" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();

        if ((System.DateTime.Now.Month - 3) == 0)
        {
            if (PlayerPrefs.HasKey(12 + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey(12 + (System.DateTime.Now.Year - 1).ToString());
            if (PlayerPrefs.HasKey("DailyKey" + System.DateTime.Now.Day.ToString() + 12.ToString() + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey("DailyKey" + System.DateTime.Now.Day.ToString() + 12.ToString() + (System.DateTime.Now.Year - 1).ToString());
        }

        else if ((System.DateTime.Now.Month - 3) == -1)
        {
            if (PlayerPrefs.HasKey(11 + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey(11 + (System.DateTime.Now.Year - 1).ToString());
            if (PlayerPrefs.HasKey("DailyKey" + System.DateTime.Now.Day.ToString() + 11.ToString() + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey("DailyKey" + System.DateTime.Now.Day.ToString() + 11.ToString() + (System.DateTime.Now.Year - 1).ToString());
        }

        else if ((System.DateTime.Now.Month - 3) == -2)
        {
            if (PlayerPrefs.HasKey(10 + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey(10 + (System.DateTime.Now.Year - 1).ToString());
            if (PlayerPrefs.HasKey("DailyKey" + System.DateTime.Now.Day.ToString() + 10.ToString() + (System.DateTime.Now.Year - 1).ToString()))
                PlayerPrefs.DeleteKey("DailyKey" + System.DateTime.Now.Day.ToString() + 10.ToString() + (System.DateTime.Now.Year - 1).ToString());
        }


        string oldkey = (System.DateTime.Now.Month - 3).ToString() + System.DateTime.Now.Year.ToString();
        string oldDailyKey = "DailyKey" + System.DateTime.Now.Day.ToString() + (System.DateTime.Now.Month - 3).ToString() + System.DateTime.Now.Year.ToString();

        if (PlayerPrefs.HasKey(oldkey))
            PlayerPrefs.DeleteKey(oldkey);

        if (PlayerPrefs.HasKey(oldDailyKey))
            PlayerPrefs.DeleteKey(oldDailyKey);

        if (PlayerPrefs.HasKey(key))
            _ListHistory = JsonUtility.FromJson<HistoryList>(PlayerPrefs.GetString(key));

        if (PlayerPrefs.HasKey(dailykey))
            _DailyHistoryList = JsonUtility.FromJson<HistoryList>(PlayerPrefs.GetString(dailykey));
    }

    public void OnLogin()
    {
        _coroutine = StartCoroutine(IenumTakedata());
    }

    public void OnLogout()
    {
        StopCoroutine(_coroutine);
    }

    IEnumerator IenumTakedata()
    {
        while (true)
        {
            ReadFiles(false);
            yield return new WaitForSeconds(5.0f);
        }
    }


    #region ReadLogin 


    //@@@ @@@ @@@ PlEASE MODIFY BOTH METHOD MENTIONED BELOW -       Readfiles and SetCarbon 

    // for taking carbon automatically 
    public void ReadFiles(bool forceRead)
    {
        if (UIManager.Instance.PrefReadPath == "")
            return;

        string path = UIManager.Instance.PrefReadPath;

         FileInfo file = new FileInfo(@path);


        //DirectoryInfo di = new DirectoryInfo(path);
        //FileInfo file = (from f in di.GetFiles()
        //                 orderby f.LastWriteTime descending
        //                 select f).First();


        if (!once)
        {
            _CurrentTime = file.LastWriteTime;
        }

        CurrentIndex = 0;
        _HeighestTime = file.LastWriteTime;

        if (once && !forceRead)
        {
            if (_HeighestTime <= _CurrentTime)
                return;
        }

        try
        {
            //Debug.Log("path" + file.FullName);
            textdata = System.IO.File.ReadAllText(file.FullName);
        }
        catch
        {
            UIManager.Instance.ShowNotification("File is open !!");
            return;
        }

        _NextItemObject.SetActive(false);
        _NextItemObject2.SetActive(false);
        if (ValueManager.IenumValue != null)
            StopCoroutine(ValueManager.IenumValue);
        //_AudioSource.Play();

        _CurrentTime = _HeighestTime;
        once = true;

        _dataList = textdata.Split('\n');


        /*
		using (StreamWriter file = File.CreateText(HISTORYPATH + System.DateTime.Now + info[CurrentIndex].Name))
		{
			foreach (string line in _dataList)
			{
				file.Write(line);
			}
		}*/

        _rawlist.Clear();

        // For Tuilsi - Varun
        //rawList _r = new rawList();
        //_rawlist.Add(_r);
        //string[] _reachraw = _dataList;
        //_r._stringList = _reachraw;

        //For Finex
        for (int i = 0; i < _dataList.Length; i++)
        {

            rawList _r = new rawList();
            _rawlist.Add(_r);
            string[] _reachraw = _dataList[i].Split('.');
            _r._stringList = _reachraw.ToList();
        }

        FindRawNumberHavingMeanValue();
        // Find raw number which contain mean value 
        FindRawNumberWhichContainMat();

        GetHeatInfo();

        GetMatvalueData();

        ShowPreviousValues();

        ShowData(false);

        OnItemSelected(false);

        OnDisplay(false);

        Debug.Log("File Readed successfully");

        DailyExport(false);

        SetSoundForAllCorrect();

        CheckNextScreen();
    }


    // Find raw number which contain mean value
    public void FindRawNumberHavingMeanValue()
    {
        _columnNumber.Clear();
        for (int k = 0; k < _rawlist.Count; k++)
        {

            string value = _rawlist[k]._stringList.Find(x => x.Contains("<x>"));
            /// Debug.Log("x value::" + value);
            if (value != null)
            {
                _columnNumber.Add(k);
            }
        }
    }

    //find raw number which contain materials 
    public void FindRawNumberWhichContainMat()
    {
        _MatColumnNumber.Clear();
        for (int k = 0; k < _rawlist.Count; k++)
        {

            string value = _rawlist[k]._stringList.Find(x => x.Contains("%"));
            // Debug.Log("x value::" + value);
            if (value != null)
            {
                _MatColumnNumber.Add(k);
            }
        }
    }



    // check next screen after final
    public void CheckNextScreen()
    {
        Debug.Log("next screen " + _PouringStatus.value);
        if (_PouringStatus.value == 0 && _Timer.text != "")
        {
            PlayerPrefs.SetString("TIMER", _Timer.text);
            Debug.Log("next plan");
            DailyPlanManager.instance.SetNextScreen(float.Parse(_Timer.text));
        }

    }



    //Set sound when all materals are green
    public void SetSoundForAllCorrect()
    {
        int count = 0;
        for (int i = 0; i < _MatList.Count - 1; i++)
        {
            if (_MatList[i]._Bgimage.color == Color.green || _MatList[i]._Bgimage.color == Color.white)
            {
                count++;

            }
        }

        if (count == (_MatList.Count - 1))
            _AudioSource.PlayOneShot(_CorrectClip);
        else
            _AudioSource.Play();


        Debug.Log("count value" + count);

    }

    public void GetHeatInfo()
    {
        _tmpArray2.Clear();

        //int dateid = 0;
        //for (int i = 0; i < _dataList.Length; i++)
        //{
        //    if (_dataList[i].Contains('-'))
        //    {
        //        dateid = i;
        //        i = _dataList.Length;
        //    }
        //}

        //Debug.Log("heat string " + _dataList[dateid]);

        string[] separatingStrings = { " " };
        string[] _tmpArray1 = _dataList[13].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);


        string newtext = "";
        for (int i = 0; i < _tmpArray1.Length; i++)
        {
            _tmpArray1[i] = ReplaceWhitespace(_tmpArray1[i], "");
            newtext = newtext + _tmpArray1[i];

            if (_tmpArray1[i].Length > 0)
            {
                _tmpArray2.Add(_tmpArray1[i]);
            }
        }


    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }



    //for getting materials and value seprate for each row
    public void GetMatvalueData()
    {

        _importedMat.Clear();
        _importedvalue.Clear();


        //for getting materiala
        string[] separatingStrings = { " " };

        for (int j = 0; j < _MatColumnNumber.Count; j++)
        {
            string[] _tmpArray1 = _rawlist[_MatColumnNumber[j] - 1]._stringList[1].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string _tmp in _tmpArray1)
                _importedMat.Add(_tmp);
        }



        //for getting values
        string[] _tmpArray6 = _rawlist[_columnNumber[0]]._stringList[0].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        string fstChar = _tmpArray6[1];
        Debug.Log("first character " + fstChar);
        string floatvalue = "";


        for (int j = 0; j < _columnNumber.Count; j++)
        {
            for (int i = 1; i < _rawlist[_columnNumber[j]]._stringList.Count; i++)
            {
                // Debug.Log("string data" + i);
                string[] _tmpArray7 = _rawlist[_columnNumber[j]]._stringList[i].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string _tmp in _tmpArray7)
                {
                    // Debug.Log("7value " + _tmp);
                }

                //Debug.Log("fst chaarcter" + fstChar);

                if (_tmpArray7.Length > 2)
                {
                    floatvalue = (fstChar + "." + _tmpArray7[1]);

                    string dummy1 = floatvalue.Replace('<', ' ').Trim();
                    string dummy2 = dummy1.Replace('>', ' ').Trim();
                    _importedvalue.Add(float.Parse(dummy2));

                    fstChar = _tmpArray7[2];

                    // Debug.Log("fst character" + fstChar);
                }
                else
                {
                    floatvalue = (fstChar + "." + _tmpArray7[1]);
                    // Debug.Log("ffvalue" + floatvalue);
                    string dummy1 = floatvalue.Replace('<', ' ').Trim();
                    string dummy2 = dummy1.Replace('>', ' ').Trim();
                    // Debug.Log("dummy 2" + dummy2);
                    _importedvalue.Add(float.Parse(dummy2));
                }
            }
        }

        _DummyText1.text = _importedvalue.Count.ToString();
        _Dummytext2.text = _importedMat.Count.ToString();


    }



    //Show previous item related values


    // For set carbon manually 
    public void SetCarbon(bool forceRead)
    {


        ShowPreviousValues();

        if (UIManager.Instance.PrefReadPath == "")
            return;

        string path = UIManager.Instance.PrefReadPath;

        FileInfo file = new FileInfo(@path);


        //DirectoryInfo di = new DirectoryInfo(path);
        //FileInfo file = (from f in di.GetFiles()
        //                 orderby f.LastWriteTime descending
        //                 select f).First();

        if (!once)
        {
            _CurrentTime = file.LastWriteTime;
        }

        CurrentIndex = 0;
        _HeighestTime = file.LastWriteTime;

        if (once && !forceRead)
        {
            if (_HeighestTime <= _CurrentTime)
                return;
        }

        try
        {
            textdata = System.IO.File.ReadAllText(file.FullName);
        }
        catch
        {
            UIManager.Instance.ShowNotification("File is open !!");
            return;
        }

        _NextItemObject.SetActive(false);
        _NextItemObject2.SetActive(false);
        if (ValueManager.IenumValue != null)
            StopCoroutine(ValueManager.IenumValue);
        //_AudioSource.Play();

        _CurrentTime = _HeighestTime;
        once = true;

        _dataList = textdata.Split('\n');

        /*
		using (StreamWriter file = File.CreateText(HISTORYPATH + System.DateTime.Now + info[CurrentIndex].Name))
		{
			foreach (string line in _dataList)
			{
				file.Write(line);
			}
		}*/

        _rawlist.Clear();

        for (int i = 0; i < _dataList.Length; i++)
        {
            rawList _r = new rawList();
            _rawlist.Add(_r);
            string[] _reachraw = _dataList[i].Split('.');
            _r._stringList = _reachraw.ToList();
        }

        FindRawNumberHavingMeanValue();
        FindRawNumberWhichContainMat();

        GetHeatInfo();

        GetMatvalueData();

        ShowData(true);

        OnItemSelected(true);

        OnDisplay(true);

        Debug.Log("File Readed successfully");

        DailyExport(true);

        SetSoundForAllCorrect();

        CheckNextScreen();
    }

    #endregion

    public void ShowPreviousValues()
    {
        _PreItemName.text = _DItemName.text;
        if (_PouringStatus.value == 0)
            _PreItemName.text = _PreItemName.text + "-F";
        else
            _PreItemName.text = _PreItemName.text + "-B";
        for (int i = 0; i < _MatList.Count; i++)
        {
            _PreMatList[i]._Material.text = _MatList[i]._Material.text;
            _PreMatList[i]._value.color = Color.black;
            _PreMatList[i]._value.text = _MatList[i]._value.text;
            _PreMatList[i]._Bgimage.color = _MatList[i]._Bgimage.color;
            if (_MatList[i]._Bgimage.color == _low)
                _PreMatList[i]._Bgimage.color = _low2;
            else if (_MatList[i]._Bgimage.color == _high)
                _PreMatList[i]._Bgimage.color = _high2;

        }
    }

    //for Finex
    public void ShowData(bool CarbonChanged)
    {


        for (int i = 0; i < _MatNumber.Count; i++)
        {

            _MatList[i]._Material.text = _importedMat[_MatNumber[i]];

            if (CarbonChanged && i == 0 && _carbonChanged.text != "")
                _MatList[i]._value.text = _carbonChanged.text;
            else
                _MatList[i]._value.text = _importedvalue[_MatNumber[i]].ToString("F4");
        }

        _MatList[_MatList.Count - 1]._value.text = ((float.Parse(_MatList[1]._value.text) / 3) + float.Parse(_MatList[0]._value.text) + float.Parse(_MatList[3]._value.text)).ToString("F4");

        int itemRawNo = 0;
        for (int k = 0; k < _rawlist.Count; k++)
        {
            string value = _rawlist[k]._stringList.Find(x => x.Contains("HEAT"));
            //Debug.Log("x value::" + value);
            if (value != null)
            {
                itemRawNo = k;
            }
        }


        string[] separatingStrings1 = { " ", "", "  " };
        string[] _tmpArray1 = _dataList[13].Split(separatingStrings1, System.StringSplitOptions.RemoveEmptyEntries);

        Debug.Log("test " + _tmpArray1.Length);

        int lenght = _tmpArray1.Length;
        string heat = _tmpArray1[1].ToString() + _tmpArray1[2].ToString();

        Debug.Log("heat" + heat);


        _HeatNumber.text = heat;

        if (_tmpArray1[2].ToString().Contains('B') || _tmpArray1[2].ToString().Contains('b'))
        {
            _FurnaceNumber.text = "BATH";
            _PouringStatus.value = 1;
        }

        else if (_tmpArray1[2].ToString().Contains('F') || _tmpArray1[2].ToString().Contains('f'))
        {
            _FurnaceNumber.text = "FINAL";
            _PouringStatus.value = 0;
        }

        if(_PouringStatus.value == 0)
        {
            string dummy = "";
            for(int j = 3; j < lenght - 2; j++)
            {
                dummy = dummy + _tmpArray1[j].ToString();
            }

            _CastingGrade.text = dummy;
        }
        else
        {
            string dummy = "";
            for (int j = 3; j < lenght - 1; j++)
            {
                dummy = dummy + _tmpArray1[j].ToString();
            }
            _CastingGrade.text = dummy;
        }


        string tempItemname = "";

        //for (int i = 7; i < _tmpArray2.Count; i++)
        //{
        //    if (i == 7)
        //        tempItemname = tempItemname + _tmpArray2[2].ToString();
        //    else
        //        tempItemname = tempItemname + " " + _tmpArray2[i].ToString();
        //}

        tempItemname = _tmpArray1[0].ToString();

        Debug.Log("Itemname " + tempItemname + " " + _PouringStatus.value);

        for (int j = 0; j < _ItemName.options.Count; j++)
        {


            if (tempItemname + "-F" == _ItemName.options[j].text && _PouringStatus.value == 0)
            {
                Debug.Log(tempItemname + "-F");
                _batchName.enabled = true;
                _SteelValues.enabled = true;
                _ItemName.value = j;

                return;

            }

            else if (tempItemname + "-B" == _ItemName.options[j].text && _PouringStatus.value == 1)
            {
                Debug.Log(tempItemname + "-B");
                _batchName.enabled = true;
                _SteelValues.enabled = true;
                _ItemName.value = j;

                return;

            }
            else
            {
                _ItemName.value = 0;
                _batchName.enabled = false;
                _SteelValues.enabled = false;

            }

        }

        if (_ItemName.value == 0)
        {
            _batchName.value = 0;
            _SteelValues.value = 0;
        }
        Debug.Log(";;;;;");
        Debug.Log("MAT 11" + _MatList[11]._value.text);
    }

    /* // For Madhuram
    public void ShowData(bool CarbonChanged)
    {


        for (int i = 0; i < _columnNumber.Count; i++)
        {
            //Debug.Log("valueeeeee" + _rawlist[0]._stringList[_columnNumber[i]]);

            _rawlist[0]._stringList[_columnNumber[i]] = _rawlist[0]._stringList[_columnNumber[i]].Replace(';', ' ').Trim();
            string[] _values = _rawlist[0]._stringList[_columnNumber[i]].Split(':');

            //_rawlist[_rawlist.Count - 2]._stringList[_columnNumber[i]] = _rawlist[_rawlist.Count - 2]._stringList[_columnNumber[i]].Replace('"', ' ').Trim();
            _MatList[i]._Material.text = _values[0];
            float value = float.Parse(_values[1]);
            //Debug.Log("final value" + value);

            if (CarbonChanged && i == 0 && _carbonChanged.text != "")
                _MatList[i]._value.text = _carbonChanged.text;
            else
                _MatList[i]._value.text = value.ToString("F3");
        }



        for (int i = 0; i < _datacolumnNumber.Count; i++)
        {


            //Debug.Log("string value ::" + _rawlist[0]._stringList[_datacolumnNumber[i]]);
            //Debug.Log("values od string ::" + _rawlist[_rawlist.Count - 2]._stringList[_datacolumnNumber[i]]);



            if (i == 0)
            {

                _rawlist[0]._stringList[_datacolumnNumber[0]] = _rawlist[0]._stringList[_datacolumnNumber[0]].Replace(';', ' ').Trim();
                _rawlist[0]._stringList[_datacolumnNumber[1]] = _rawlist[0]._stringList[_datacolumnNumber[1]].Replace(';', ' ').Trim();
                //_FurnaceNumber.text = _rawlist[_rawlist.Count - 2]._stringList[_datacolumnNumber[i]];

                string half = "";

                if (_rawlist[0]._stringList[_datacolumnNumber[0]].Contains("("))
                    half = _rawlist[0]._stringList[_datacolumnNumber[0]].Split('(')[1].Split(')')[0];

                //var result = _FurnaceNumber.text.Substring(_FurnaceNumber.text.Length - 1);
                if (half == "2")
                    _PouringStatus.value = 0;
                else
                    _PouringStatus.value = 1;

                _CastingGrade.text = _rawlist[0]._stringList[_datacolumnNumber[1]].Split('(')[0];
                _HeatNumber.text = _rawlist[0]._stringList[_datacolumnNumber[0]].Split('/')[1].Split('(')[0] + "(" + _rawlist[0]._stringList[_datacolumnNumber[1]].Split('(')[1];
            }


            if (i == 0)
            {
                string ItemName = _rawlist[0]._stringList[_datacolumnNumber[0]].Split('/')[0];
                string fItemname = ItemName;

                Debug.Log("Item name" + fItemname);

                if (ItemName.Contains(','))
                {
                    fItemname = ItemName.Replace(",", "");

                }

                for (int j = 0; j < _ItemName.options.Count; j++)
                {

                    //Debug.Log("Item namesssssssssssssssss:: " + _rawlist[_rawlist.Count - 2]._stringList[_datacolumnNumber[i]]);

                    //Debug.Log("Item optionssssssssssssss:: " + _ItemName.options[j].text);
                    string value = _ItemName.options[j].text;
                    //Debug.Log("item name::" + (_rawlist[_rawlist.Count - 2]._stringList[_datacolumnNumber[i]] + "-F"));
                    //Debug.Log("item from item list" + _ItemName.options[j].text);



                    if ((fItemname + "-2") == _ItemName.options[j].text && _PouringStatus.value == 0)
                    {
                        _batchName.enabled = true;
                        _SteelValues.enabled = true;
                        _ItemName.value = j;
                        return;
                    }

                    else if ((fItemname) == _ItemName.options[j].text && _PouringStatus.value == 1)
                    {
                        _batchName.enabled = true;
                        _SteelValues.enabled = true;
                        _ItemName.value = j;
                        return;
                    }
                    else
                    {

                        _ItemName.value = 0;
                        //_batchName.value = 0;
                        //_SteelValues.value = 0;
                        _batchName.enabled = false;
                        _SteelValues.enabled = false;
                    }

                }
            }


            if (_ItemName.value == 0)
            {
                _batchName.value = 0;
                _SteelValues.value = 0;
            }
        }
    }
    */
    public void OnDisplay(bool carbonChange)
    {
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        for (int i = 0; i < _MatList.Count; i++)
        {
            _MatDisplayList[i]._Material.text = _MatList[i]._Material.text;
            _MatDisplayList[i]._value.text = _MatList[i]._value.text;
            _MatDisplayList[i]._Bgimage.color = _MatList[i]._Bgimage.color;
            _MatDisplayList[i]._value.color = _MatList[i]._value.color;
        }

        _MatDisplayList[_MatDisplayList.Count - 1]._value.text = _MatList[_MatList.Count - 1]._value.text;

        SetHeader(_HeatNumber.text, _ItemName.options[_ItemName.value].text, _FurnaceNumber.text, _PouringStatus.options[_PouringStatus.value].text, _CastingGrade.text);

        HistoryMethod(carbonChange);

        //Debug.Log("issssssssssss" + ISi.text);

        for (int i = 0; i < Ilist.Count; i++)
        {
            Dlist[i].text = Ilist[i].text;
        }

        /*
		for(int d=0; d <_D1SuggestionStyle.Count; d++)
		{
			_D2SuggestionStyle[d].BG.color = _D1SuggestionStyle[d].BG.color;
			_D2SuggestionStyle[d].value.color = _D1SuggestionStyle[d].value.color;
		}*/

    }

    #region History
    //save in history
    public void HistoryMethod(bool carbonChanged)
    {
        Debug.Log("History Method Call");

        History _his = new History();
        _his.HeatNumber = _HeatNumber.text;
        _his.ItemName = _ItemName.options[_ItemName.value].text;
        _his.BatchName = _batchName.options[_batchName.value].text;
        _his.PouringStatus = _PouringStatus.options[_PouringStatus.value].text;
        _his.FurnaceSize = _furnaceSize.text;
        _his.FurId = _FurnaceNumber.text;
        _his.Steelvalue = _SteelValues.options[_SteelValues.value].text;
        _his.CastingGrade = _CastingGrade.text;
        _his.TappingTemp = _TappingTemp.text;
        _his.PouringTempFirst = _PouringTempFst.text;
        _his.PouringTempLast = _PouringTempLst.text;
        if (carbonChanged)
            _his.SrNo = "**";


        _his._time = System.DateTime.Now.ToString();

        //Save Metals
        for (int i = 0; i < _MatList.Count; i++)
        {
            // Debug.Log("material "+_MatList[i]._Material.text);

            Matlistsave _mlist = new Matlistsave();
            _mlist._Material = _MatList[i]._Material.text.Replace("\r\n", "").Replace("\r", "").Replace("\n", ""); ;

            if (_MatList[i]._Bgimage.color == _high)
                _mlist._value = ">" + _MatList[i]._value.text;

            else if (_MatList[i]._Bgimage.color == _low)
                _mlist._value = "<" + _MatList[i]._value.text;

            else
                _mlist._value = _MatList[i]._value.text;

            _his._matlist.Add(_mlist);
        }

        //Save suggestions
        for (int i = 0; i < Ilist.Count; i++)
        {
            Matlistsave _mlist = new Matlistsave();

            if (i == 0)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }
            else if (i == 1)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 2)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 3)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 4)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 5)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 6)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 7)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }


            else if (i == 8)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 9)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }

            else if (i == 10)
            {
                _mlist._Material = "Sug." + IMatlist[i].text + " in Kg";
                _mlist._value = Ilist[i].text;
                _his._matlist.Add(_mlist);
            }



        }

        //save in history 
        string key = System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();

        string dailykey = "DailyKey" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();

        //Debug.Log("daily key ::: " + PlayerPrefs.GetString(dailykey));

        if (PlayerPrefs.HasKey(key))
        {
            _ListHistory._History.Add(_his);
            string json = JsonUtility.ToJson(_ListHistory);
            PlayerPrefs.SetString(key, json);
        }
        else
        {
            _ListHistory = new HistoryList();
            _ListHistory._History.Add(_his);
            string json = JsonUtility.ToJson(_ListHistory);
            PlayerPrefs.SetString(key, json);
        }

        if (PlayerPrefs.HasKey(dailykey))
        {
            _DailyHistoryList._History.Add(_his);
            string json = JsonUtility.ToJson(_DailyHistoryList);
            PlayerPrefs.SetString(dailykey, json);
        }
        else
        {
            _DailyHistoryList = new HistoryList();
            _DailyHistoryList._History.Add(_his);
            string json = JsonUtility.ToJson(_DailyHistoryList);
            PlayerPrefs.SetString(dailykey, json);
        }
    }

    //Set History Panel
    public void SetHistoryMenu()
    {
        string month = (_DropMonth.value + 1).ToString();
        string year = _DropYear.options[_DropYear.value].text.ToString();

        HistoryList Templist = JsonUtility.FromJson<HistoryList>(PlayerPrefs.GetString(month + year));

        foreach (Transform child in _HistoryParent.transform)
            Destroy(child.gameObject);

        if (Templist == null || Templist._History.Count <= 0)
        {
            UIManager.Instance.ShowNotification("No record found");
            return;
        }

        for (int i = 0; i < Templist._History.Count; i++)
        {
            GameObject HisInstance = Instantiate(_HistoryChild, _HistoryParent.transform);
            HistoryPref historypref = HisInstance.GetComponent<HistoryPref>();
            historypref.OnPrefebSet(_ListHistory._History[i].SrNo, _ListHistory._History[i].ItemName, _ListHistory._History[i]._time, _ListHistory._History[i].HeatNumber, _ListHistory._History[i].BatchName, _ListHistory._History[i].Steelvalue, _ListHistory._History[i].CastingGrade, _ListHistory._History[i]._matlist);
        }
    }

    //Export History
    public void ExportHistory()
    {
        if (_ExportPath.text == "")
        {
            UIManager.Instance.ShowNotification("Please select path of folder");
            return;
        }

        UIManager.Instance.PrefWriteHistoryPath = _ExportPath.text;

        string month = (_DropMonth.value + 1).ToString();
        string year = _DropYear.options[_DropYear.value].text.ToString();


        HistoryList Templist = JsonUtility.FromJson<HistoryList>(PlayerPrefs.GetString(month + year));

        if (Templist == null || Templist._History.Count <= 0)
        {
            UIManager.Instance.ShowNotification("No Data Found");
            return;
        }

        string _ctime = "HistoryOf" + month + year;
        using (StreamWriter file = File.CreateText(_ExportPath.text + @"\" + _ctime + ".csv"))
        {
            for (int i = 0; i < Templist._History.Count; i++)
            {
                if (i == 0)
                {
                    file.Write("C Modified" + "," + "Time" + "," + "Heat Number" + "," + "Item Name" + "," + "Batch Name" + "," + "Pouring Status" + "," + "Furnace Size" + "," + "Furnace ID" + "," + "Steel Values" + "," + "Casting Grade" + ",");

                    Debug.Log("History count" + Templist._History[i]._matlist.Count);
                    for (int j = 0; j < Templist._History[i]._matlist.Count; j++)
                    {
                        file.Write(Templist._History[i]._matlist[j]._Material + ",");
                    }
                    file.Write('\n');

                }

                file.Write(Templist._History[i].SrNo + "," + Templist._History[i]._time + "," + Templist._History[i].HeatNumber + "," + Templist._History[i].ItemName + "," + Templist._History[i].BatchName + "," + Templist._History[i].PouringStatus + "," + Templist._History[i].FurnaceSize + "," + Templist._History[i].FurId + "," + Templist._History[i].Steelvalue + "," + Templist._History[i].CastingGrade + ",");

                for (int k = 0; k < Templist._History[i]._matlist.Count; k++)
                {
                    file.Write(Templist._History[i]._matlist[k]._value + ",");
                }

                file.Write('\n');
            }
        }

        UIManager.Instance.ShowNotification("History exported successfully at :" + '\n' + _ExportPath.text + @"\" + _ctime + ".csv");

    }

    //date time wise hostiry
    public void SetHistroyDateTimeWise()
    {
        int FromYear = int.Parse(_FromYear.options[_FromYear.value].text);
        int ToYear = int.Parse(_ToYear.options[_ToYear.value].text);

        Debug.Log(System.DateTime.Now.Month);

        int FromMonth = _FromMonth.value + 1;
        int ToMonth = _ToMonth.value + 1;

        int FromDay = int.Parse(_FromDay.options[_FromDay.value].text);
        int ToDay = int.Parse(_ToDay.options[_ToDay.value].text);

        if ((FromMonth + FromYear) > (ToMonth + ToYear))
        {
            UIManager.Instance.ShowNotification("Please select valid date");
            return;
        }

        if (_RandomExportPath.text == "")
        {
            UIManager.Instance.ShowNotification("Please select path of folder");
            return;
        }

        UIManager.Instance.PrefWriteRandomBackup = _RandomExportPath.text;



        string _ctime = "HistoryFrom" + FromDay + FromMonth + FromYear + "To" + ToDay + ToMonth + ToYear;
        string jsonstring;

        bool firstraw = false;
        using (StreamWriter file = File.CreateText(_RandomExportPath.text + @"\" + _ctime + ".csv"))
        {
            for (int i = FromYear; i <= ToYear; i++)
            {
                for (int j = FromMonth; j <= ToMonth; j++)
                {
                    if (j == FromMonth)
                    {
                        for (int k = FromDay; k <= 31; k++)
                        {
                            Debug.Log("key" + "DailyKey" + k.ToString() + j.ToString() + i.ToString());
                            jsonstring = PlayerPrefs.GetString("DailyKey" + k.ToString() + j.ToString() + i.ToString());
                            Debug.Log("Json string " + jsonstring);

                            if (jsonstring != "")
                            {
                                HistoryList Templist = JsonUtility.FromJson<HistoryList>(jsonstring);

                                for (int a = 0; a < Templist._History.Count; a++)
                                {
                                    if (a == 0)
                                    {
                                        file.Write("C Modified" + "," + "Time" + "," + "Heat Number" + "," + "Item Name" + "," + "Batch Name" + "," + "Pouring Status" + "," + "Furnace Size" + "," + "Furnace ID" + "," + "Steel Values" + "," + "Casting Grade" + ",");

                                        Debug.Log("History " + Templist._History[a]._matlist.Count);
                                        for (int b = 0; b < Templist._History[a]._matlist.Count; b++)
                                        {
                                            file.Write(Templist._History[a]._matlist[b]._Material + ",");
                                        }
                                        file.Write('\n');
                                        firstraw = true;
                                    }

                                    file.Write(Templist._History[a].SrNo + "," + Templist._History[a]._time + "," + Templist._History[a].HeatNumber + "," + Templist._History[a].ItemName + "," + Templist._History[a].BatchName + "," + Templist._History[a].PouringStatus + "," + Templist._History[a].FurnaceSize + "," + Templist._History[a].FurId + "," + Templist._History[a].Steelvalue + "," + Templist._History[a].CastingGrade + ",");

                                    for (int c = 0; c < Templist._History[a]._matlist.Count; c++)
                                    {
                                        file.Write(Templist._History[a]._matlist[c]._value + ",");
                                    }

                                    file.Write('\n');
                                }
                            }
                        }
                    }
                    else if (j > FromMonth && j < ToMonth)
                    {
                        for (int k = 1; k <= 31; k++)
                        {
                            Debug.Log("key :::" + PlayerPrefs.GetString("DailyKey" + k.ToString() + j.ToString() + i.ToString()));

                            jsonstring = PlayerPrefs.GetString("DailyKey" + k.ToString() + j.ToString() + i.ToString());

                            if (jsonstring != "")
                            {
                                HistoryList Templist = JsonUtility.FromJson<HistoryList>(jsonstring);

                                for (int a = 0; a < Templist._History.Count; a++)
                                {
                                    if (a == 0 && !firstraw)
                                    {
                                        file.Write("C Modified" + "," + "Time" + "," + "Heat Number" + "," + "Item Name" + "," + "Batch Name" + "," + "Pouring Status" + "," + "Furnace Size" + "," + "Furnace ID" + "," + "Steel Values" + "," + "Casting Grade" + ",");

                                        for (int b = 0; b < Templist._History[a]._matlist.Count; b++)
                                        {
                                            file.Write(Templist._History[a]._matlist[b]._Material + ",");
                                        }
                                        file.Write('\n');
                                        firstraw = true;
                                    }

                                    file.Write(Templist._History[a].SrNo + "," + Templist._History[a]._time + "," + Templist._History[a].HeatNumber + "," + Templist._History[a].ItemName + "," + Templist._History[a].BatchName + "," + Templist._History[a].PouringStatus + "," + Templist._History[a].FurnaceSize + "," + Templist._History[a].FurId + "," + Templist._History[a].Steelvalue + "," + Templist._History[a].CastingGrade + ",");

                                    for (int c = 0; c < Templist._History[a]._matlist.Count; c++)
                                    {
                                        file.Write(Templist._History[a]._matlist[c]._value + ",");
                                    }

                                    file.Write('\n');
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = 1; k <= ToDay; k++)
                        {
                            Debug.Log("key :::" + PlayerPrefs.GetString("DailyKey" + k.ToString() + j.ToString() + i.ToString()));

                            jsonstring = PlayerPrefs.GetString("DailyKey" + k.ToString() + j.ToString() + i.ToString());

                            if (jsonstring != "")
                            {
                                HistoryList Templist = JsonUtility.FromJson<HistoryList>(jsonstring);

                                for (int a = 0; a < Templist._History.Count; a++)
                                {
                                    if (a == 0 && !firstraw)
                                    {
                                        file.Write("C Modified" + "," + "Time" + "," + "Heat Number" + "," + "Item Name" + "," + "Batch Name" + "," + "Pouring Status" + "," + "Furnace Size" + "," + "Furnace ID" + "," + "Steel Values" + "," + "Casting Grade" + ",");

                                        for (int b = 0; b < Templist._History[a]._matlist.Count; b++)
                                        {
                                            file.Write(Templist._History[a]._matlist[b]._Material + ",");
                                        }
                                        file.Write('\n');
                                        firstraw = true;
                                    }

                                    file.Write(Templist._History[a].SrNo + "," + Templist._History[a]._time + "," + Templist._History[a].HeatNumber + "," + Templist._History[a].ItemName + "," + Templist._History[a].BatchName + "," + Templist._History[a].PouringStatus + "," + Templist._History[a].FurnaceSize + "," + Templist._History[a].FurId + "," + Templist._History[a].Steelvalue + "," + Templist._History[a].CastingGrade + ",");

                                    for (int c = 0; c < Templist._History[a]._matlist.Count; c++)
                                    {
                                        file.Write(Templist._History[a]._matlist[c]._value + ",");
                                    }

                                    file.Write('\n');
                                }
                            }
                        }
                    }
                }
            }
        }

        UIManager.Instance.ShowNotification("File exported successfully!");
    }

    //Daily Export
    public void DailyExport(bool changed)
    {
        string month = System.DateTime.Now.Month.ToString();
        string year = System.DateTime.Now.Year.ToString();


        HistoryList Templist = JsonUtility.FromJson<HistoryList>(PlayerPrefs.GetString(month + year));

        //Debug.Log("get string" + PlayerPrefs.GetString(month + year));

        if (Templist == null || Templist._History.Count <= 0)
        {
            UIManager.Instance.ShowNotification("No Data Found");
            return;
        }

        string _ctime = "HistoryOf_" + month + "_" + year;

        StreamWriter tempfile;


        try
        {
            tempfile = File.CreateText(UIManager.Instance.PrefWiteBackuppath + @"\" + _ctime + ".csv");
        }
        catch
        {
            UIManager.Instance.ShowNotification("Some error occured!, History not updated");
            return;
        }

        using (StreamWriter file = tempfile)
        {

            for (int i = 0; i < Templist._History.Count; i++)
            {
                if (i == 0)
                {
                    file.Write("C Modified" + "," + "Time" + "," + "Heat Number" + "," + "Item Name" + "," + "Batch Name" + "," + "Pouring Status" + "," + "Furnace Size" + "," + "Furnace ID" + "," + "Steel Values" + "," + "Casting Grade" + ",");

                    Debug.Log("first " + Templist._History[i]._matlist.Count);
                    for (int j = 0; j < Templist._History[i]._matlist.Count; j++)
                    {
                        file.Write(Templist._History[i]._matlist[j]._Material + ",");
                    }
                    file.Write('\n');

                }

                file.Write(Templist._History[i].SrNo + "," + Templist._History[i]._time + "," + Templist._History[i].HeatNumber + "," + Templist._History[i].ItemName + "," + Templist._History[i].BatchName + "," + Templist._History[i].PouringStatus + "," + Templist._History[i].FurnaceSize + "," + Templist._History[i].FurId + "," + Templist._History[i].Steelvalue + "," + Templist._History[i].CastingGrade + ",");

                for (int k = 0; k < Templist._History[i]._matlist.Count; k++)
                {
                    file.Write(Templist._History[i]._matlist[k]._value + ",");
                }

                file.Write('\n');
            }
        }

        //UIManager.Instance.ShowNotification("History exported successfully at :" + '\n' + _ExportPath.text + @"\" + _ctime + ".csv");
    }

    #endregion

    //Set HEader
    public void SetHeader(string Hname, string IName, string FNumber, string PorName, string GCast)
    {
        _DHeatNum.text = Hname;

        if (IName != "Item Not Found")
            _DItemName.text = IName.Substring(0, IName.Length - 2);
        else
            _DItemName.text = IName;

        _DFurnaceNumber.text = FNumber;
        _DGradeofcast.text = GCast;
    }

    // as drop down of item changed
    public void OnItemSelected(bool carbonChange)
    {

        string CIName = _ItemName.options[_ItemName.value].text;
        Debug.Log("Current Item Name ::" + CIName);
        string CBName = _batchName.options[_batchName.value].text;
        string CSteelName = _SteelValues.options[_SteelValues.value].text;

        ItemList _CIlist = _ItemManager._currentitemlist;
        ListBatchList _CBatchList = _BatchManager._currenBatchesList;
        ListBatchList _CSteelList = _BatchManager._currentSteelList;

        int CValueRange = 0; // Ok = 0, down = 1, up = 2
        int SiValueRange = 0;
        int Cnumber = 0;
        int SiNumber = 0;
        int CrNumber = 0;
        int SNumber = 0;
        int MnNumber = 0;
        int PNmber = 0;
        int CuNumber = 0;
        int Ninumber = 0;
        int Snnumber = 0;
        int Monumber = 0;
        int MgNumber = 0;
        int AlNumber = 0;
        int CoNumber = 0;
        int TiNumber = 0;
        int VNumber = 0;
        int PbNumber = 0;
        int WNumber = 0;

        int matchcount = 0;

        float CarbonHigherRange = 0; float CarbonLowerRange = 0; float SiHigherRange = 0; float SiLowerRange = 0;


        foreach (ItemDetail _detail in _CIlist._ItemDetail)
        {

            if (_detail.Itemname == CIName)
            {
                matchcount = 1;
                _CItemDetail = _detail;
                for (int i = 0; i < _MatList.Count; i++)
                {
                    foreach (ItemData data in _CItemDetail._itemdatalist)
                    {
                        string temp = _MatList[i]._Material.text;
                        string temp2 = data.MName;
                        //Debug.Log("termp" + _MatList[i]._Material.text + "TEMP2" + temp2);


                        if (temp == temp2 || (temp2 == "Ni" && i == 7))
                        {
                            Debug.Log("temp2:" + temp2 + " " + i);
                            if (temp2 == "Ni")
                            {
                                Debug.Log("Min value" + data.Min.ToString());
                            }

                            _MatList[i]._Min.text = data.Min.ToString();
                            _MatList[i]._Max.text = data.Max.ToString();

                            _MatList[i]._Avg.text = ((data.Min + data.Max) / 2).ToString();

                            float dif = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                            float com = float.Parse(_MatList[i]._value.text);

                            if (!Mathf.Approximately(com, dif) && com < dif)
                            {
                                _MatList[i]._Bgimage.color = _low;
                                _MatList[i]._value.color = Color.white;
                                if (_MatList[i]._Material.text == "C")
                                {
                                    CarbonLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    CarbonHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    CValueRange = 1;
                                    Cnumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Si")
                                {
                                    SiLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    SiHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    SiValueRange = 1;
                                    SiNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Cr")
                                {
                                    CrNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Mn")
                                {
                                    MnNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "P")
                                {
                                    PNmber = i;
                                }

                                else if (_MatList[i]._Material.text == "S")
                                {
                                    SNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Cu")
                                {
                                    CuNumber = i;

                                }
                                else if (_MatList[i]._Material.text == "Ni")
                                {
                                    Ninumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Sn")
                                    Snnumber = i;

                                else if (_MatList[i]._Material.text == "Mo")
                                {
                                    Monumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Mg")
                                {
                                    MgNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Al")
                                {
                                    AlNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Co")
                                {
                                    CoNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Ti")
                                {
                                    TiNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "V")
                                {
                                    VNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Pb")
                                {
                                    PbNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "W")
                                {
                                    WNumber = i;
                                }


                            }

                            else if (float.Parse(_MatList[i]._value.text) > (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text)))
                            {
                                _MatList[i]._Bgimage.color = _high;
                                _MatList[i]._value.color = Color.white;
                                if (_MatList[i]._Material.text == "C")
                                {
                                    CarbonLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    CarbonHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    CValueRange = 2;
                                    Cnumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Si")
                                {
                                    SiLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    SiHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    SiValueRange = 2;
                                    SiNumber = i;
                                }


                                else if (_MatList[i]._Material.text == "Cr")
                                {
                                    CrNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Mn")
                                {
                                    MnNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "P")
                                {
                                    PNmber = i;
                                }

                                else if (_MatList[i]._Material.text == "S")
                                {
                                    SNumber = i;
                                }


                                else if (_MatList[i]._Material.text == "Cu")
                                {

                                    CuNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Ni")
                                {
                                    Ninumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Sn")
                                {
                                    Snnumber = i;

                                }
                                else if (_MatList[i]._Material.text == "Mo")
                                {
                                    Monumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Mg")
                                {
                                    MgNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Al")
                                {
                                    AlNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Co")
                                {
                                    CoNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Ti")
                                {
                                    TiNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "V")
                                {
                                    VNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Pb")
                                {
                                    PbNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "W")
                                {
                                    WNumber = i;
                                }


                            }

                            else
                            {
                                _MatList[i]._Bgimage.color = Color.green;
                                _MatList[i]._value.color = Color.black;
                                if (_MatList[i]._Material.text == "C")
                                {
                                    CarbonLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    CarbonHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    CValueRange = 0;
                                    Cnumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Si")
                                {
                                    SiLowerRange = (float.Parse(_MatList[i]._Min.text) - float.Parse(_MatList[i]._Max.text));
                                    SiHigherRange = (float.Parse(_MatList[i]._Min.text) + float.Parse(_MatList[i]._Max.text));
                                    SiNumber = i;
                                    SiValueRange = 0;
                                }

                                else if (_MatList[i]._Material.text == "Cr")
                                {
                                    CrNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Mn")
                                {
                                    MnNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "P")
                                {
                                    PNmber = i;
                                }

                                else if (_MatList[i]._Material.text == "S")
                                {
                                    SNumber = i;
                                }


                                else if (_MatList[i]._Material.text == "Cu")
                                {
                                    Debug.Log("CCCCCCCCCCCCCCu 3");
                                    CuNumber = i;
                                }

                                else if (_MatList[i]._Material.text == "Ni")
                                {
                                    Ninumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Sn")
                                {
                                    Snnumber = i;

                                }
                                else if (_MatList[i]._Material.text == "Mo")
                                {
                                    Monumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Mg")
                                {
                                    MgNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Al")
                                {
                                    AlNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Co")
                                {
                                    CoNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Ti")
                                {
                                    TiNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "V")
                                {
                                    VNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "Pb")
                                {
                                    PbNumber = i;
                                }
                                else if (_MatList[i]._Material.text == "W")
                                {
                                    WNumber = i;
                                }

                            }

                        }
                    }
                }
            }

            else if (matchcount == 0)
            {

                //Debug.Log("Not same ::");

                for (int i = 0; i < _MatList.Count; i++)
                {
                    foreach (ItemData data in _CItemDetail._itemdatalist)
                    {
                        if (data.MName == _MatList[i]._Material.text)
                        {
                            _MatList[i]._Bgimage.color = Color.green;
                            _MatList[i]._value.color = Color.black;

                            _MatList[i]._Min.text = "";
                            _MatList[i]._Max.text = "";
                        }
                    }
                }
            }
        }


        float BC = 0;
        float BSi = 0;
        float BCr = 0, BMn = 0, BP = 0, BS = 0, BCu = 0, BNi = 0, BSn = 0, BMo = 0, BMg = 0, BAl = 0, BCo = 0, BTi = 0, BV = 0, BPb = 0, BW=0;
        float SCr = 0;
        float SMn = 0, SP = 0, SS = 0, SCu = 0, SNi = 0, SSn = 0, SMo = 0, SMg = 0;


        foreach (BatchesList list in _CBatchList._list)
        {
            if (list.batchName == CBName)
            {
                Debug.Log("in thissssssssssssssssssssssssssssss");
                _CBatchDetail = list;

                foreach (Batchdetail data in _CBatchDetail._batchdetail)
                {
                    if (data._matname == "C" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BC = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Si" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BSi = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Cr" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BCr = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Mn" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BMn = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Cu" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BCu = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "P" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BP = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "S" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BS = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Ni" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BNi = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Sn" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BSn = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Mo" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BMo = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Al" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BAl = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Co" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BCo = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Ti" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BTi = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "V" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BV = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "Pb" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BPb = float.Parse(data._batchvalue);
                    }
                    if (data._matname == "W" && data._batchvalue != "" && data._batchvalue != null)
                    {
                        BW = float.Parse(data._batchvalue);
                    }


                }
            }
        }

        Debug.Log("Carbon condition " + CValueRange);
        Debug.Log("Silicon condition " + SiValueRange);


        if (CBName == "Select Batch")
        {
            for (int i = 0; i < Ilist.Count; i++)
            {
                Ilist[i].text = "";
            }

            for (int i = 0; i < Alist.Count; i++)
            {
                Alist[i].text = "";
            }


            /*for (int i = 0; i < _MatList.Count; i++)
			{
				_MatList[i]._Bgimage.color = Color.white;
				_MatList[i]._value.color = Color.black;
			}*/
            //Debug.Log("1");
            //OnDisplay();
            return;

        }

        Debug.Log("min texttt..." + _MatList[SiNumber]._Min.text);
        float FurSize = float.Parse(_furnaceSize.text);
        Debug.Log("before exception");
        float RequireSi = float.Parse(_MatList[SiNumber]._Min.text);
        Debug.Log("after exception");
        float RequireC = float.Parse(_MatList[Cnumber]._Min.text);

        float ActualC = float.Parse(_MatList[Cnumber]._value.text);
        float ActualSi = float.Parse(_MatList[SiNumber]._value.text);

        float ActualCr = float.Parse(_MatList[CrNumber]._value.text);
        float ActualMn = float.Parse(_MatList[MnNumber]._value.text);
        float ActualP = float.Parse(_MatList[PNmber]._value.text);
        float ActualS = float.Parse(_MatList[SNumber]._value.text);
        float ActualCu = float.Parse(_MatList[CuNumber]._value.text);
        float ActualNi = float.Parse(_MatList[Ninumber]._value.text);
        float ActualSn = float.Parse(_MatList[Snnumber]._value.text);
        float ActualMo = float.Parse(_MatList[Monumber]._value.text);
        float ActualMg = float.Parse(_MatList[MgNumber]._value.text);
        float ActualAl = float.Parse(_MatList[AlNumber]._value.text);
        float ActualCo = float.Parse(_MatList[CoNumber]._value.text);
        float ActualTi = float.Parse(_MatList[TiNumber]._value.text);
        float ActualV = float.Parse(_MatList[VNumber]._value.text);
        float ActualPb = float.Parse(_MatList[PbNumber]._value.text);
        float ActualW = float.Parse(_MatList[WNumber]._value.text);




        float RequireCr = float.Parse(_MatList[CrNumber]._Min.text);
        float RequireMn = float.Parse(_MatList[MnNumber]._Min.text);
        float RequireP = float.Parse(_MatList[PNmber]._Min.text);
        float RequireS = float.Parse(_MatList[SNumber]._Min.text);
        float RequireCu = float.Parse(_MatList[CuNumber]._Min.text);
        float RequireNi = float.Parse(_MatList[Ninumber]._Min.text);
        float RequireSn = float.Parse(_MatList[Snnumber]._Min.text);
        float RequireMo = float.Parse(_MatList[Monumber]._Min.text);
        float RequireMg = float.Parse(_MatList[MgNumber]._Min.text);
        float RequireAl = float.Parse(_MatList[AlNumber]._Min.text);
        float RequireCo = float.Parse(_MatList[CoNumber]._Min.text);
        float RequireTi = float.Parse(_MatList[TiNumber]._Min.text);
        float RequireV = float.Parse(_MatList[VNumber]._Min.text);
        float RequirePb = float.Parse(_MatList[PbNumber]._Min.text);
        float RequireW = float.Parse(_MatList[WNumber]._Min.text);


        float TolCr = float.Parse(_MatList[CrNumber]._Max.text);
        float TolMn = float.Parse(_MatList[MnNumber]._Max.text);
        float TolP = float.Parse(_MatList[PNmber]._Max.text);
        float TolS = float.Parse(_MatList[SNumber]._Max.text);
        float TolCu = float.Parse(_MatList[CuNumber]._Max.text);
        float TolNi = float.Parse(_MatList[Ninumber]._Max.text);
        float TolSn = float.Parse(_MatList[Snnumber]._Max.text);
        float TolMo = float.Parse(_MatList[Monumber]._Max.text);
        float TolMg = float.Parse(_MatList[MgNumber]._Max.text);
        float TolAl = float.Parse(_MatList[AlNumber]._Max.text);
        float TolCo = float.Parse(_MatList[CoNumber]._Max.text);
        float TolTi = float.Parse(_MatList[TiNumber]._Max.text);
        float TolV = float.Parse(_MatList[VNumber]._Max.text);
        float TolPb = float.Parse(_MatList[PbNumber]._Max.text);
        float TolW = float.Parse(_MatList[WNumber]._Max.text);


        Debug.Log("test value");

        //Ilist[5].text = OtherElementSuggest(RequireMo, ActualMo, BMo, FurSize, TolMo).ToString("F2");
        Ilist[2].text = OtherElementSuggest(RequireMn, ActualMn, BMn, FurSize, TolMn).ToString("F3");
        Ilist[3].text = OtherElementSuggest(RequireP, ActualP, BP, FurSize, TolP).ToString("F3");

        Ilist[4].text = OtherElementSuggest(RequireS, ActualS, BS, FurSize, TolS).ToString("F3");
        Ilist[5].text = OtherElementSuggest(RequireCr, ActualCr, BCr, FurSize, TolCr).ToString("F3");
        Ilist[6].text = OtherElementSuggest(RequireMo, ActualMo, BMo, FurSize, TolMo).ToString("F3");
        Ilist[7].text = OtherElementSuggest(RequireNi, ActualNi, BNi, FurSize, TolNi).ToString("F2");

        Ilist[8].text = OtherElementSuggest(RequireCu, ActualCu, BCu, FurSize, TolCu).ToString("F2");
        Ilist[9].text = OtherElementSuggest(RequireMg, ActualMg, BMg, FurSize, TolMg).ToString("F2");
        Ilist[10].text = OtherElementSuggest(RequireAl, ActualAl, BAl, FurSize, TolAl).ToString("F2");
        Ilist[11].text = OtherElementSuggest(RequireSn, ActualSn, BSn, FurSize, TolSn).ToString("F2");


        Ilist[12].text = OtherElementSuggest(RequireTi, ActualTi, BTi, FurSize, TolTi).ToString("F2");
        Ilist[13].text = OtherElementSuggest(RequireW, ActualW, BW, FurSize, TolW).ToString("F2");

        //Ilist[14].text = OtherElementSuggest(RequireSn, ActualSn, BSn, FurSize, TolSn).ToString("F2");
        //Ilist[15].text = OtherElementSuggest(RequireMg, ActualMg, BMg, FurSize, TolMg).ToString("F2");


        // Carbon within range or lower Silicon higer
        if (CValueRange != 2 && SiValueRange == 2)
        {

            float FeAddition = ((ActualSi - RequireSi) / ActualSi) * FurSize;

            Ilist[Ilist.Count - 1].text = FeAddition.ToString("F3");

            float CarbonReduce = (FeAddition / FurSize) * ActualC;

            float CarbonAfterAdd = ActualC - CarbonReduce;

            Debug.Log("higher range " + CarbonHigherRange + " lower range " + CarbonLowerRange);
            //if carbonafteradd is within range , No calculation further 
            if (CarbonAfterAdd < CarbonHigherRange && CarbonAfterAdd > CarbonLowerRange)
            {

                Ilist[0].text = "0.00";

                Ilist[1].text = "0.00";
            }
            else
            {
                float ActualCAfterAddition = float.Parse(_MatList[Cnumber]._Min.text) - CarbonAfterAdd;

                float CarbonAdd = (ActualCAfterAddition / BC) * FurSize;

                Ilist[0].text = CarbonAdd.ToString("F3");

                Ilist[1].text = "0.00";
            }




            foreach (BatchesList list in _CSteelList._list)
            {
                Debug.Log("Steel name ::" + list.batchName);
                if (list.batchName == CSteelName)
                {
                    _SteelList = list;

                    foreach (Batchdetail data in _SteelList._batchdetail)
                    {
                        if (data._matname == "Cr" && data._batchvalue != "" && data._batchvalue != null)
                            SCr = float.Parse(data._batchvalue);

                        else if (data._matname == "Mn" && data._batchvalue != "" && data._batchvalue != null)
                            SMn = float.Parse(data._batchvalue);

                        else if (data._matname == "P" && data._batchvalue != "" && data._batchvalue != null)
                            SP = float.Parse(data._batchvalue);

                        else if (data._matname == "S" && data._batchvalue != "" && data._batchvalue != null)
                            SS = float.Parse(data._batchvalue);

                        else if (data._matname == "Cu" && data._batchvalue != "" && data._batchvalue != null)
                            SCu = float.Parse(data._batchvalue);

                        else if (data._matname == "Ni" && data._batchvalue != "" && data._batchvalue != null)
                            SNi = float.Parse(data._batchvalue);

                        else if (data._matname == "Sn" && data._batchvalue != "" && data._batchvalue != null)
                            SSn = float.Parse(data._batchvalue);

                        else if (data._matname == "Mo" && data._batchvalue != "" && data._batchvalue != null)
                            SMo = float.Parse(data._batchvalue);
                    }
                }
            }

            if (CSteelName == "Select Steel Values")
            {
                AllSuggestionBecomeNull();
                return;
            }


            Debug.Log("Scr value ::" + SCr);
            float CrIncreased = (FeAddition * SCr) / FurSize;
            float MnIncreased = (FeAddition * SMn) / FurSize;
            float PIncreased = (FeAddition * SP) / FurSize;
            float SIncreased = (FeAddition * SS) / FurSize;
            float CuIncreased = (FeAddition * SCu) / FurSize;
            float NiIncreased = (FeAddition * SNi) / FurSize;
            float SnIncreased = (FeAddition * SSn) / FurSize;
            float MoIncreased = (FeAddition * SMo) / FurSize;


            float CrF = ActualCr + CrIncreased;
            Alist[1].text = CrF.ToString("F2");

            float Mnf = ActualMn + MnIncreased;
            Alist[3].text = Mnf.ToString("F2");

            float Pf = ActualP + PIncreased;
            Alist[4].text = Pf.ToString("F2");

            //float Sf = ActualS + SIncreased;
            //Alist[0].text = Sf.ToString("F2");

            float Cuf = ActualCu + CuIncreased;
            Alist[2].text = Cuf.ToString("F2");

            //float Nif = ActualNi + NiIncreased;
            //Alist[4].text = Nif.ToString("F2");

            //float Snf = ActualSn + SnIncreased;
            //Alist[0].text = Snf.ToString("F2");

            float Mof = ActualMo + MoIncreased;
            Alist[0].text = Mof.ToString("F2");



            //for setting colors in suggestions


        }


        // Carbon higher Silicon range or lower
        else if (CValueRange == 2 && SiValueRange != 2)
        {

            float FeAddition = ((ActualC - RequireC) / ActualC) * FurSize;

            Ilist[Ilist.Count - 1].text = FeAddition.ToString("F3");

            float SiReduce = (FeAddition / FurSize) * ActualSi;

            float SiAfterAdd = ActualSi - SiReduce;

            if (SiAfterAdd < SiHigherRange && SiAfterAdd > SiLowerRange)
            {
                Ilist[1].text = "0.00";

                Ilist[0].text = "0.00";
            }
            else
            {
                float actualSiAddition = float.Parse(_MatList[SiNumber]._Min.text) - SiAfterAdd;

                float Siaddition = (actualSiAddition / BSi) * FurSize;

                Ilist[1].text = Siaddition.ToString("F3");

                Ilist[0].text = "0";
            }



            foreach (BatchesList list in _CSteelList._list)
            {
                Debug.Log("Steel name ::" + list.batchName);
                if (list.batchName == CSteelName)
                {
                    _SteelList = list;

                    foreach (Batchdetail data in _SteelList._batchdetail)
                    {
                        if (data._matname == "Cr" && data._batchvalue != "" && data._batchvalue != null)
                            SCr = float.Parse(data._batchvalue);

                        else if (data._matname == "Mn" && data._batchvalue != "" && data._batchvalue != null)
                            SMn = float.Parse(data._batchvalue);

                        else if (data._matname == "P" && data._batchvalue != "" && data._batchvalue != null)
                            SP = float.Parse(data._batchvalue);

                        else if (data._matname == "S" && data._batchvalue != "" && data._batchvalue != null)
                            SS = float.Parse(data._batchvalue);

                        else if (data._matname == "Cu" && data._batchvalue != "" && data._batchvalue != null)
                            SCu = float.Parse(data._batchvalue);

                        else if (data._matname == "Ni" && data._batchvalue != "" && data._batchvalue != null)
                            SNi = float.Parse(data._batchvalue);

                        else if (data._matname == "Sn" && data._batchvalue != "" && data._batchvalue != null)
                            SSn = float.Parse(data._batchvalue);

                        else if (data._matname == "Mo" && data._batchvalue != "" && data._batchvalue != null)
                            SMo = float.Parse(data._batchvalue);
                    }
                }
            }

            if (CSteelName == "Select Steel Values")
            {
                AllSuggestionBecomeNull();
                return;
            }

            float CrIncreased = (FeAddition * SCr) / FurSize;
            float MnIncreased = (FeAddition * SMn) / FurSize;
            float PIncreased = (FeAddition * SP) / FurSize;
            float SIncreased = (FeAddition * SS) / FurSize;
            float CuIncreased = (FeAddition * SCu) / FurSize;
            float NiIncreased = (FeAddition * SNi) / FurSize;
            float SnIncreased = (FeAddition * SSn) / FurSize;
            float MoIncreased = (FeAddition * SMo) / FurSize;

            float CrF = ActualCr + CrIncreased;
            Alist[1].text = CrF.ToString("F2");

            float Mnf = ActualMn + MnIncreased;
            Alist[3].text = Mnf.ToString("F2");

            float Pf = ActualP + PIncreased;
            Alist[4].text = Pf.ToString("F2");

            //float Sf = ActualS + SIncreased;
            //Alist[0].text = Sf.ToString("F2");

            float Cuf = ActualCu + CuIncreased;
            Alist[2].text = Cuf.ToString("F2");

            //float Nif = ActualNi + NiIncreased;
            //Alist[4].text = Nif.ToString("F2");

            //float Snf = ActualSn + SnIncreased;
            //Alist[0].text = Snf.ToString("F2");

            float Mof = ActualMo + MoIncreased;
            Alist[0].text = Mof.ToString("F2");
        }


        //carbon higher silicon higher
        else if (CValueRange == 2 && SiValueRange == 2)
        {

            float carbonPerHigh = (((ActualC * 100) / RequireC) - 100);
            float SiPerHigh = (((ActualSi * 100) / RequireSi) - 100);

            if (SiPerHigh > carbonPerHigh)
            {
                float FeAddition = ((ActualSi - RequireSi) / ActualSi) * FurSize;

                Ilist[Ilist.Count - 1].text = FeAddition.ToString("F3");

                float CarbonReduce = (FeAddition / FurSize) * ActualC;

                float CarbonAfterAdd = ActualC - CarbonReduce;

                if (CarbonAfterAdd < CarbonHigherRange && CarbonAfterAdd > CarbonLowerRange)
                {
                    Ilist[0].text = "0.00";
                    Ilist[1].text = "0.00";
                }
                else
                {
                    float ActualCAfterAddition = float.Parse(_MatList[Cnumber]._Min.text) - CarbonAfterAdd;

                    float CarbonAdd = (ActualCAfterAddition / BC) * FurSize;

                    Ilist[0].text = CarbonAdd.ToString("F3");
                    Ilist[1].text = "0.00";
                }

                foreach (BatchesList list in _CSteelList._list)
                {
                    Debug.Log("Steel name ::" + list.batchName);
                    if (list.batchName == CSteelName)
                    {
                        _SteelList = list;

                        foreach (Batchdetail data in _SteelList._batchdetail)
                        {
                            if (data._matname == "Cr" && data._batchvalue != "" && data._batchvalue != null)
                                SCr = float.Parse(data._batchvalue);

                            else if (data._matname == "Mn" && data._batchvalue != "" && data._batchvalue != null)
                                SMn = float.Parse(data._batchvalue);

                            else if (data._matname == "P" && data._batchvalue != "" && data._batchvalue != null)
                                SP = float.Parse(data._batchvalue);

                            else if (data._matname == "S" && data._batchvalue != "" && data._batchvalue != null)
                                SS = float.Parse(data._batchvalue);

                            else if (data._matname == "Cu" && data._batchvalue != "" && data._batchvalue != null)
                                SCu = float.Parse(data._batchvalue);

                            else if (data._matname == "Ni" && data._batchvalue != "" && data._batchvalue != null)
                                SNi = float.Parse(data._batchvalue);

                            else if (data._matname == "Sn" && data._batchvalue != "" && data._batchvalue != null)
                                SSn = float.Parse(data._batchvalue);

                            else if (data._matname == "Mo" && data._batchvalue != "" && data._batchvalue != null)
                                SMo = float.Parse(data._batchvalue);
                        }
                    }
                }

                if (CSteelName == "Select Steel Values")
                {
                    AllSuggestionBecomeNull();
                    return;
                }

                float CrIncreased = (FeAddition * SCr) / FurSize;
                float MnIncreased = (FeAddition * SMn) / FurSize;
                float PIncreased = (FeAddition * SP) / FurSize;
                float SIncreased = (FeAddition * SS) / FurSize;
                float CuIncreased = (FeAddition * SCu) / FurSize;
                float NiIncreased = (FeAddition * SNi) / FurSize;
                float SnIncreased = (FeAddition * SSn) / FurSize;
                float MoIncreased = (FeAddition * SMo) / FurSize;


                float CrF = ActualCr + CrIncreased;
                Alist[1].text = CrF.ToString("F2");

                float Mnf = ActualMn + MnIncreased;
                Alist[3].text = Mnf.ToString("F2");

                float Pf = ActualP + PIncreased;
                Alist[4].text = Pf.ToString("F2");

                //float Sf = ActualS + SIncreased;
                //Alist[0].text = Sf.ToString("F2");

                float Cuf = ActualCu + CuIncreased;
                Alist[2].text = Cuf.ToString("F2");

                //float Nif = ActualNi + NiIncreased;
                //Alist[4].text = Nif.ToString("F2");

                //float Snf = ActualSn + SnIncreased;
                //Alist[0].text = Snf.ToString("F2");

                float Mof = ActualMo + MoIncreased;
                Alist[0].text = Mof.ToString("F2");

            }

            else if (SiPerHigh < carbonPerHigh)
            {
                float FeAddition = ((ActualC - RequireC) / ActualC) * FurSize;

                Ilist[Ilist.Count - 1].text = FeAddition.ToString("F3");

                float SiReduce = (FeAddition / FurSize) * ActualSi;

                float SiAfterAdd = ActualSi - SiReduce;

                if (SiAfterAdd < SiHigherRange && SiAfterAdd > SiHigherRange)
                {
                    Ilist[1].text = "0.00";
                    Ilist[0].text = "0.00";
                }
                else
                {
                    float actualSiAddition = float.Parse(_MatList[SiNumber]._Min.text) - SiAfterAdd;

                    float Siaddition = (actualSiAddition / BSi) * FurSize;

                    Ilist[1].text = Siaddition.ToString("F3");
                    Ilist[0].text = "0.00";
                }




                foreach (BatchesList list in _CSteelList._list)
                {
                    Debug.Log("Steel name ::" + list.batchName);
                    if (list.batchName == CSteelName)
                    {
                        _SteelList = list;

                        foreach (Batchdetail data in _SteelList._batchdetail)
                        {
                            if (data._matname == "Cr" && data._batchvalue != "" && data._batchvalue != null)
                                SCr = float.Parse(data._batchvalue);

                            else if (data._matname == "Mn" && data._batchvalue != "" && data._batchvalue != null)
                                SMn = float.Parse(data._batchvalue);

                            else if (data._matname == "P" && data._batchvalue != "" && data._batchvalue != null)
                                SP = float.Parse(data._batchvalue);

                            else if (data._matname == "S" && data._batchvalue != "" && data._batchvalue != null)
                                SS = float.Parse(data._batchvalue);

                            else if (data._matname == "Cu" && data._batchvalue != "" && data._batchvalue != null)
                                SCu = float.Parse(data._batchvalue);

                            else if (data._matname == "Ni" && data._batchvalue != "" && data._batchvalue != null)
                                SNi = float.Parse(data._batchvalue);

                            else if (data._matname == "Sn" && data._batchvalue != "" && data._batchvalue != null)
                                SSn = float.Parse(data._batchvalue);

                            else if (data._matname == "Mo" && data._batchvalue != "" && data._batchvalue != null)
                                SMo = float.Parse(data._batchvalue);
                        }
                    }
                }

                if (CSteelName == "Select Steel Values")
                {

                    AllSuggestionBecomeNull();
                    return;
                }

                float CrIncreased = (FeAddition * SCr) / FurSize;
                float MnIncreased = (FeAddition * SMn) / FurSize;
                float PIncreased = (FeAddition * SP) / FurSize;
                float SIncreased = (FeAddition * SS) / FurSize;
                float CuIncreased = (FeAddition * SCu) / FurSize;
                float NiIncreased = (FeAddition * SNi) / FurSize;
                float SnIncreased = (FeAddition * SSn) / FurSize;
                float MoIncreased = (FeAddition * SMo) / FurSize;

                float CrF = ActualCr + CrIncreased;
                Alist[1].text = CrF.ToString("F2");

                float Mnf = ActualMn + MnIncreased;
                Alist[3].text = Mnf.ToString("F2");

                float Pf = ActualP + PIncreased;
                Alist[4].text = Pf.ToString("F2");

                //float Sf = ActualS + SIncreased;
                //Alist[0].text = Sf.ToString("F2");

                float Cuf = ActualCu + CuIncreased;
                Alist[2].text = Cuf.ToString("F2");

                //float Nif = ActualNi + NiIncreased;
                //Alist[4].text = Nif.ToString("F2");

                //float Snf = ActualSn + SnIncreased;
                //Alist[0].text = Snf.ToString("F2");

                float Mof = ActualMo + MoIncreased;
                Alist[0].text = Mof.ToString("F2");


            }
        }


        //Carbon lower silicon lower
        else if (CValueRange == 1 && SiValueRange == 1)
        {

            Debug.Log("Both lower");

            float CarDif = RequireC - ActualC;
            float SiDif = RequireSi - ActualSi;

            float carbonPerLower = ((CarDif / BC) * FurSize);
            float SiPerLower = ((SiDif / BSi) * FurSize);

            Ilist[0].text = carbonPerLower.ToString("F3");
            Ilist[1].text = SiPerLower.ToString("F3");

            Ilist[Ilist.Count - 1].text = "0.00";

            SetAlistBlank();

        }

        //Carbon range silicon lower
        else if (CValueRange == 0 && SiValueRange == 1)
        {

            float SiDif = RequireSi - ActualSi;
            float SiPerLower = ((SiDif / BSi) * FurSize);
            Ilist[1].text = SiPerLower.ToString("F3");

            Ilist[0].text = "0.00";
            Ilist[Ilist.Count - 1].text = "0.00";
            SetAlistBlank();
        }


        //Carbon lower silicon range
        else if (CValueRange == 1 && SiValueRange == 0)
        {

            float CarDif = RequireC - ActualC;
            float carbonPerLower = ((CarDif / BC) * FurSize);
            Ilist[0].text = carbonPerLower.ToString("F3");

            Ilist[1].text = "0.00";
            Ilist[Ilist.Count - 1].text = "0.00";
            SetAlistBlank();
        }

        else
        {
            Ilist[1].text = "0.00";
            Ilist[0].text = "0.00";
            Ilist[Ilist.Count - 1].text = "0.00";
            SetAlistBlank();

            /*
			for(int i = 0; i < _D1SuggestionStyle.Count; i++)
            {
				_D1SuggestionStyle[i].BG.color = Color.white;
				_D1SuggestionStyle[i].value.color = Color.black;
            }*/
        }
        //Debug.Log("2");
        //OnDisplay();
    }

    private void AllSuggestionBecomeNull()
    {
        SetAlistBlank();
        //Debug.Log("3");
        //OnDisplay();
    }

    public void SetAlistBlank()
    {
        for (int i = 0; i < Alist.Count; i++)
        {
            Alist[i].text = "";
        }
    }

    private void ColourSuggestion()
    {
        #region Colors
        /*
		if (Mnf >= float.Parse(_MatList[MnNumber]._Max.text))
		{
			_D1SuggestionStyle[0].BG.color = _high;
			_D1SuggestionStyle[0].value.color = Color.white;
		}
		else
		{
			_D1SuggestionStyle[0].BG.color = Color.white;
			_D1SuggestionStyle[0].value.color = Color.black;
		}

		if (Pf >= float.Parse(_MatList[PNmber]._Max.text))
		{
			_D1SuggestionStyle[1].BG.color = _high;
			_D1SuggestionStyle[1].value.color = Color.white;
		}
		else
		{
			_D1SuggestionStyle[1].BG.color = Color.white;
			_D1SuggestionStyle[1].value.color = Color.black;
		}

		if (Sf >= float.Parse(_MatList[SNumber]._Max.text))
		{
			_D1SuggestionStyle[2].BG.color = _high;
			_D1SuggestionStyle[2].value.color = Color.white;
		}
		else
		{
			_D1SuggestionStyle[2].BG.color = Color.white;
			_D1SuggestionStyle[2].value.color = Color.black;
		}

		if (CrF >= float.Parse(_MatList[CrNumber]._Max.text))
		{
			_D1SuggestionStyle[3].BG.color = _high;
			_D1SuggestionStyle[3].value.color = Color.white;
		}
		else
		{
			_D1SuggestionStyle[3].BG.color = Color.white;
			_D1SuggestionStyle[3].value.color = Color.black;
		}

		if (Cuf >= float.Parse(_MatList[CuNumber]._Max.text))
		{
			_D1SuggestionStyle[4].BG.color = _high;
			_D1SuggestionStyle[4].value.color = Color.white;
		}
		else
		{
			_D1SuggestionStyle[4].BG.color = Color.white;
			_D1SuggestionStyle[4].value.color = Color.black;
		}*/
        #endregion
    }

    public float OtherElementSuggest(float RequireMat, float ActualMat, float BValue, float FurSize, float tol)
    {
        if (ActualMat < RequireMat - tol)
        {
            float DifValue = RequireMat - ActualMat;
            float MatValue = ((DifValue / BValue) * FurSize);
            return MatValue;
        }
        else
        {
            return 0;
        }

    }

    public void OnBatchSelected()
    {
        //Debug.Log("item valuesssssss " + _BatchDrop.options[_BatchDrop.value].text);
    }

}

[Serializable]
class rawList
{
    public List<string> _stringList = new List<string>();
}

[Serializable]
public class MaterialList
{
    public TMP_Text _Material, _value, _Min, _Max, _Avg;
    public Image _Bgimage;
}

[Serializable]
public class HistoryList
{
    public List<History> _History = new List<History>();
}

[Serializable]
public class History
{
    public string SrNo, _time, HeatNumber, ItemName, BatchName, PouringStatus, FurnaceSize, FurId, Steelvalue, CastingGrade, TappingTemp, PouringTempFirst, PouringTempLast;
    public List<Matlistsave> _matlist = new List<Matlistsave>();
}

[Serializable]
public class Matlistsave
{
    public string _Material, _value;
}

[Serializable]
public class SuggestionStyle
{
    public Image BG;
    public TMP_Text value;
}
