using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContrl : MonoBehaviour
{
    public static TextContrl Instance;
    public GameObject readyText;
    public GameObject gameOverText;
    public GameObject clearText;
    private void Awake()
    {
        if (TextContrl.Instance == null)
        {
            TextContrl.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        readyText.SetActive(false);
        gameOverText.SetActive(false);
        clearText.SetActive(false);
        StartCoroutine(ShowReady());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        gameOverText.SetActive(false);
        clearText.SetActive(false);
        StartCoroutine(ShowReady());
        
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 3)
        {
            readyText.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            readyText.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }

    public void ShowGameClear()
    {
        clearText.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOverText.SetActive(true);
    }
}
