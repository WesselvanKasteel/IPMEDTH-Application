using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevUiController : MonoBehaviour
{

    public bool ImageTracked;

    public GameObject UiScanIcon;
    public GameObject Fase1Scanning;

    // Start is called before the first frame update
    void Start()
    {
        ImageTracked = false;

        UiScanIcon.SetActive(true);
        Fase1Scanning.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ImageTracked)
        {
            UiScanIcon.SetActive(false);
            Fase1Scanning.SetActive(false);
        }
    }
}
