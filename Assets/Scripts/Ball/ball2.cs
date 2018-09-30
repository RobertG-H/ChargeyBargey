using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball2 : MonoBehaviour {

	public bool charged = false;
	private Rigidbody2D body;
	private Animator anim;
	// Use this for initialization
	public float chargeTime = 5.0f;
	void Start () {
		anim = gameObject.GetComponent<Animator>();
		body = gameObject.GetComponent<Rigidbody2D>();
		body.sharedMaterial.bounciness = 0;
		body.mass = 500;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if( charged ) {
			chargeTime = chargeTime - Time.deltaTime; 
			if( chargeTime < 0 ) {
				disableBall();
			}
		} else if ( !charged ) {
			disableBall();
		}
	}
	public void Move(float speed, Vector2 velocity)
    {
		body.mass = 2;
		body.sharedMaterial.bounciness = 1.5f;
		Vector3 force = new Vector2(1,1) * velocity.x * 100;
        body.AddForce(force);
		//Debug.Log("velocity");
		//Debug.Log(Mathf.Abs(body.velocity.x));
		//Debug.Log(body.velocity.x);
		anim.SetTrigger("charged");
		charged = true;
    }

	void disableBall(){
		body.sharedMaterial.bounciness = 0;
		body.velocity = new Vector2 (0,0);
		body.mass = 500;
		charged = false;
		chargeTime = 5.0f;
		anim.ResetTrigger("charged");
	}
	void OnTriggerEnter2D(Collider2D collider){
		if ( collider.tag == "projectile" ) {
			Move(5.0f, collider.gameObject.GetComponent<Rigidbody2D>().velocity);
			Destroy(collider.gameObject);
		}
	}
}
