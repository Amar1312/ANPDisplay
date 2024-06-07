using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class ItemManager : MonoBehaviour
{

	public ItemList _currentitemlist;
	private FileInfo[] info;

	[Space]
	[Header("Show optionselected data")]
	[SerializeField]
	List<DisplayDetail> _optionSeletedData = new List<DisplayDetail>();
	[SerializeField]
	TMP_InputField _STappingtemp, _SPouringTemp1st, _SPouringTemplast;


	[Space]
	[Header("Item Name")]
	[SerializeField]
	TMP_InputField _itemname;
	[SerializeField]
	TMP_InputField _Tappingtemp, _PouringTemp1st, _PouringTemplast;

	[Space]
	[Header("InputData")]
	[SerializeField]
	List<Inputdetail> _Input = new List<Inputdetail>();

	[Space]
	[Header("Option value")]
	[SerializeField]
	TMP_Dropdown _DropSelection, _DropHome;

	[Space]
	[Header("Recover Item data")]
	[SerializeField]
	TMP_InputField _ItemJsonPath;

	[Space]
	[Header("Import Items")]
	[SerializeField]
	TMP_InputField _ImportPath;
	public ItemList _ImportItemlist ;
	[SerializeField]
	TMP_InputField _ExportPath;

	[Space]
	[Header("Import Items extras")]
	public string[] _dataList;
	public string[] _matlist;
	public string[] _rawvalue;
	public List<string> _importmat = new List<string>();

	private string ITEMSAVEPATH;
	private int CurrentIndex;
	private DateTime _HeighestTime;


	private void OnEnable()
	{

	}

	// Start is called before the first frame update
	void Start()
	{
		_ExportPath.text = UIManager.Instance.PrefWriteItemPath;

		if (UIManager.Instance.PrefItemData != "")
		{
			ItemList _list = JsonUtility.FromJson<ItemList>(UIManager.Instance.PrefItemData);
			_currentitemlist = _list;
		}

		DropDownvalues();
	}

	// Update is called once per frame
	void Update()
	{

	}


	public void OnAddItemClick()
	{
		if (_itemname.text == "")
		{
			UIManager.Instance.ShowNotification("Please add item name !!");
			return;
		}

		if (_currentitemlist._ItemDetail.Count != 0)
		{
			foreach (ItemDetail _detial in _currentitemlist._ItemDetail)
			{
				if (_detial.Itemname == _itemname.text)
				{
					UIManager.Instance.ShowNotification("Item with same name already exist !!");
					return;
				}
			}
		}


		ItemDetail _ItemDetail = new ItemDetail();
		_ItemDetail.Itemname = _itemname.text;
		_ItemDetail.TappingTemp = _Tappingtemp.text;
		_ItemDetail.Pouring1st = _PouringTemp1st.text;
		_ItemDetail.PouringLast = _PouringTemplast.text;


		for (int i = 0; i < _Input.Count; i++)
		{
			ItemData _data = new ItemData();
			_data.MName = _Input[i]._Name.text;
			if (_Input[i]._Minvalue.text != "")
				_data.Min = float.Parse(_Input[i]._Minvalue.text);
			if (_Input[i]._Maxvalue.text != "")
				_data.Max = float.Parse(_Input[i]._Maxvalue.text);

			_ItemDetail._itemdatalist.Add(_data);
		}

		_currentitemlist._ItemDetail.Add(_ItemDetail);

		string _json = JsonUtility.ToJson(_currentitemlist);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/itemsjson" + _ctime + ".txt", _json);

		UIManager.Instance.PrefItemData = _json;

		DropDownvalues();

		WriteText();

		UIManager.Instance.ShowNotification("Item saved successfully !!");

		clearInput();

	}



	//For saving item in form of csv
	public void WriteText()
	{
		string Iname = _currentitemlist._ItemDetail[(_currentitemlist._ItemDetail.Count) - 1].Itemname;
		ItemDetail _detail = _currentitemlist._ItemDetail[(_currentitemlist._ItemDetail.Count) - 1];

		/*
		using (StreamWriter file = File.CreateText(ITEMSAVEPATH + Iname + ".csv"))
		{
			for (int i = 0; i < _detail._itemdatalist.Count; i ++)
			{
				if(i == 0){
					file.Write("Sr No" + "," + "Item Name" + "," + "Tapping temp." + "," + "Pouring temp. first" + "," + "Pouring temp. last" + ",");
				}
				file.Write(_detail._itemdatalist[i].MName+ "  :min" + "," + _detail._itemdatalist[i].MName + "  :max" + ",");
			}
		}
		*/
	}




	public void OnbackupClick()
	{
		string jsonTransform = File.ReadAllText(@"d:\json.txt");
		Debug.Log(jsonTransform);
	}

	public void DropDownvalues()
	{

		_DropSelection.options.Clear();
		_DropHome.options.Clear();
		TMP_Dropdown.OptionData _optiondatablank = new TMP_Dropdown.OptionData();
		_optiondatablank.text = "Select Item";
		TMP_Dropdown.OptionData _optiondatablankhome = new TMP_Dropdown.OptionData();
		_optiondatablankhome.text = "Item Not Found";
		_DropSelection.options.Add(_optiondatablank);
		_DropHome.options.Add(_optiondatablankhome);

		for (int i = 0; i < _currentitemlist._ItemDetail.Count; i++)
		{
			TMP_Dropdown.OptionData _optiondata = new TMP_Dropdown.OptionData();
			_optiondata.text = _currentitemlist._ItemDetail[i].Itemname;
			_DropSelection.options.Add(_optiondata);
			_DropHome.options.Add(_optiondata);
		}
	}


	public void SetAsperDropdown()
	{
		//Debug.Log(" Drop values     " + _DropSelection.value);
		//Debug.Log(" texttt  " + _DropSelection.options[_DropSelection.value].text);
		foreach (ItemDetail _detail in _currentitemlist._ItemDetail)
		{


			if (_DropSelection.options[_DropSelection.value].text == _detail.Itemname)
			{
				_STappingtemp.text = _detail.TappingTemp;
				_SPouringTemp1st.text = _detail.Pouring1st;
				_SPouringTemplast.text = _detail.PouringLast;

				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					//Debug.Log("value ::" + _detail._itemdatalist[i].MName);
					_optionSeletedData[i]._Name.text = _detail._itemdatalist[i].MName;
					_optionSeletedData[i]._Minvalue.text = _detail._itemdatalist[i].Min.ToString();
					_optionSeletedData[i]._Maxvalue.text = _detail._itemdatalist[i].Max.ToString();
				}
			}
			else if (_DropSelection.options[_DropSelection.value].text == "Select Item")
			{
				_STappingtemp.text = "";
				_SPouringTemp1st.text = "";
				_SPouringTemplast.text = "";

				for (int i = 0; i < _optionSeletedData.Count; i++)
				{
					//Debug.Log("value 2222 ::" + _detail._itemdatalist[i].MName);
					_optionSeletedData[i]._Name.text = "";
					_optionSeletedData[i]._Minvalue.text = "";
					_optionSeletedData[i]._Maxvalue.text = "";
				}
			}
		}

		if (_currentitemlist._ItemDetail.Count == 0)
		{
			for (int i = 0; i < _optionSeletedData.Count; i++)
			{
				_optionSeletedData[i]._Name.text = "";
				_optionSeletedData[i]._Minvalue.text = "";
				_optionSeletedData[i]._Maxvalue.text = "";
			}
		}
	}


	public void DeleteItem()
	{
		if (_DropSelection.value != 0)
		{
			_currentitemlist._ItemDetail.RemoveAt(_DropSelection.value - 1);
			UIManager.Instance.ShowNotification("Item Deleted Successfully !!");
			DropDownvalues();

			string _json = JsonUtility.ToJson(_currentitemlist);
			string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
			//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/itemsjson" + _ctime + ".txt", _json);

			UIManager.Instance.PrefItemData = _json;
		}

		_DropSelection.value = 0;

		DropDownvalues();
		SetAsperDropdown();
	}


	//clear all input field
	public void clearInput()
	{
		_itemname.text = "";
		_Tappingtemp.text = "";
		_PouringTemp1st.text = "";
		_PouringTemplast.text = "";


		foreach (Inputdetail _input in _Input)
		{
			_input._Maxvalue.text = "";
			_input._Minvalue.text = "";
		}
	}


	#region RecoverItems
	public void OnItemRecoverClick()
	{
		if (_ItemJsonPath.text == "")
		{
			UIManager.Instance.ShowNotification("please insert path !!");
			return;
		}

		string path = _ItemJsonPath.text;

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

		UIManager.Instance.PrefItemData = textdata;

		UIManager.Instance.ShowNotification("Items updated successfully !!");

	}
	#endregion

	#region ImportItems
	public void ImportItems()
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
		_ImportItemlist = new ItemList();

		for (int j = 0; j < _dataList.Length - 1; j++)
		{

			if (j == 0)
			{
				_matlist = _dataList[j].Split(',');

				for (int c = 6; c < _matlist.Length; c++)
				{
					if (c % 2 == 0)
					{
						string matname = _matlist[c].Replace("  :tol", " ").Trim();
						Debug.Log("material name ::" + matname);
						_importmat.Add(matname);
					}
				}


			}


			else
			{
				Debug.Log("value of j" + j);

				ItemDetail _CItemDetail = new ItemDetail();

				for (int i = 0; i < _importmat.Count; i++)
				{
					ItemData _data = new ItemData();
					_data.MName = _importmat[i];
					_CItemDetail._itemdatalist.Add(_data);

				}

				_ImportItemlist._ItemDetail.Add(_CItemDetail);

				_rawvalue = null;
				_rawvalue = _dataList[j].Split(',');

				_ImportItemlist._ItemDetail[j - 1].Itemname = _rawvalue[1];
				_ImportItemlist._ItemDetail[j - 1].TappingTemp = _rawvalue[2];
				_ImportItemlist._ItemDetail[j - 1].Pouring1st = _rawvalue[3];
				_ImportItemlist._ItemDetail[j - 1].PouringLast = _rawvalue[4];


				int w = 0;
				for (int k = 0; k < _ImportItemlist._ItemDetail[j - 1]._itemdatalist.Count; k++)
				{
					//Debug.Log("value ok k::" + k);
					for (int c = (w + 5); c < (w + 7); c++)
					{
						//Debug.Log("value of raw value C " + c + " " + _rawvalue[c]);
						if (c == (w + 5))
						{
							Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrr" + _rawvalue[c]);
							_ImportItemlist._ItemDetail[j - 1]._itemdatalist[k].Min = (_rawvalue[c] == "" ? 0 : float.Parse(_rawvalue[c]));
						}
							
						else if (c == (w + 6))
						{
							try
							{
								_ImportItemlist._ItemDetail[j - 1]._itemdatalist[k].Max = (_rawvalue[c] == "" ? 0 : float.Parse(_rawvalue[c]));
							}
							catch
							{
								_rawvalue[c] = "0";
								_ImportItemlist._ItemDetail[j - 1]._itemdatalist[k].Max = (_rawvalue[c] == "" ? 0 : float.Parse(_rawvalue[c]));
							}
							
						}
							
					}
					w = w + 2;
				}

				Debug.Log("outttttttttttt");
			}
		}

		_currentitemlist = null;
		_currentitemlist = _ImportItemlist;

		string _json = JsonUtility.ToJson(_currentitemlist);
		string _ctime = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/itemsjson" + _ctime + ".txt", _json);

		UIManager.Instance.PrefItemData = _json;

		DropDownvalues();

		WriteText();

		UIManager.Instance.ShowNotification("Items updated successfully !!");
	}
	#endregion

	#region ExportItems
	public void ExportItems()
	{
		if (_ExportPath.text == "")
		{
			UIManager.Instance.ShowNotification("Please select path of folder");
			return;
		}

		UIManager.Instance.PrefWriteItemPath = _ExportPath.text;
		string _ctime = "Items"+System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Hour.ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString();
		using (StreamWriter file = File.CreateText(_ExportPath.text + @"\" + _ctime + ".csv"))
		{
			for (int i = 0; i < _currentitemlist._ItemDetail.Count; i++)
			{
				if (i == 0)
				{
					file.Write("Sr No" + "," + "Item Name" + "," + "Tapping temp." + "," + "Pouring temp. first" + "," + "Pouring temp. last" + ",");

					for (int j = 0; j < _currentitemlist._ItemDetail[i]._itemdatalist.Count; j++)
					{
						file.Write(_currentitemlist._ItemDetail[i]._itemdatalist[j].MName + "  :fv" + "," + _currentitemlist._ItemDetail[i]._itemdatalist[j].MName + "  :tol" + ",");
					}
					file.Write('\n');

				}

				file.Write((i + 1) + "," + _currentitemlist._ItemDetail[i].Itemname + "," + _currentitemlist._ItemDetail[i].TappingTemp + "," + _currentitemlist._ItemDetail[i].Pouring1st + "," + _currentitemlist._ItemDetail[i].PouringLast + ",");

				for (int k = 0; k < _currentitemlist._ItemDetail[i]._itemdatalist.Count; k++)
				{
					file.Write(_currentitemlist._ItemDetail[i]._itemdatalist[k].Min + "," + _currentitemlist._ItemDetail[i]._itemdatalist[k].Max + ",");
				}

				file.Write('\n');
			}
		}

		UIManager.Instance.ShowNotification("Items exported successfully at :"+ '\n' + _ExportPath.text + @"\" + _ctime + ".csv");

	}
	#endregion
}

[System.Serializable]
class Inputdetail
{
	public TMP_Text _Name;
	public TMP_InputField _Minvalue, _Maxvalue;
}

[System.Serializable]
class DisplayDetail
{
	public TMP_Text _Name, _Minvalue, _Maxvalue;
}

