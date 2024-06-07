using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance = null;

    [SerializeField]
    private TMP_InputField _LoginInput, _PasswordInput;

    [SerializeField]
    private TMP_Text _LoginError, _PasswordError;

    [SerializeField]
    List<Employee> _EmployeeList = new List<Employee>();

    [SerializeField]
    private Toggle _Admin, _Emplyee;


    [Space]
    [Header("Admin Credentail")]
    [SerializeField]
    private string _ID;
    [SerializeField]
    private string _Password, _AdminName;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        if (UIManager.Instance.PrefAdminPass == "")
            UIManager.Instance.PrefAdminPass = _Password;

        if (UIManager.Instance.PrefEmployee != "")
        {
            Employeelist _list = JsonUtility.FromJson<Employeelist>(UIManager.Instance.PrefEmployee);
            if (_list != null || _list._EmployeeList.Count != 0)
                _EmployeeList = _list._EmployeeList;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEmployeeUpdate()
    {
        if (UIManager.Instance.PrefEmployee != "")
        {
            Employeelist _list = JsonUtility.FromJson<Employeelist>(UIManager.Instance.PrefEmployee);
            if (_list != null || _list._EmployeeList.Count != 0)
                _EmployeeList = _list._EmployeeList;
        }
    }

    public void OnLoginClick()
    {
        if (UIManager.Instance.PrefReadPath == "" || UIManager.Instance.PrefWiteBackuppath == "")
        {
            UIManager.Instance.ShowNotification("Please insert all path for database");
            return;
        }


        if (_LoginInput.text == null || _LoginInput.text == "")
            _LoginError.text = "Please Enter Login ID";

        else if (_PasswordInput.text == null || _PasswordInput.text == "")
            _PasswordError.text = "Please Enter Password";

        else if (_LoginInput.text != _ID && _PasswordInput.text != UIManager.Instance.PrefAdminPass && _Admin.isOn)
        {
            _LoginError.text = "ID or Password wrong!";
            _PasswordError.text = "";
        }

        else if (_Admin.isOn)
        {
            if (_LoginInput.text == _ID && _PasswordInput.text == UIManager.Instance.PrefAdminPass)
            {
                _LoginError.text = "";
                _PasswordError.text = "";
                this.gameObject.SetActive(false);
                UIManager.Instance.ActiveScreen(0);
                UIManager.Instance.SetEmployeeDetail(_AdminName, 0);
                _LoginInput.text = "";
                _PasswordInput.text = "";
                DataReader.Instance.OnLogin();
            }
        }

        else if (_Emplyee.isOn)
        {

            if (UIManager.Instance.PrefEmployee != "")
            {
                foreach (Employee _emp in _EmployeeList)
                {
                    Debug.Log(_emp.EmployeeID + " " + _emp.EmployeePassword);
                    if (_emp.EmployeeID == _LoginInput.text && _emp.EmployeePassword == _PasswordInput.text)
                    {
                        _LoginError.text = "";
                        _PasswordError.text = "";
                        this.gameObject.SetActive(false);
                        UIManager.Instance.ActiveScreen(0);
                        UIManager.Instance.SetEmployeeDetail(_emp.EmployeeName, 1);
                        DataReader.Instance.OnLogin();
                        _LoginInput.text = "";
                        _PasswordInput.text = "";
                        return;
                    }
                    else
                    {
                        _LoginError.text = "ID or Password wrong!";
                    }
                }
            }
        }
    }
}
