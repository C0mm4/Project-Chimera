using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBarUI : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Image fillFront;
    [SerializeField] Image fillBack;

    [Header("게이지 바 색상")]
    [SerializeField] Color fillFrontNormalColor;
    [SerializeField] Color fillFrontLowValueColor;
    [SerializeField, Range(0, 1)] float frontLowThreshold;
    [SerializeField] Color fillBackNormalColor;
    [SerializeField] Color fillBackLowValueColor;
    [SerializeField, Range(0, 1)] float backLowThreshold;
  
    Canvas canvas;

    AnimationCurve curve;
    float frontValue;
    float backValue;

    Coroutine lerpCoroutine;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }


    void Start()
    {
        canvas.worldCamera = Camera.main;
        SetFillPercent(1f);
    }

    private void Update()
    {
        if (frontValue < frontLowThreshold)
        {
            if (fillFront.color != fillFrontLowValueColor)
            {
                fillFront.color = fillFrontLowValueColor;
            }
        }
        else
        {
            if (fillFront.color != fillFrontNormalColor)
            {
                fillFront.color = fillFrontNormalColor;
            }
        }

        if (backValue < backLowThreshold)
        {
            if (fillBack.color != fillBackLowValueColor)
            {
                fillBack.color = fillBackLowValueColor;
            }
        }
        else
        {
            if (fillBack.color != fillBackNormalColor)
            {
                fillBack.color = fillBackNormalColor;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetFillPercent(frontValue - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetFillPercent(frontValue + 0.1f);
        }
    }

    private void LateUpdate()
    {
        transform.localPosition = offset;
        
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetFillPercent(float percent)
    {
        frontValue = percent;
        fillFront.fillAmount = frontValue;

        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpGaugeCoroutine(backValue, frontValue, 0.5f));

    }

    IEnumerator LerpGaugeCoroutine(float start, float end, float t)
    {
        float elapsed = 0;

        while (elapsed < t)
        {
            fillBack.fillAmount = Mathf.Lerp(start, end, curve.Evaluate(elapsed/t));
            backValue = fillBack.fillAmount;
            yield return null;

            elapsed += Time.deltaTime;
        }

        
    }
    
}
