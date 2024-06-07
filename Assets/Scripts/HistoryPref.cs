using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HistoryPref : MonoBehaviour
{
    public TMP_Text No, ItemName, Date, HeatNumber, RecoveryAlloy, ValuesSteel, CastingGrade;
	public List<Matlistsave> _newmatlist = new List<Matlistsave>();
	public Button _button;


    private void Start()
    {
		_button.onClick.AddListener(OnHistoryClick);
    }

    public void OnPrefebSet(string _No, string _ItemName, string _Date, string _HeatNum,string _RecoveryAlloy, string _ValuesSteel, string _CastingGrade, List<Matlistsave> _Matlist)
	{
		No.text = _No;
		ItemName.text = _ItemName;
		Date.text = _Date;
		HeatNumber.text = _HeatNum;
		RecoveryAlloy.text = _RecoveryAlloy;
		ValuesSteel.text = _ValuesSteel;
		CastingGrade.text = _CastingGrade;
		_newmatlist = _Matlist;
	}

	public void OnHistoryClick()
    {
		PrintingManager.instance.SetValues(No.text, ItemName.text, Date.text, HeatNumber.text, RecoveryAlloy.text, ValuesSteel.text, CastingGrade.text, _newmatlist);
    }


}
