using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharge : MonoBehaviour {

    [SerializeField]
    private int playerNum;

    public Rigidbody2D rigidbody;
	public MeterController meter;
    public ProjectileController projectile;
    private PlayerStateController stateController;

    private float CHARGERATE = 20f;
	private float DISCHARGERATE = 5f;

	public float charge;
	public bool onGround;
    public bool charging;

    // Use this for initialization
	void Start () {
		charge = 0.0f;
		onGround = false;
		rigidbody = GetComponent<Rigidbody2D>();
        stateController = GetComponent<PlayerStateController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (charge > 100)
        {
            //stateController.SetStateDead();
        }
        if (isCharging()) {
			charge += 0.75f * (1 - charge/100f) * CHARGERATE * Time.deltaTime + 0.5f * CHARGERATE * Time.deltaTime;
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
			charging = true;
			onGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "projectile") {
            Debug.Log(collision.gameObject.GetComponent<ProjectileController>().playerNum);
            Debug.Log(playerNum);
            if (collision.gameObject.GetComponent<ProjectileController>().playerNum != playerNum) {
                float calcDamage = collision.gameObject.GetComponent<ProjectileController>().damage;
                charge += calcDamage;
                Debug.Log(calcDamage);
                if (collision.gameObject.GetComponent<ProjectileController>().damage < 100)
                    Destroy(collision.gameObject);
            }
        } else if ( collision.gameObject.tag == "charge" ) {
            Debug.Log("hit by ball");
            Debug.Log(collision.gameObject.GetComponent<ball2>().charged);
            if ( collision.gameObject.GetComponent<ball2>().charged == true ) {
                Debug.Log("hit by charged ball");
                charge += 50.0f;
                collision.gameObject.GetComponent<ball2>().charged = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charging Platform")
        {
			charging = false;
			onGround = false;
        }
    }

	bool isCharging() {
		return onGround && Mathf.Abs(rigidbody.velocity.x) > 0.25f;
	}
}
