using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevUiFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    // Reference DevLogger script
    private DevLogger devLogger;

    private void Awake()
    {
        // Get script-component from GameObject
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
    }

    public void ShowUI() 
    {
        //fadeIn = true;

        gameObject.SetActive(true);
    }
    public void HideUI()
    {
        //fadeOut = true;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (fadeIn)
        {
            gameObject.SetActive(true);

            if (uiGroup.alpha < 1)
            {

                uiGroup.alpha += Time.deltaTime;

                if (uiGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (uiGroup.alpha >= 0)
            {
                uiGroup.alpha -= Time.deltaTime;

                if (uiGroup.alpha == 0)
                {
                    fadeOut = false;

                    gameObject.SetActive(false);
                }
            }
        }
    }
}
