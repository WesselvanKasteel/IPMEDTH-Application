using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevFadeOutOnStart : MonoBehaviour
{

    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private bool fadeOut = false;

    [SerializeField] private GameObject gameObjectToActivate;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutOnStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            if (uiGroup.alpha >= 0)
            {
                uiGroup.alpha -= Time.deltaTime;

                if (uiGroup.alpha == 0)
                {
                    fadeOut = false;

                    gameObject.SetActive(false);

                    gameObjectToActivate.SetActive(true);
                }
            }
        }
    }

    IEnumerator FadeOutOnStart()
    {
        yield return new WaitForSeconds(1);

        fadeOut = true;
    }
}
