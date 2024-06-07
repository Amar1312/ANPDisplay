using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net.Http;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance = null;
	public NotificationController _NotificationController, _NotificationController2;
	public List<GameObject> _screens;

	[Space]
	[Header("PasswordforremovingErrorscreen")]
	public string _errorpassword;
	public TMP_InputField _errorPass;

	[Space]
	[Header("Device ID")]
	[SerializeField]
	string DeviceID, _AmarDeviceId1, _PCDeviceid2;
	public bool testing;
	public GameObject _ErrorScreen;

	[Space]
	[Header("Employee List")]
	[SerializeField]
	List<Employee> _EmployeeList = new List<Employee>();

	[Space]
	[Header("SideMenuList")]
	[SerializeField]
	List<GameObject> _sideMenu = new List<GameObject>();

	[SerializeField]
	Color _NormalColor, _HighlightedColor;

	[Space]
	[Header("Functions for Admin")]
	[SerializeField]
	List<GameObject> _AdminObject = new List<GameObject>();

	[Space]
	[Header("ActiveEmployeeDetail")]
	[SerializeField]
	private TMP_Text _username;

	[Space]
	[Header("Set History Dropdown")]
	[SerializeField]
	private TMP_Dropdown _HistoryDropdown, _HistoryDrop2, _HistoryDrop3;
	public GameObject _HistoryParent, _HistoryChild;




	private void Awake()
	{
		//PlayerPrefs.DeleteAll();
		if (Instance == null)
			Instance = this;
	}

	private void OnEnable()
	{
		SetEmployee();
	}

	// Start is called before the first frame update
	void Start()
	{
		checkdevice();
		SetHistoryyear();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	#region Public Methods

	public void checkdevice()
	{
		//Debug.Log(" id " + SystemInfo.deviceUniqueIdentifier);
		if (PreDeviceId == "" || PreDeviceId == null)
		{
			if (SystemInfo.deviceUniqueIdentifier == DeviceID || SystemInfo.deviceUniqueIdentifier == _AmarDeviceId1 || SystemInfo.deviceUniqueIdentifier == _PCDeviceid2)
			{
				_ErrorScreen.SetActive(false);
				PreDeviceId = DeviceID;
			}
			else
			{
				_ErrorScreen.SetActive(true);
			}
		}

		else
		{
			_ErrorScreen.SetActive(false);
		}
		
	}

	//For Notification
	public void ShowNotification(string notitext)
	{
		_NotificationController._NotificationText.text = notitext;
		_NotificationController.gameObject.SetActive(true);
	}

	public void ShowNotificationTwo(string notitext)
	{
		_NotificationController2._NotificationText.text = notitext;
		_NotificationController2.gameObject.SetActive(true);
	}

	//For Activescreens 
	public void ActiveScreen(int value)
	{
		foreach (GameObject _obj in _screens)
		{
			_obj.SetActive(false);
		}

		foreach (GameObject _sideobj in _sideMenu)
		{
			_sideobj.GetComponent<Image>().color = _NormalColor;
		}

		_screens[value].SetActive(true);
		_sideMenu[value].GetComponent<Image>().color = _HighlightedColor;
	}

	//For ReturnEmployeeList
	public void SetEmployee()
	{
		if (PrefEmployee != "")
		{
			Employeelist _list = JsonUtility.FromJson<Employeelist>(UIManager.Instance.PrefEmployee);
			if (_list != null || _list._EmployeeList.Count != 0)
				_EmployeeList = _list._EmployeeList;
		}
	}

	public void SetEmployeeDetail(string Name, int roleid)
	{
		_username.text = Name;
		if (roleid == 1)
		{
			foreach (GameObject obj in _AdminObject)
				obj.SetActive(false);
		}

		else
		{
			foreach (GameObject obj in _AdminObject)
				obj.SetActive(true);
		}

	}

	public void Onerrorlogin()
	{
		if (PreDeviceId != "")
		{
			if (_errorPass.text == _errorpassword)
				_ErrorScreen.SetActive(false);
		}
			
	}

	public void CheckBrowse()
	{
		//System.Diagnostics.Process.Start("start");
	}

	//SetHistoryYear
	public void SetHistoryyear()
	{
		for (int i = 2021; i < 2050; i++)
		{
			TMP_Dropdown.OptionData _optiondata = new TMP_Dropdown.OptionData();
			_optiondata.text = i.ToString();
			_HistoryDropdown.options.Add(_optiondata);
			_HistoryDrop2.options.Add(_optiondata);
			_HistoryDrop3.options.Add(_optiondata);
		}
			
	}

	//Set History Panel
	public void SetHistoryMenu()
	{
		foreach (Transform child in _HistoryParent.transform)
			Destroy(child.gameObject);


	}


	#endregion


	#region PlayerPrefs
	#region CREDENTIAL
	public string PrefAdminPass
	{
		get { return PlayerPrefs.GetString("ADMINPASS"); }
		set { PlayerPrefs.SetString("ADMINPASS", value); }
	}

	public string PrefAdminID
	{
		get { return PlayerPrefs.GetString("ADMINID"); }
		set { PlayerPrefs.SetString("ADMINID", value); }
	}
	#endregion

	#region DATA
	public string PrefEmployee
	{
		get { return PlayerPrefs.GetString("EMPLO"); }
		set { PlayerPrefs.SetString("EMPLO", value); }
	}

	public string PrefItemData
	{
		get { return PlayerPrefs.GetString("ITEM"); }
		set { PlayerPrefs.SetString("ITEM", value); }
	}

	public string PrefBatchData
	{
		get { return PlayerPrefs.GetString("BATCH"); }
		set { PlayerPrefs.SetString("BATCH", value); }
	}

	public string PrefSteelValues
	{
		get { return PlayerPrefs.GetString("STEEL"); }
		set { PlayerPrefs.SetString("STEEL", value); }
	}

	public string prefDailyPlan
	{
		get { return PlayerPrefs.GetString("PLAN"); }
		set { PlayerPrefs.SetString("PLAN", value); }
	}

	public string prefTodayPlan
    {
        get { return PlayerPrefs.GetString("TODAY"); }
		set { PlayerPrefs.SetString("TODAY",value); }
    }
    #endregion

    #region PATH
    public string PrefReadPath
	{
		get { return PlayerPrefs.GetString("READPATH"); }
		set { PlayerPrefs.SetString("READPATH", value); }
	}


	public string PrefWriteItemPath
	{
		get { return PlayerPrefs.GetString("WRITEPATH"); }
		set { PlayerPrefs.SetString("WRITEPATH", value); }
	}

	public string PrefWriteBatchPath
	{
		get { return PlayerPrefs.GetString("WRITEPATHBATCH"); }
		set { PlayerPrefs.SetString("WRITEPATHBATCH", value); }
	}

	public string PrefWriteSteelPath
	{
		get { return PlayerPrefs.GetString("WRITESTEELPATH"); }
		set { PlayerPrefs.SetString("WRITESTEELPATH", value); }
	}

	public string PrefWriteHistoryPath
	{
		get { return PlayerPrefs.GetString("WRITEPATHHISTORY"); }
		set { PlayerPrefs.SetString("WRITEPATHHISTORY", value); }
	}

	public string PrefWriteRandomBackup
	{
		get { return PlayerPrefs.GetString("RANDOMBACKUP"); }
		set { PlayerPrefs.SetString("RANDOMBACKUP", value); }
	}

	public string PrefWiteBackuppath
	{
		get { return PlayerPrefs.GetString("WRITEPATHEMPLOYEE"); }
		set { PlayerPrefs.SetString("WRITEPATHEMPLOYEE", value); }
	}

	public string PreDeviceId
	{
		get { return PlayerPrefs.GetString("DEVICEID"); }
		set { PlayerPrefs.SetString("DEVICEID", value); }
	}
	#endregion
	#endregion
}