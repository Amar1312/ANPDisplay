using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class BatchesManager : MonoBehaviour
{

	public ListBatchList _currenBatchesList;
	public ListBatchList _currentSteelList;
	private FileInfo[] info;

	[Space]
	[Header("Show optionselected data")]
	[SerializeField]
	List<DisplayDetailBatch> _optionSeletedData = new List<DisplayDetailBatch>();


	[Space]
	[Header("Item Name")]
	[SerializeField]
	TMP_InputField _batchName;

	[Space]
	[Header("InputData")]
	[SerializeField]
	List<InputBatches> _Input = new List<InputBatches>();

	[Space]
	[Header("Option value")]
	[SerializeField]
	TMP_Dropdown _DropSelection;
	[SerializeField]
	TMP_Dropdown _DropHome , _DropSelectionSteel, _DropSteelHome;


	[Space]
	[Header("Recover batch data")]
	[SerializeField]
	TMP_InputField _BatchJsonPath;

	[Space]
	[Header("Import Batch")]
	[SerializeField]
	TMP_InputField _ImportPath;
	public ListBatchList _ImportBatchList;

	[Space]
	[Header("Changes require for steel")]
	[SerializeField]
	TMP_Text _Header;

	[Space]
	[Header("Import Recovery Steel")]
	[SerializeField]
	TMP_InputField _ImportSteelPath;
	public ListBatchList _ImportSteelList;
	[SerializeField]
	TMP_InputField _ExportPath, _ExportSteelPath;

	[Space]
	[Header("Import Items extras")]
	public string[] _dataList;
	public string[] _matlist;
	public string[] _rawvalue;
	public List<string> _importmat = new List<string>();


	private int CurrentIndex;
	private DateTime _HeighestTime;
	private string BATCHSAVEPATH, BATCHSTEELPATH;



	private void OnEnable()
	{



	}

	// Start is called before the first frame update
	void Start()
	{
		_ExportPath.text = UIManager.Instance.PrefWriteItemPath;
		_ExportSteelPath.text = UIManager.Instance.PrefWriteBatchPath;

		if (UIManager.Instance.PrefBatchData != "")
		{
			ListBatchList _list = JsonUtility.FromJson<ListBatchList>(UIManager.Instance.PrefBatchData);
			_currenBatchesList = _list;
		}

		if(UIManager.Instance.PrefSteelValues != "")
		{
			ListBatchList _list = JsonUtility.FromJson<ListBatchList>(UIManager.Instance.PrefSteelValues);
			_currentSteelList = _list;
		}

		DropDownvalues();
		DropDownSteelvalues();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnAddSteelButtonClick()
	{
		_Header.text = "Add name for steel values";
	}

	public void OnAddValuesOfSteel()
	{
		if (_batchName.text == "")
		{
			UIManager.Instance.ShowNotification("Please add name !!");
			return;
		}

		if (_currentSteelList._list.Count != 0)
		{
			foreach (BatchesList _detial in _currentSteelList._list)
			{
				if (_detial.batchName == _batchName.text)
				{
					UIManager.Instance.ShowNotification("Similar name already exist !!");
					return;
				}
			}
		}


		BatchesList _batchList = new BatchesList();
		_batchList.batchName = _batchName.text;


		for (int i = 0; i < _Input.Count; i++)
		{
			Batchdetail _data = new Batchdetail();
			_data._matname = _Input[i]._Mname.text;
			if (_Input[i].__Vlaue.text != "")
				_data._batchvalue = _Input[i].__Vlaue.text;

			_batchList._batchdetail.Add(_data);
		}

		_currentSteelList._list.Add(_batchList);

		string _json = JsonUtility.ToJson(_currentSteelList);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/steelvalues" + _ctime + ".txt", _json);

		UIManager.Instance.PrefSteelValues = _json;

		DropDownSteelvalues();

		WriteTextSteel();

		UIManager.Instance.ShowNotification("Batch saved successfully !!");

		clearInput();
	}

	public void DropDownSteelvalues()
	{

		_DropSelectionSteel.options.Clear();
		_DropSteelHome.options.Clear();
		TMP_Dropdown.OptionData _optiondatablank = new TMP_Dropdown.OptionData();
		_optiondatablank.text = "Select Steel Values";
		_DropSelectionSteel.options.Add(_optiondatablank);
		_DropSteelHome.options.Add(_optiondatablank);
		_DropSteelHome.captionText.text = _DropSteelHome.options[0].text;

		for (int i = 0; i < _currentSteelList._list.Count; i++)
		{
			TMP_Dropdown.OptionData _optiondata = new TMP_Dropdown.OptionData();
			_optiondata.text = _currentSteelList._list[i].batchName;
			_DropSelectionSteel.options.Add(_optiondata);
			_DropSteelHome.options.Add(_optiondata);
		}
	}

	public void WriteTextSteel()
	{
		string Iname = _currentSteelList._list[(_currentSteelList._list.Count) - 1].batchName;
		BatchesList _detail = _currentSteelList._list[(_currentSteelList._list.Count) - 1];

		/*
		using (StreamWriter file = File.CreateText(BATCHSAVEPATH + Iname + ".csv"))
		{
			file.WriteLine("Material, Value");
			foreach (Batchdetail _data in _detail._batchdetail)
			{
				file.WriteLine(_data._matname + "," + _data._batchvalue);
			}
		}*/
	}

	public void SetAsperSteelDropdown()
	{
		_DropSelection.value = 0;
		Debug.Log(" Drop values     " + _DropSelectionSteel.value);
		Debug.Log(" texttt  " + _DropSelectionSteel.options[_DropSelectionSteel.value].text);
		foreach (BatchesList _detail in _currentSteelList._list)
		{
			if (_DropSelectionSteel.options[_DropSelectionSteel.value].text == _detail.batchName)
			{
				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					_optionSeletedData[i]._Mname.text = _detail._batchdetail[i]._matname;
					_optionSeletedData[i]._value.text = _detail._batchdetail[i]._batchvalue;

				}
			}
			else if (_DropSelectionSteel.options[_DropSelectionSteel.value].text == "Select Steel Values")
			{
				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					_optionSeletedData[i]._Mname.text = "";
					_optionSeletedData[i]._value.text = "";

				}
			}
		}

		if (_currenBatchesList._list.Count == 0)
		{
			for (int i = 0; i < _optionSeletedData.Count; i++)
			{
				_optionSeletedData[i]._Mname.text = "";
				_optionSeletedData[i]._value.text = "";
			}
		}
	}


	


	public void OnAddBatchButtonClick()
	{
		_Header.text = "Add name for recovery of alloys";
	}

	public void OnAddBatchClick()
	{
		if (_batchName.text == "")
		{
			UIManager.Instance.ShowNotification("Please add batch name !!");
			return;
		}

		if (_currenBatchesList._list.Count != 0)
		{
			foreach (BatchesList _detial in _currenBatchesList._list)
			{
				if (_detial.batchName == _batchName.text)
				{
					UIManager.Instance.ShowNotification("Batch with same name already exist !!");
					return;
				}
			}
		}


		BatchesList _batchList = new BatchesList();
		_batchList.batchName = _batchName.text;


		for (int i = 0; i < _Input.Count; i++)
		{
			Batchdetail _data = new Batchdetail();
			_data._matname = _Input[i]._Mname.text;
			if (_Input[i].__Vlaue.text != "")
				_data._batchvalue = _Input[i].__Vlaue.text;

			_batchList._batchdetail.Add(_data);
		}

		_currenBatchesList._list.Add(_batchList);

		string _json = JsonUtility.ToJson(_currenBatchesList);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/batchesjson" + _ctime + ".txt", _json);

		UIManager.Instance.PrefBatchData = _json;

		DropDownvalues();

		WriteText();

		UIManager.Instance.ShowNotification("Recovery of Alloy saved successfully !!");

		clearInput();
	}




	public void DropDownvalues()
	{

		_DropSelection.options.Clear();
		_DropHome.options.Clear();
		TMP_Dropdown.OptionData _optiondatablank = new TMP_Dropdown.OptionData();
		_optiondatablank.text = "Select Batch";
		_DropSelection.options.Add(_optiondatablank);
		_DropHome.options.Add(_optiondatablank);
		_DropHome.captionText.text = _DropHome.options[0].text;

		for (int i = 0; i < _currenBatchesList._list.Count; i++)
		{
			TMP_Dropdown.OptionData _optiondata = new TMP_Dropdown.OptionData();
			_optiondata.text = _currenBatchesList._list[i].batchName;
			_DropSelection.options.Add(_optiondata);
			_DropHome.options.Add(_optiondata);
		}
	}

	//For saving item in form of csv
	public void WriteText()
	{
		string Iname = _currenBatchesList._list[(_currenBatchesList._list.Count) - 1].batchName;
		BatchesList _detail = _currenBatchesList._list[(_currenBatchesList._list.Count) - 1];

		/*
		using (StreamWriter file = File.CreateText(BATCHSAVEPATH + Iname + ".csv"))
		{
			file.WriteLine("Material, Value");
			foreach (Batchdetail _data in _detail._batchdetail)
			{
				file.WriteLine(_data._matname + "," + _data._batchvalue);
			}
		}*/
	}

	// For clear everthing 
	public void clearInput()
	{
		_batchName.text = "";
		foreach (InputBatches _input in _Input)
		{
			_input.__Vlaue.text = "";
		}
	}


	//set as per drop down values
	public void SetAsperDropdown()
	{
		_DropSelectionSteel.value = 0;
		Debug.Log(" Drop values     " + _DropSelection.value);
		Debug.Log(" texttt  " + _DropSelection.options[_DropSelection.value].text);
		foreach (BatchesList _detail in _currenBatchesList._list)
		{
			if (_DropSelection.options[_DropSelection.value].text == _detail.batchName)
			{
				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					_optionSeletedData[i]._Mname.text = _detail._batchdetail[i]._matname;
					_optionSeletedData[i]._value.text = _detail._batchdetail[i]._batchvalue;

				}
			}
			else if (_DropSelection.options[_DropSelection.value].text == "Select Batch")
			{
				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					_optionSeletedData[i]._Mname.text = "";
					_optionSeletedData[i]._value.text = "";

				}
			}
		}

		if (_currenBatchesList._list.Count == 0)
		{
			for (int i = 0; i < _optionSeletedData.Count; i++)
			{
				_optionSeletedData[i]._Mname.text = "";
				_optionSeletedData[i]._value.text = "";
			}
		}
	}

	//Delete Batch
	public void DeleteBatch()
	{
		if (_DropSelection.value != 0)
		{
			_currenBatchesList._list.RemoveAt(_DropSelection.value - 1);
			UIManager.Instance.ShowNotification("Batch Deleted Successfully !!");
			DropDownvalues();

			string _json = JsonUtility.ToJson(_currenBatchesList);
			string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
			//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/batchesjson" + _ctime + ".txt", _json);

			UIManager.Instance.PrefBatchData = _json;
		}

		_DropSelection.value = 0;

		DropDownvalues();
		SetAsperDropdown();
	}

	#region Recover
	public void OnBatchRecoverClick()
	{
		if (_BatchJsonPath.text == "")
		{
			UIManager.Instance.ShowNotification("please insert path !!");
			return;
		}

		string path = _BatchJsonPath.text;

		DirectoryInfo dir = new DirectoryInfo(path);


		try
		{
			info = dir.GetFiles("*.*");
		}
		catch
		{
			UIManager.Instance.ShowNotification("Directory not found");
			return;
		}

		string textdata = System.IO.File.ReadAllText(path + info[0].Name);

		UIManager.Instance.PrefBatchData = textdata;

		UIManager.Instance.ShowNotification("Batch updated successfully !!");

	}
	#endregion


	#region ImportBatch
	public void ImportBatch()
	{
		if (_ImportPath.text == "")
		{
			UIManager.Instance.ShowNotification("please insert path !!");
			return;
		}

		string path = _ImportPath.text;

		FileInfo info = new FileInfo(@path);

		string textdata = "";

		_HeighestTime = info.LastWriteTime;


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
		_matlist = null;
		_importmat.Clear();
		_ImportBatchList = new ListBatchList();


		for (int j = 0; j < _dataList.Length - 1; j++)
		{

			if (j == 0)
			{
				_matlist = _dataList[j].Split(',');

				for (int c = 2; c < _matlist.Length; c++)
				{
					string matname = _matlist[c].Replace("  :max", " ").Trim();
					Debug.Log("material name ::" + matname);
					_importmat.Add(matname);

				}
			}


			else
			{
				Debug.Log("value of j" + j);

				BatchesList _ClientBatchList = new BatchesList();

				for (int i = 0; i < _importmat.Count; i++)
				{
					Batchdetail _data = new Batchdetail();
					_data._matname = _importmat[i];
					_ClientBatchList._batchdetail.Add(_data);

				}

				_ImportBatchList._list.Add(_ClientBatchList);


				_rawvalue = null;
				_rawvalue = _dataList[j].Split(',');

				_ImportBatchList._list[j - 1].batchName = _rawvalue[1];

				for (int k = 0; k < _ImportBatchList._list[j - 1]._batchdetail.Count -1; k++)
				{

					_ImportBatchList._list[j - 1]._batchdetail[k]._batchvalue = _rawvalue[k + 2];

				}
			}
		}

		_currenBatchesList = null;
		_currenBatchesList = _ImportBatchList;

		string _json = JsonUtility.ToJson(_currenBatchesList);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/batchjson" + _ctime + ".txt", _json);

		UIManager.Instance.PrefBatchData = _json;

		DropDownvalues();

		WriteText();

		UIManager.Instance.ShowNotification("Recovery of Alloy imported successfully !!");
	}
	#endregion

	#region ExportBatch
	public void ExportBatches()
	{
		if (_ExportPath.text == "")
		{
			UIManager.Instance.ShowNotification("Please select path of folder");
			return;
		}

		UIManager.Instance.PrefWriteBatchPath = _ExportPath.text;

		string _ctime = "RecoveryOfAlloy"+System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		using (StreamWriter file = File.CreateText(_ExportPath.text + @"\" + _ctime + ".csv"))
		{
			for (int i = 0; i < _currenBatchesList._list.Count; i++)
			{
				if (i == 0)
				{
					file.Write("Sr No" + "," + "Batch Name" + ",");

					for (int j = 0; j < _currenBatchesList._list[i]._batchdetail.Count; j++)
					{
						file.Write(_currenBatchesList._list[i]._batchdetail[j]._matname + ",");
					}
					file.Write('\n');

				}

				file.Write((i + 1) + "," + _currenBatchesList._list[i].batchName + ",");

				for (int k = 0; k < _currenBatchesList._list[i]._batchdetail.Count; k++)
				{
					if (k != _currenBatchesList._list[i]._batchdetail.Count - 1)
						file.Write(_currenBatchesList._list[i]._batchdetail[k]._batchvalue + ",");
					else
						file.Write(_currenBatchesList._list[i]._batchdetail[k]._batchvalue );
				}

				file.Write('\n');
			}
		}

		UIManager.Instance.ShowNotification("Recovery of Alloy exported successfully at :"+"\n" + _ExportPath.text+ @"\" + _ctime + ".csv");

	}
	#endregion

	#region ImportSteelValues
	public void ImportSteelValues()
	{
		if (_ImportSteelPath.text == "")
		{
			UIManager.Instance.ShowNotification("please insert path !!");
			return;
		}

		string path = _ImportSteelPath.text;

		FileInfo info = new FileInfo(@path);


		string textdata = "";

		_HeighestTime = info.LastWriteTime;


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
		_matlist = null;
		_importmat.Clear();
		_ImportBatchList = new ListBatchList();


		for (int j = 0; j < _dataList.Length - 1; j++)
		{

			if (j == 0)
			{
				_matlist = _dataList[j].Split(',');

				for (int c = 2; c < _matlist.Length; c++)
				{
					string matname = _matlist[c].Replace("  :max", " ").Trim();
					Debug.Log("material name ::" + matname);
					_importmat.Add(matname);

				}
			}


			else
			{
				Debug.Log("value of j" + j);

				BatchesList _ClientBatchList = new BatchesList();

				for (int i = 0; i < _importmat.Count; i++)
				{
					Batchdetail _data = new Batchdetail();
					_data._matname = _importmat[i];
					_ClientBatchList._batchdetail.Add(_data);

				}

				_ImportBatchList._list.Add(_ClientBatchList);


				_rawvalue = null;
				_rawvalue = _dataList[j].Split(',');

				_ImportBatchList._list[j - 1].batchName = _rawvalue[1];

				for (int k = 0; k < _ImportBatchList._list[j - 1]._batchdetail.Count - 1; k++)
				{

					_ImportBatchList._list[j - 1]._batchdetail[k]._batchvalue = _rawvalue[k + 2];

				}
			}
		}

		_currentSteelList = null;
		_currentSteelList = _ImportBatchList;

		string _json = JsonUtility.ToJson(_currentSteelList);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/steelvalues" + _ctime + ".txt", _json);

		UIManager.Instance.PrefSteelValues = _json;

		DropDownSteelvalues();

		WriteTextSteel();

		UIManager.Instance.ShowNotification("Steel Values imported successfully !!");
	}
	#endregion

	#region ExportSteelValues
	public void ExportSteelValues()
	{
		if (_ExportSteelPath.text == "")
		{
			UIManager.Instance.ShowNotification("Please select path of folder");
			return;
		}

		UIManager.Instance.PrefWriteSteelPath = _ExportSteelPath.text;

		string _ctime = "SteelValues" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		using (StreamWriter file = File.CreateText(_ExportSteelPath.text + @"\"+ _ctime + ".csv"))
		{
			for (int i = 0; i < _currentSteelList._list.Count; i++)
			{
				if (i == 0)
				{
					file.Write("Sr No" + "," + "Steel Value Name" + ",");

					for (int j = 0; j < _currentSteelList._list[i]._batchdetail.Count; j++)
					{
						file.Write(_currentSteelList._list[i]._batchdetail[j]._matname + ",");
					}
					file.Write('\n');

				}

				file.Write((i + 1) + "," + _currentSteelList._list[i].batchName + ",");

				for (int k = 0; k < _currentSteelList._list[i]._batchdetail.Count; k++)
				{
					if (k != _currentSteelList._list[i]._batchdetail.Count - 1)
						file.Write(_currentSteelList._list[i]._batchdetail[k]._batchvalue + ",");
					else
						file.Write(_currentSteelList._list[i]._batchdetail[k]._batchvalue);
				}

				file.Write('\n');
			}
		}

		UIManager.Instance.ShowNotification("Steel Values exported successfully at :" + "\n" + _ExportSteelPath.text + @"\" + _ctime + ".csv");

	}
	#endregion
}




[System.Serializable]
public class ListBatchList
{
	public List<BatchesList> _list = new List<BatchesList>();
}


[System.Serializable]
public class BatchesList
{
	public string batchName;
	public List<Batchdetail> _batchdetail = new List<Batchdetail>();
}

[System.Serializable]
public class Batchdetail
{
	public string _matname;
	public string _batchvalue;
}

[System.Serializable]
class InputBatches
{
	public TMP_Text _Mname;
	public TMP_InputField __Vlaue;
}


[System.Serializable]
class DisplayDetailBatch
{
	public TMP_Text _Mname, _value;

}
