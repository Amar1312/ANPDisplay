using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationController : MonoBehaviour
{
	public static NotificationController Instance = null;

	public TMP_Text _NotificationText;
	public float WaitTime = 4;


	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void OnEnable()
	{
		StartCoroutine(IenumShowNotification());
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator IenumShowNotification()
	{
		yield return new WaitForSeconds(WaitTime);
		this.gameObject.SetActive(false);
	}
}
