using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmployeeController : MonoBehaviour
{

	public static EmployeeController Instance = null;

	[SerializeField]
	private TMP_InputField _Ename, _EID, _EPassword, _ERePassword;

	[Space]
	[SerializeField]
	List<Employee> _EmployeeList = new List<Employee>();

	[Space]
	[SerializeField]
	private TMP_Text _NError, _IError, _PError, _RPError;

	[Space]
	[SerializeField]
	public GameObject _ParentObject;

	public GameObject _Prefeb;


	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}


	private void OnEnable()
	{

		if (UIManager.Instance.PrefEmployee != "")
		{
			Employeelist _list = JsonUtility.FromJson<Employeelist>(UIManager.Instance.PrefEmployee);
			if (_list != null || _list._EmployeeList.Count != 0)
				_EmployeeList = _list._EmployeeList;
		}

		OnPageRefresh();
	}


	// Start is called before the first frame update
	void Start()
	{


	}

	// Update is called once per frame
	void Update()
	{

	}

	public void CreateEmployee(string _Name, string _ID, string _Pass)
	{
		Employee _Employee = new Employee();
		_Employee.EmployeeName = _Name;
		_Employee.EmployeeID = _ID;
		_Employee.EmployeePassword = _Pass;
		_Employee.dateTime = System.DateTime.Now.ToString();

		Employeelist _Elist = new Employeelist();
		_Elist._EmployeeList = _EmployeeList;

		_Elist._EmployeeList.Add(_Employee);

		string employeedata = JsonUtility.ToJson(_Elist);
		Debug.Log(employeedata);

		_EmployeeList = _Elist._EmployeeList;

		UIManager.Instance.PrefEmployee = employeedata;
	}

	public void OnSaveClick()
	{
		if (_Ename.text == "")
			_NError.text = "Please enter name";
		else if (_EID.text == "")
			_IError.text = "Please enter ID";
		else if (_EPassword.text == "")
			_PError.text = "Please enter Password";
		else if (_ERePassword.text == "")
			_RPError.text = "Please re-enter Password";
		else if (_EPassword.text != _ERePassword.text)
			_RPError.text = "Password mismatch";
		else
		{
			_IError.text = "";
			_NError.text = "";
			_PError.text = "";
			_RPError.text = "";
			foreach (Employee _em in _EmployeeList)
			{
				Debug.Log("inside this");
				if (_em.EmployeeID == _EID.text)
				{
					_NError.text = "";
					_IError.text = "Same ID available";
					_PError.text = "";
					_RPError.text = "";
					return;
				}
				else
				{
					_IError.text = "";
					_NError.text = "";
					_PError.text = "";
					_RPError.text = "";
				}
			}
		}

		if (_IError.text == "" && _NError.text == "" && _PError.text == "" && _RPError.text == "") 
		{
			CreateEmployee(_Ename.text, _EID.text, _EPassword.text);
			_Ename.text = "";
			_EID.text = "";
			_EPassword.text = "";
			_ERePassword.text = "";
		}
			

		OnPageRefresh();

	}


	public void OnPageRefresh()
	{
		LoginManager.Instance.OnEmployeeUpdate();

		foreach (Transform child in _ParentObject.transform)
		{
			Destroy(child.gameObject);
		}


		int i = 0;
		foreach (Employee _emp in _EmployeeList)
		{

			GameObject obj = Instantiate(_Prefeb, _ParentObject.transform);
			EmployeeListDetail detail = obj.GetComponent<EmployeeListDetail>();

			detail.SetEmployeeDetail(_emp.EmployeeName, _emp.EmployeeID, _emp.EmployeePassword, _emp.dateTime, i);
			i++;
		}

		UIManager.Instance.SetEmployee();
	}

	public void OnEmployeeDelete(int number)
	{
		_EmployeeList.RemoveAt(number);

		Employeelist _Elist = new Employeelist();
		_Elist._EmployeeList = _EmployeeList;

		string employeedata = JsonUtility.ToJson(_Elist);
		UIManager.Instance.PrefEmployee = employeedata;

		OnPageRefresh();

		UIManager.Instance.SetEmployee();

		LoginManager.Instance.OnEmployeeUpdate();
	}

}
