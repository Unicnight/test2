﻿using UnityEngine;
using System.Collections;
using System;

public class Player1_Move : Base_Behaviour,IMagnetic
{
    public float jump_time;
    public float jump_force;
    public float walk_force;
    public float stop_force;
    public float max_velocity;
    public float max_radius;
    public float min_radius;
    public float magnitude;


    private Player_Move _player_move;
    private Rigidbody2D _rigidbody;
    public GameObject[] _game_objects;
    public bool _show_debug = false;
    private Magnetic_Sector _magnetic_sector = new Magnetic_Sector(
            new Magnetic_Sector.Sector_Effective_Range(new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), 1, 10, 10, sync: true)
            );
    public Magnetic_Object magnetic_object
    {
        get { return this._magnetic_sector; }
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

            this._player_move = new Player_Move(Player_Info._Player_1, gameObject, this);
            this._rigidbody = GetComponent<Rigidbody2D>();
            //Player_Info._Player_1.initialize();
        }catch
        {
            return false;
        }
        this._magnetic_sector.add_magnetic_objects(magnetic_objects);
        return true;
    }
    protected override void custom_update()
    {
        throw new NotImplementedException();
    }

    protected override void custom_fixed_update()
    {
        this._player_move.jump_time = jump_time;
        this._player_move.jump_force = jump_force;
        this._player_move.walk_force = walk_force;
        this._player_move.stop_force = stop_force;
        this._player_move.max_velocity = max_velocity;
        this._magnetic_sector.effective_range.min_radius = min_radius;
        this._magnetic_sector.effective_range.max_radius = max_radius;
        this._magnetic_sector.effective_range.magnitude = magnitude;

        this._player_move.Processing(this.gameObject);
        this._magnetic_sector.Processing(this.gameObject);
        Vector2 fff = this._player_move.force + this._magnetic_sector.force;
        this._rigidbody.AddForce(fff);
        if (Player_Info._Player_1.control.Pole_Key_Up())
        {
            this._magnetic_sector.pole.Reverse();
        }
    }

    void OnGUI()
    {
        if (this._show_debug)
        {
            GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), magnetic_object.pole.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 50), new Vector2(500, 50)), magnetic_object.d1.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 100), new Vector2(500, 50)), magnetic_object.d3.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 150), new Vector2(50, 50)), magnetic_object.d4.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), gameObject.GetComponent<CircleCollider2D>().radius.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
        }
    }
}
