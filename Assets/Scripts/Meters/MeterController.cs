using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour {

	private RectTransform rectTransform;
	private Renderer renderer;

	private const int MAXLENGTH = 175;
	private float length;
	private float leftPos;
	// Use this for initialization
	void Start () {
		length = MAXLENGTH;
		rectTransform = GetComponent< RectTransform >();
		leftPos = (int) rectTransform.localPosition.x - MAXLENGTH/2;
		renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Blink () {
		renderer.enabled = !renderer.enabled;
	}


	public void SetLength (float percent) {
		if (percent >= 100 && length < MAXLENGTH) {
			InvokeRepeating("Blink", 0.4f - Time.unscaledTime % 0.4f, 0.2f);
			//Debug.Log(0.2f - Time.unscaledTime % 0.2f);
			//Debug.Log(Time.unscaledTime);
		} else if (percent < 100 && length >= MAXLENGTH) {
			CancelInvoke("Blink");
			renderer.enabled = true;
		}

		length = Mathf.Min(1.0f, (percent / 100.0f)) * (float) MAXLENGTH;
		rectTransform.localScale = new Vector2(length, rectTransform.localScale.y);
		rectTransform.localPosition = new Vector2(leftPos + length/2, rectTransform.localPosition.y);
	}
}
