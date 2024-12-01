using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private bool timerActive = false;
    private float timer = 0f;
    private float delay = 3f;

    public GameObject resultPanel;
    public TMP_Text first1Round;
    public TMP_Text first2Round;
    public TMP_Text second1Round;
    public TMP_Text second2Round;
    public TMP_Text third1Round;
    public TMP_Text third2Round;

    private int round11;
    private int round12;
    private int round21;
    private int round22;
    private int round31;
    private int round32;

    public TMP_Text finalResultText;

    private void Awake()
    {
        // Реализация Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }
        //resultPanel = GameObject.Find("resultPanel");
    }

    public void ShowResult(int result, int round)
    {
        resultPanel.SetActive(true);
        timerActive = true;
        switch (round) 
        {
            case 1:
                round11 = result;
                break;
            case 2:
                round12 = result;
                break;
            case 3:
                round21 = result;
                break;
            case 4:
                round22 = result;
                break;
            case 5:
                round31 = result;
                break;
            case 6:
                round32 = result;
                break;
        }
        first1Round.text = round11.ToString();
        first2Round.text = round12.ToString();
        second1Round.text = round21.ToString();
        second2Round.text = round22.ToString();
        third1Round.text = round31.ToString();
        third2Round.text = round32.ToString();
    }

    public void ShowFinalResults(List<int> results)
    {
        resultPanel.SetActive(true);
        int total = 0;
        for (int i = 0; i < results.Count; i++)
        {
            total += results[i];
        }
        finalResultText.text = total.ToString();
    }

    private void Update()
    {
        // Если таймер активен, уменьшаем время
        if (timerActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timerActive = false; // Останавливаем таймер
                resultPanel.SetActive(false);
                Debug.Log("Таймер завершён. Переменная passed изменена.");
            }
        }
    }
}
