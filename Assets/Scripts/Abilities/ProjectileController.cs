using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	private int SPEED = 10;
	private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Shoot (Vector2 direction) {
		GetComponent<Rigidbody2D>().velocity = SPEED * direction.normalized;
	}
}
