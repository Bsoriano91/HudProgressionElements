using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// 
/// DESCRIPCION:
/// 
/// </summary>


// =========================================================================
public class ProgressiveBar : MonoBehaviour
{
    // =====================================================================
    #region 1) DEFINICION VARIABLES
    public BarData barData;


    Coroutine myCoro;

    public float virtualInitialValue, virtualFinalValue, currentTime, timeProgression, normalizedTime;
    #endregion
    // =====================================================================
    #region 2) FUNCIONES PREDETERMINADAS DE UNITY
    void Awake () => InitializeBarData();
    
    void Start() => RefreshBar();

    private void Reset() => InitializeBarData();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Start_MyCoro(250f);
        if (Input.GetMouseButtonDown(1)) Start_MyCoro(-250f);
    }
    #endregion
    // =====================================================================
    #region 3) METODOS ORIGINALES
    void InitializeBarData()
    {
        TextMeshProUGUI _txtTemp = transform.GetChild(transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        Image[] _imgsTemp = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            _imgsTemp[i] = transform.GetChild(i).GetComponent<Image>();
        }

        barData = new BarData(_imgsTemp, _txtTemp);

    }

    void RefreshBar()
    {

        barData.myImgs[2].fillAmount = GetNormalizedBarStatus();

        string message = string.Format("{0} {1} {2}", barData.currentValue, "/", barData.maxValue);
        barData.myText.text = message;
    }

    float GetNormalizedBarStatus()
    {
        return barData.currentValue / barData.maxValue;
    }

    #region COROUTINE MANAGEMENT
    void Start_MyCoro(float _value)
    {
        if (myCoro == null)
        {
            virtualInitialValue = barData.currentValue;
            barData.currentValue += _value;
            barData.currentValue = Mathf.Clamp(barData.currentValue, 0f, barData.maxValue);

            virtualFinalValue = barData.currentValue;
            myCoro = StartCoroutine(ProgressiveScoreCoro());
        }
        else
        {

            virtualInitialValue = (int)Mathf.Lerp(virtualInitialValue, virtualFinalValue, normalizedTime);
            barData.currentValue += _value;
            virtualFinalValue = barData.currentValue;

            currentTime = 0f;
            barData.myImgs[2].fillAmount = GetNormalizedBarStatus();
        }
    }
    void Stop_ProgressiveScoreCoro()
    {        
        virtualInitialValue = 0;
        virtualFinalValue = 0;
        currentTime = 0f;
        normalizedTime = 0f;

        StopCoroutine(myCoro);
        myCoro = null;
    }
    IEnumerator ProgressiveScoreCoro()
    {
        float progressLerped = 0f;

        currentTime = 0f;
        normalizedTime = currentTime / timeProgression; // from 0 to 1

        barData.myImgs[2].fillAmount = GetNormalizedBarStatus();

        string message = string.Empty;

        while (normalizedTime <= 1f)
        {
            progressLerped = Mathf.Lerp(virtualInitialValue, virtualFinalValue, normalizedTime);

            normalizedTime = currentTime / timeProgression;
            currentTime += Time.deltaTime;

            barData.myImgs[1].fillAmount = progressLerped / barData.maxValue;

            message = string.Format("{0} {1} {2}", progressLerped.ToString("N0"), "/", barData.maxValue);
            barData.myText.text = message;
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        progressLerped = Mathf.Lerp(virtualInitialValue, virtualFinalValue, normalizedTime);
        
        barData.myImgs[1].fillAmount = progressLerped / barData.maxValue;

        message = string.Format("{0} {1} {2}", progressLerped.ToString("N0"), "/", barData.maxValue);
        barData.myText.text = message;

        Stop_ProgressiveScoreCoro();

    }
    #endregion
    #endregion
    // =====================================================================
}

[Serializable]
public class BarData
{
    public float currentValue;
    public float maxValue;

    internal Image[] myImgs;
    internal TextMeshProUGUI myText;

    public BarData(Image[] _myImgs, TextMeshProUGUI _myText)
    {
        myImgs = _myImgs;
        myText = _myText;

        maxValue = 500f;
        currentValue = maxValue;
    }
}
// =========================================================================
