using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
    public Vector2 f=new Vector2 (0,100);
    // Use this for initialization
    void Start()
    {
        this.GetComponent<Rigidbody2D>().AddForce(f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<Rigidbody2D>().AddForce(f);
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(10,0));
            print("a");
        }
    }
}
