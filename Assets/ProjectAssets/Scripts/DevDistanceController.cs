using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDistanceController : MonoBehaviour
{

    private float minDist = 1f;

    private float fadeSpeed = 2f;
    public float timeToStartFading = 0.5f;

    private static GameObject arCamera;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        var all = FindObjectsOfType<GameObject>();

        foreach (GameObject item in all)
        {
            if (item.tag.CompareTo("ArCamera") == 0)
            {
                arCamera = item;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, arCamera.transform.position);

        //Timer
        if (timeToStartFading > 0)
        {
            timeToStartFading -= Time.deltaTime;
            return;
        }

        if (dist < minDist)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a + (fadeSpeed * Time.deltaTime));
        }
        else if (dist > minDist)
        {
            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - (fadeSpeed * Time.deltaTime));
        }
    }

    IEnumerator FadeIn()
    {

        yield return null;
    }

    IEnumerator FadeOut()
    {
        yield return null;
    }
}
