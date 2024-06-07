using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeveloperLogin : MonoBehaviour
{
	[SerializeField]
	private string _DeveloperPassword;

	[SerializeField]
	private TMP_InputField _password;

	[SerializeField]
	private GameObject _ResetPasswordScreen, _OptionSelectionScreen;

	[SerializeField]
	private TMP_InputField _adminNewPassword, _adminNewRepeatPassword;

	[Space]
	[Header("Path value")]
	[SerializeField]
	private TMP_InputField _SetReadPathInput, _SetDailyPlanInput;

	private void OnEnable()
	{
		if (UIManager.Instance.PrefReadPath != "")
			_SetReadPathInput.text = UIManager.Instance.PrefReadPath;
		if (UIManager.Instance.PrefWiteBackuppath != "")
			_SetDailyPlanInput.text = UIManager.Instance.PrefWiteBackuppath;
	}


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnDeveloperLogin()
	{
		if (_password.text == _DeveloperPassword)
		{
			_OptionSelectionScreen.gameObject.SetActive(true);
			_password.text = "";
		}
		else
		{
			UIManager.Instance.ShowNotification("Incorrect Password!");
		}
	}

	private void OnDisable()
	{
		_password.text = "";
	}

	public void OnPassWordReset()
	{
		if (_adminNewPassword.text != "" && _adminNewRepeatPassword.text != "")
		{
			if (_adminNewPassword.text != _adminNewRepeatPassword.text)
				UIManager.Instance.ShowNotification("Password not matched !!");
			else if (_adminNewPassword.text == _adminNewRepeatPassword.text)
			{
				UIManager.Instance.PrefAdminPass = _adminNewPassword.text;
				UIManager.Instance.ShowNotification("Password Updated Successfully!");
				_adminNewPassword.text = "";
				_adminNewRepeatPassword.text = "";

			}
		}
	}


	public void OnSavePath()
	{
		if (_SetReadPathInput.text == "" || _SetDailyPlanInput.text == "")
		{
			UIManager.Instance.ShowNotification("Please insert all path!");
			return;
		}

		UIManager.Instance.PrefReadPath = _SetReadPathInput.text;
		UIManager.Instance.PrefWiteBackuppath = _SetDailyPlanInput.text;
		UIManager.Instance.ShowNotification("Path Updated Successfully!");
	}
}
