using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 
/// DESCRIPCION:
/// 
/// </summary>


// =========================================================================
public class ProgressiveScore : MonoBehaviour
{
    // =====================================================================
    #region 1) DEFINICION VARIABLES
    public int currentScore;
    public float virtualInitialScore;
    public float virtualFinalScore;

    public float currentTime;
    public float timeProgression;

    float normalizedTime;

    Coroutine progressiveScoreCoro;

    TextMeshProUGUI myText;
    Image image;
    #endregion
    // =====================================================================
    #region 2) FUNCIONES PREDETERMINADAS DE UNITY
    void Awake (){
        myText = GetComponent<TextMeshProUGUI>();
        image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        image.fillAmount = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space)) Start_ProgressiveScoreCoro();
    }
    #endregion
    // =====================================================================
    #region 3) METODOS ORIGINALES
    void Start_ProgressiveScoreCoro()
    {
        if (progressiveScoreCoro == null) 
        {

            Debug.Log("START");

            virtualInitialScore = currentScore;

            AddScore(250);
            progressiveScoreCoro = StartCoroutine(ProgressiveScoreCoro());

        }
        else
        {

            virtualInitialScore = (int)Mathf.Lerp(virtualInitialScore, virtualFinalScore, normalizedTime);

            currentTime = 0f;
            image.fillAmount = 0f;

            AddScore(250);
        }
    }
    void Stop_ProgressiveScoreCoro()
    {
        if (progressiveScoreCoro != null)
        {
            myText.text = virtualFinalScore.ToString("N0");

            virtualInitialScore = 0;
            virtualFinalScore = 0;

            currentTime = 0f;
            image.fillAmount = 0f;


            Debug.Log("STOP");

            StopCoroutine(progressiveScoreCoro);
            progressiveScoreCoro = null;

        }
    }
    IEnumerator ProgressiveScoreCoro()
    {
        float intLerped = 0f;

        currentTime = 0f;
        normalizedTime = currentTime / timeProgression; // from 0 to 1

        while (normalizedTime <= 1f)
        {
            intLerped = Mathf.Lerp (virtualInitialScore, virtualFinalScore, normalizedTime);

            normalizedTime = currentTime / timeProgression;
            currentTime += Time.deltaTime;

            image.fillAmount = normalizedTime;
            myText.text = intLerped.ToString("N0");
            yield return null;
        }

        Debug.Log("Stop 2");

        Stop_ProgressiveScoreCoro();

    }
    void AddScore (int scoreAdded)
    {
        currentScore += scoreAdded;
        virtualFinalScore = currentScore;
    }
    #endregion
    // =====================================================================
}
// =========================================================================
