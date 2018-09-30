using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour {
    public GameObject[] players;
    [SerializeField]
    private float floorValue;
    [SerializeField]
    private float ceilingValue;
    [SerializeField]
    private float wallValueLeft;
    [SerializeField]
    private float wallValueRight;
    [SerializeField]
    private float minimumSize;
    [SerializeField]
    private float maximumSize;
    [SerializeField]
    private float sizeFactor;
    [SerializeField]
    private float maxChange;
    [SerializeField]
    private float minimumHeight;

    private Camera pcam;

    // Use this for initialization
    void Start () {
        pcam = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        
        float xTotal = 0;
        float yTotal = 0;
        float xMax = 0;
        float yMax = 0;
        float xMin = 0;
        float yMin = 0;
        float cHeight = pcam.orthographicSize;
        float cWidth = cHeight * pcam.aspect / 2f;
        
        float[] xpos = new float[players.Length];
        float[] ypos = new float[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            xpos[i] = players[i].transform.position.x;
            ypos[i] = players[i].transform.position.y;
            if (i == 0)
            {
                xMax = xpos[i];
                xMin = xpos[i];
                yMax = ypos[i];
                yMin = ypos[i];
            }
            if (xpos[i] > xMax)
            {
                xMax = xpos[i];
            }
            else if (xpos[i] < xMin)
            {
                xMin = xpos[i];
            }
            if (ypos[i] > yMax)
            {
                yMax = ypos[i];
            }
            else if (ypos[i] < yMin)
            {
                yMin = ypos[i];
            }

            xTotal += xpos[i] / players.Length;
            yTotal += ypos[i] / players.Length;
        }

        // Calculate camera boundary
        float xBoundRight = cWidth + xTotal;
        float xBoundLeft = xTotal - cWidth;
        float yBoundUpper = cHeight + yTotal;
        float yBoundBottom = yTotal - cHeight;

        float xDiff = xMax - xMin;
        float yDiff = yMax - yMin;
        float camVar = 0;

        // Get new camera size
        if (xDiff > yDiff * 2)
        {
            camVar = sizeFactor * Mathf.Pow(xDiff / 1.5f, 1f);
        }
        else
        {
            camVar = sizeFactor * Mathf.Pow(yDiff * 1.25f, 1f);
        }

        // Restrict camera size
        if (camVar < minimumSize)
        {
            camVar = minimumSize;
        }
        else if (camVar > maximumSize)
        {
            camVar = maximumSize;
        }

        // Stop the camera from changing too fast
        if (camVar - pcam.orthographicSize > maxChange)
        {
            camVar = pcam.orthographicSize + maxChange;
        }
        else if (camVar - pcam.orthographicSize < -1 * maxChange)
        {
            camVar = pcam.orthographicSize - maxChange;
        }
        pcam.orthographicSize = camVar;

        // Set minimum and maximum camera position values
        if (yBoundBottom < floorValue)
        {
            yTotal = yTotal - (yBoundBottom - floorValue);
        }
        if (yBoundUpper > ceilingValue)
        {
            yTotal = yTotal - (yBoundUpper - ceilingValue);
        }
        if (xBoundLeft < wallValueLeft)
        {
            xTotal = xTotal - (xBoundLeft - wallValueLeft);
            Debug.Log("xTotal " + xTotal.ToString() + ", " + xBoundLeft.ToString() + ", " + wallValueLeft.ToString());
        }
        if (xBoundRight > wallValueRight)
        {
            xTotal = xTotal - (xBoundRight - wallValueRight);
        }

        // Center the Camera
        transform.position = new Vector3(xTotal, yTotal  - minimumHeight, -10);

    }
}
