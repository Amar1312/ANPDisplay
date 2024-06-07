using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{

    public static Coroutine IenumValue;
    public GameObject _Prevalues;
    public float WaitForNext;

    private void OnEnable()
    {
        IenumValue = StartCoroutine(IenumWaitSecond());
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator IenumWaitSecond()
    {
        yield return new WaitForSeconds(WaitForNext);
        this.gameObject.SetActive(false);
        _Prevalues.SetActive(true);
    }
}
