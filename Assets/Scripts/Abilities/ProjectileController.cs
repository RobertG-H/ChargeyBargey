using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	private Rigidbody2D rigidbody;
    public float damage;
    public float playerNum;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    
	public void Shoot (Vector2 direction, float speed, float duration, float charge, int player, float xdirection) {
		GetComponent<SpriteRenderer>().flipX = xdirection < 0;
		GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;
		tag = "projectile";
        damage = charge;
        playerNum = player;
		Destroy(gameObject, duration);
	}
}
