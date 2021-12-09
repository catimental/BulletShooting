using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    private Slider SlHP;

    private float fSliderBarTime;
    
    // Start is called before the first frame update
    void Start()
    {
        SlHP = GetComponent<Slider>();
        SlHP.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SlidetActivationCheck();
    }

    private void SlidetActivationCheck()
    {
        if(SlHP.value <= 0)
            transform.Find("Fill Area").gameObject.SetActive(false);
        else
            transform.Find("Fill Area").gameObject.SetActive(true);
    }

    public void SetSliderValue(float value)
    {
        if (value > 0 && !SlHP.gameObject.activeSelf)
        {
            SlHP.gameObject.SetActive(true);
        }
        SlHP.value = value;
    }
}
