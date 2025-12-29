using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GojoSatoru : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;
    public TMP_Text storyText;
    //public TMP_Text timerText;

    [Header("Story")]
    [TextArea(3, 6)]
    public string[] storyLines;

    [Header("Timing")]
    public float lineDelay = 1.5f;
    public float fadeStep = 0.15f;
    public float timerDuration = 3f;

    private void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        storyText.text = "";

        // Полное затемнение
       

        Color fadeColor = fadeImage.color;
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;

        yield return StartCoroutine(FadeToBlack());
        // Постепенно добавляем текст + затемняем экран
        foreach (string line in storyLines)
        {
            storyText.text += line + "\n";
            yield return new WaitForSeconds(lineDelay);
        }

        // Таймер
        float timer = timerDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            //timerText.text = $"20:31 Прибыл Годжо Сатору";
            yield return null;
        }

        SceneManager.LoadScene("MainLevel");
    }

    IEnumerator IncreaseFade()
    {
        float targetAlpha = Mathf.Clamp01(fadeImage.color.a + fadeStep);
        float t = 0;
        Color c = fadeImage.color;

        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(c.a, targetAlpha, t);
            fadeImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        float t = 0;
        Color c = fadeImage.color;
        float startAlpha = c.a;

        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 1f, t);
            fadeImage.color = c;
            yield return null;
        }
    }
}
