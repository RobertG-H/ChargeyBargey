using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {

    Camera cam;
    private bool complete = false;
    private float yRect = 0.5f;
    private float hRect = 0.0f;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(complete);
		if (!complete) {
            yRect -= 0.1f;
            hRect += 0.2f;
            cam.rect = new Rect(0.0f, yRect, 1.0f, hRect);
        }
        if (cam.rect.y < 0 || cam.rect.height > 1) {
            complete = true;
        }
	}
}
