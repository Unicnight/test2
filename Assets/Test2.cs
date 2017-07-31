using UnityEngine;
using System.Collections;
using System;

public class Test2 : MonoBehaviour {
    public bool a = false;
    public bool b = false;
    private RaycastHit2D _hit;
    private bool j = false;
    private float t = 0;
    public float tttt;
    public bool show_debug = false;
    public bool jj = true;

    private Players _player;

    // Use this for initialization
    void Start()
    {
        this._player = Player_Info._Player_1;
    }
    void FixedUpdate()
    {
        jump();
    }
    void jump()
    {
        try
        {
            _hit = Physics2D.CircleCastAll(this.gameObject.GetComponent<Rigidbody2D>().position, 0.8f, new Vector2(0, 0))[1];
        }
        catch
        {
            _hit = new RaycastHit2D();
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            t = 0;
            j = false;
            return;
        }
        if (_hit.collider != null || j)
        {
            jj = true;
        }
        else
        {
            j = false;
            jj = false;
            return;
        }
        if (jj)
        {
      
                if (Input.GetKey(KeyCode.Space))
                {
                    if (t < tttt)
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100));
                        t += Time.deltaTime;
                        j = true;
                    }else
                    {
                        j = false;
                        jj = false;
                    }
                }
              
        }
        print(t.ToString());
    }
    void OnGUI()
    {
        if (this.show_debug)
        {
            GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), this._player.control.Jump_Key().ToString());
            GUI.TextArea(new Rect(new Vector2(0, 50), new Vector2(500, 50)), _hit.normal.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 100), new Vector2(500, 50)), j.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 150), new Vector2(50, 50)), magnetic_object.d4.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), gameObject.GetComponent<CircleCollider2D>().radius.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
        }
    }
}
