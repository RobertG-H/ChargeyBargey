using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour {

	private RectTransform rectTransform;

	private const int MAXLENGTH = 200;
	private float length;
	private float leftPos;
	// Use this for initialization
	void Start () {
		length = MAXLENGTH;
		rectTransform = GetComponent< RectTransform >();
		leftPos = (int) rectTransform.localPosition.x - MAXLENGTH/2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void SetLength (float percent) {
		length = Mathf.Min(1.0f, (percent / 100.0f)) * (float) MAXLENGTH;
		rectTransform.localScale = new Vector2(length, rectTransform.localScale.y);
		rectTransform.localPosition = new Vector2(leftPos + length/2, rectTransform.localPosition.y);
	}
}
