using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharge : MonoBehaviour {

	public Rigidbody2D rigidbody;
	public MeterController meter;

	private float CHARGERATE = 20f;
	private float DISCHARGERATE = 14f;

	public float charge;
	public bool onGround;

    // Use this for initialization
	void Start () {
		charge = 0.0f;
		onGround = false;
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isCharging()) {
			charge += CHARGERATE * Time.deltaTime;
			if (charge > 100) charge = 100;
		}
		else {
			charge -=  DISCHARGERATE * Time.deltaTime;
			if (charge < 0) charge = 0;
		}
		meter.SetLength(charge);
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charging Platform")
        {
			onGround = true;
        }
    }
	void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charging Platform")
        {
			onGround = false;
        }
    }

	bool isCharging() {
		return onGround && Mathf.Abs(rigidbody.velocity.x) > 0.25f;
	}
}
