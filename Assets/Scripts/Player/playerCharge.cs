using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharge : MonoBehaviour {

	public float charge;
	public bool charging;
    // Use this for initialization
	void Start () {
		charge = 0.0f;
		charging = false;
	}
	
	// Update is called once per frame
	void Update () {
        if ( charging && charge < 100) {
			charge += 0.1f;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charging Platform")
        {
			Debug.Log("Platform Detected");
			charging = true;
        }
    }
	void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charging Platform")
        {
			Debug.Log("Platform Detected");
			charging = false;
        }
    }
}
