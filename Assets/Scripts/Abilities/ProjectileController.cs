using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	//private int SPEED = 20;
	private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Shoot (Vector2 direction, float speed) {
		GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;
	}
}
