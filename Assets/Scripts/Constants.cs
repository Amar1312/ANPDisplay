using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Constants : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}

[Serializable]
public class Employee
{
	public string EmployeeName;
	public string EmployeeID;
	public string EmployeePassword;
	public string dateTime;
}


[Serializable]
public class Employeelist
{
	public List<Employee> _EmployeeList = new List<Employee>();
}

[Serializable]
public class ItemData
{
	public string MName;
	public float Min;
	public float Max;
}

[Serializable]
public class ItemDetail
{
	public string Itemname, TappingTemp, Pouring1st, PouringLast;
	public List<ItemData> _itemdatalist = new List<ItemData>();
}

[Serializable]
public class ItemList
{
	public List<ItemDetail> _ItemDetail = new List<ItemDetail>();
}
