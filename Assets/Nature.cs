using UnityEngine;
using System.Collections;

public class Nature : MonoBehaviour {
	public GameObject ball_1;
	public GameObject ball_2;
	public float k=0.1f;
	public float n=0.1f;
	private Rigidbody2D Rigidbody_1;
	private Rigidbody2D Rigidbody_2;
	private bool b1, b2;


	float distance;
	Vector2 force;
	float angle;
	// Use this for initialization
	void Start () {
		Rigidbody_1 = ball_1.GetComponent<Rigidbody2D> ();
		Rigidbody_2 = ball_2.GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void FixedUpdate () {
		distance = Vector2.Distance (Rigidbody_1.position, Rigidbody_2.position);
		//b1 = ball_1.GetComponent<Move_1> ().b;
		b2 = ball_2.GetComponent<Move> ().b;

		if (distance > 1) {
			if (b1 == b2) {
				force = (Rigidbody_1.position - Rigidbody_2.position).normalized / distance / distance * k;
				angle = Vector2.Angle (force, new Vector2 (0, 1));
                Vector2 f=new Vector2();
                //if(angle<11.25)
                //{
                //    f = new Vector2(0, 1);
                //}else if(angle >= 168.75)
                //{
                //    f = new Vector2(0, -1);
                //}else
                //{
                //    int i = (int)((angle - 11.25) / 22.5);
                //    switch (i)
                //    {
                //        case 0:
                //            f = new Vector2(force.x/Mathf.Abs(force.x), 2.4142f).normalized;
                //            break;
                //        case 1:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), 1f).normalized;
                //            break;
                //        case 2:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), 0.4142f).normalized;
                //            break;
                //        case 3:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), 0f).normalized;
                //            break;
                //        case 4:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), -0.4142f).normalized;
                //            break;
                //        case 5:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), -1f).normalized;
                //            break;
                //        case 6:
                //            f = new Vector2(force.x / Mathf.Abs(force.x), -2.4142f).normalized;
                //            break;
                //    }
                //}
                f = new Vector2(force.x / Mathf.Abs(force.x)* Mathf.Sin(Mathf.Round(angle * 0.044f) * 0.3927f), Mathf.Cos(Mathf.Round(angle* 0.044f)*0.3927f)).normalized;
                force = f;
                //Rigidbody_1.AddForce(f / distance / distance * k);
                //Rigidbody_2.AddForce(-f / distance / distance * k);
                //Rigidbody_1.gravityScale = 1f - n / distance / distance;

                AreaEffector2D a = new AreaEffector2D();
            } else {
				force = (Rigidbody_2.position - Rigidbody_1.position).normalized / distance / distance * k;
				Rigidbody_1.AddForce ((Rigidbody_2.position - Rigidbody_1.position).normalized / distance / distance * k);
				Rigidbody_2.AddForce ((Rigidbody_1.position - Rigidbody_2.position).normalized / distance / distance * k);
				Rigidbody_1.gravityScale = 1f - n / distance / distance;
			}
//			print (distance);
		}
	}
	void OnGUI () {
		GUI.TextArea (new Rect (new Vector2 (0, 0), new Vector2 (500, 50)), distance.ToString ());
		GUI.TextArea (new Rect (new Vector2 (0, 50), new Vector2 (500, 50)), Mathf.Round(angle * 0.044f).ToString ());
		GUI.TextArea (new Rect (new Vector2 (0, 100), new Vector2 (500, 50)), force.ToString ());
		GUI.TextArea (new Rect (new Vector2 (0, 150), new Vector2 (50, 50)), force.magnitude.ToString ());
		GUI.TextArea (new Rect (new Vector2 (0, 200), new Vector2 (50, 50)), angle.ToString ());
        GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
    }
}
