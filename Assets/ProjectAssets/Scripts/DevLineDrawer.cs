using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevLineDrawer : MonoBehaviour
{
    public LineRenderer line;

    public Transform worldOrigin;
    public Transform objectOrigin;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, worldOrigin.position);
        line.SetPosition(1, objectOrigin.position);
    }
}
