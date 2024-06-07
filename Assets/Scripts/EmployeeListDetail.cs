using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EmployeeListDetail : MonoBehaviour
{

	public TMP_Text Ename, EID, EPassword, EDate;

	public int Eid;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetEmployeeDetail(string name, string id, string pass, string date, int _id)
	{
		Ename.text = name;
		EID.text = id;
		EPassword.text = pass;
		EDate.text = date.ToString();
		Eid = _id;
	}

	public void OnDelete()
	{
		EmployeeController.Instance.OnEmployeeDelete(Eid);
	}
}
