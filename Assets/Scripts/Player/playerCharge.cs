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
	private float DISCHARGERATE = 14f;

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
			charging = true;
			onGround = true;
        }
        if (collision.gameObject.tag == "projectile")
        {
            for (int i = 0; i < 4; i++)
            {
                if (collision.gameObject.GetComponent<ProjectileController>().playerNum != playerNum)
                {
                    float calcDamage = collision.gameObject.GetComponent<ProjectileController>().damage;
                    charge += calcDamage;
                    Debug.Log(calcDamage);
                    Destroy(collision.gameObject);
                }
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
