using UnityEngine;
using System.Collections;

public class Move_1 : MonoBehaviour {
	public float speed = 0.4f;
	public float force = 20f;
	public float r = 1f;
	public float d = 1f;
	public bool b=false;
	Vector2 dest = Vector2.zero;
	private Rigidbody2D Rigidbody;

	// Use this for initialization
	void Start () {
		Rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	void FixedUpdate () {
//
//
//		//通过Rigidbody2D，实现Pacman的移动
//		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
//
//		GetComponent<Rigidbody2D>().MovePosition(p);
//
//		//收集输入信息，通过Valid方法，计算移动距离
		if (Input.GetKey (KeyCode.W)) 
		{
			Rigidbody.AddForce (new Vector2 (0, 50));
			print ("!");
		}

		if (Input.GetKey (KeyCode.D)) 
		{

			Rigidbody.AddForce (new Vector2 (10, 0));
			print (">");
		}


		if (Input.GetKey (KeyCode.S)) 
		{
			b = !b;
			Rigidbody.AddForce (new Vector2 (0, 0));
			print ("@");
		}

		if (Input.GetKey (KeyCode.A)) 
		{

			Rigidbody.AddForce (new Vector2 (-10, 0));
			print ("<");
		}
	}
//
//	void OnTriggerEnter2D(Collider2D co)
//	{
//		print("asd");
//			Rigidbody.AddForce (new Vector2 (0, force));
//	}

}
