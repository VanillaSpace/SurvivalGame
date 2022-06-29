using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageIndicator : MonoBehaviour
{
    public Image damageImg;
    public float flashSpeed;

    private Coroutine fadeAway;

    public void Flash ()
    {
        if(fadeAway != null)
        {
            StopCoroutine(fadeAway);
        }

        damageImg.enabled = true;
        damageImg.color = Color.white;
        fadeAway = StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        float alpha = 1.0f;

        while(alpha > 0.0f)
        {
            alpha -= (1.0f / flashSpeed) * Time.deltaTime;
            damageImg.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }

        damageImg.enabled = false;
    }
}
