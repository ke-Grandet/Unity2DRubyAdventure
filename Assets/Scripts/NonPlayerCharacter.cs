using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{

    [Header("�Ի���")]
    public GameObject dialogBox;
    [Header("�Ի�����ʾʱ��")]
    public float displayTime = 4f;

    private float timerDisplayer = -1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplayer > 0)
        {
            timerDisplayer -= Time.deltaTime;
            if (timerDisplayer <= 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        dialogBox.SetActive(true);
        timerDisplayer = displayTime;
    }

}
