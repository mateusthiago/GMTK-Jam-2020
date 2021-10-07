using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesLeftLabel;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI endLabel;
    [SerializeField] TextMeshProUGUI boostLabel;

    public static UI_Controller instance;

    void Awake()
    {
        instance = this;
        StartCoroutine(StartTextTimer());
    }

    public void UpdateScore(int score)
    {
        enemiesLeftLabel.text = score.ToString();
    }

    public void UpdateHealthBar(float value)
    {
        //print("health" + value);
        healthBar.value = value;        
    }

    public void EndGame(bool win)
    {
        if (win) endLabel.text = "A WINNER IS YOU";
        else endLabel.text = "YOU EXPLODED INTO LITTLE PIECES";
        StartCoroutine(EndTextTimer());
    }
    private IEnumerator EndTextTimer()
    {
        float t = 0;
        while (t < 1)
        {
            endLabel.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < 5)
        {
            t += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    private IEnumerator StartTextTimer()
    {
        yield return new WaitForSeconds(2);
        endLabel.text = "YOU MUST SURVIVE\n 100 ENEMIES";
        float t = 0;
        while (t < 1)
        {
            endLabel.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        t = 0;
        while (t < 1)
        {
            endLabel.color = Color.Lerp(Color.white, Color.clear, t);
            t += Time.deltaTime;
            yield return null;
        }

        endLabel.text = "";
    }

    public void UpdateBoostLabel(float value)
    {
        string percentBoost = string.Format("{0}%", Mathf.RoundToInt(value * 100).ToString());
        boostLabel.text = percentBoost;
    }
}
