using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed = 0.4f;
    public float FForce = 20f;
    public float r = 1f;
    public float d = 1f;
    public float v = 4f;
    Vector2 dest = Vector2.zero;
    private Rigidbody2D Rigidbody;


    public bool b = false;
    private bool bb = false;
    // Use this for initialization
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //
        //
        //		//通过Rigidbody2D，实现Pacman的移动
        //		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        //
        //		GetComponent<Rigidbody2D>().MovePosition(p);
        //
        //		//收集输入信息，通过Valid方法，计算移动距离
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Rigidbody.AddForce(new Vector2(0, 50));
            print("!");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            walk(new Vector2(10, 0));
            print(">");
        }
        if (Input.GetKey(KeyCode.LeftArrow))

        {
            walk(new Vector2(-10, 0));
            print("<");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!bb)
            {
                b = !b;
                Rigidbody.AddForce(new Vector2(0, 0));
                print("@");
                bb = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            bb = false;
        }
        if (!Input.anyKey)
        {
            stand(new Vector2 (0,0));
        }
    }
    private void walk(Vector2 force)
    {
        if (force.x / Rigidbody.velocity.x < 0) if (!stand(force)) return;
        if (Mathf.Abs(Rigidbody.velocity.x) < 4)
        {
            Rigidbody.AddForce(force);
        }
    }
    private bool stand(Vector2 force)
    {
        if (Mathf.Abs(Rigidbody.velocity.x) > 0.2)
        {
            Rigidbody.AddForce(new Vector2(-Rigidbody.velocity.x, 0).normalized * (FForce));
            return false;
        }
        else
        {
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        }
        return true;
    }
    void OnGUI()
    {
        GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), Rigidbody.velocity.x.ToString());
    }
}
