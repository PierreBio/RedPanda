using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClawUI : MonoBehaviour
{
    private float duration;
    [SerializeField] RawImage uiImage;


    private void OnEnable()
    {
        StartCoroutine(DisappearCoroutine());
    }

    public void SetDuration(float inDuration)
    {
        duration = inDuration;
    }

    IEnumerator DisappearCoroutine ()
    {
        Color color = uiImage.color;
        float time = 0;
        while (time < duration)
        {
            color.a = (duration - time)/duration;
            uiImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
