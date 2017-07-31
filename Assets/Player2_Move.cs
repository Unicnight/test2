using UnityEngine;
using System.Collections;
using System;

public class Player2_Move : Base_Behaviour,IMagnetic
{
    private Player_Move _player_move;
    private Rigidbody2D _rigidbody;
    public GameObject[] _game_objects;
    public bool show_debug = false;
    private Magnetic_Object _magnetic_object = new Magnetic_Sector(
            new Magnetic_Sector.Sector_Effective_Range(new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), 1, 10, 10, sync: true)
            );
    public Magnetic_Object magnetic_object
    {
        private set { this._magnetic_object = value; }
        get { return this._magnetic_object; }
    }

    
    public override bool initialize()
    {

        System.Collections.Generic.List<Magnetic_Object> magnetic_objects = null; ;
        try
        {
            magnetic_objects = new System.Collections.Generic.List<Magnetic_Object>();

            foreach (GameObject g in _game_objects)
            {
                magnetic_objects.Add(g.GetComponent<IMagnetic>().magnetic_object);
            }

            this._player_move = new Player_Move(Player_Info._Player_2, gameObject, this);
            this._rigidbody = GetComponent<Rigidbody2D>();
            //Player_Info._Player_1.initialize();
        }catch
        {
            return false;
        }
        return true;
    }
    protected override void custom_update()
    {
        throw new NotImplementedException();
    }

    protected override void custom_fixed_update()
    {
        this._player_move.Processing(this.gameObject);
        this._magnetic_object.Processing(this.gameObject);
        this._rigidbody.AddForce(this._player_move.force + this._magnetic_object.force);
    }

    void OnGUI()
    {
        if (this.show_debug)
        {
            //GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), magnetic_object.d2.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 50), new Vector2(500, 50)), magnetic_object.d1.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 100), new Vector2(500, 50)), magnetic_object.d3.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 150), new Vector2(50, 50)), magnetic_object.d4.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), gameObject.GetComponent<CircleCollider2D>().radius.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
        }
    }
}
