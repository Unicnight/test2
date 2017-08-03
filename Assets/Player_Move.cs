using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player_Move
{
    private bool _jumping;
    private bool _jumpable;
    private Vector2 _offset;
    private GameObject _game_object;
    private RaycastHit2D _hit;
    private Rigidbody2D _rigidbody;
    private Base_Behaviour _base_behaviour;
    private Vector2 _force;
    private Vector2 _velocity;
    private Players _player;
    
    private float _jump_time=0.1f;
    private float _jump_force = 100;
    private float _walk_force = 10;
    private float _stop_force = 20f;
    private float _max_velocity = 4f;

    public float jump_time
    {
        set { this._jump_time = value; }
        get { return this._jump_time; }
    }
    public float jumping_timer { set; get; }
    public float jump_force
    {
        set { this._jump_force = value; }
        get { return this._jump_force; }
    }
    public float walk_force
    {
        set { this._walk_force = value; }
        get { return this._walk_force; }
    }
    public float stop_force
    {
        set { this._stop_force = value; }
        get { return this._stop_force; }
    }
    public Vector2 force
    {
        private set { this._force = value; }
        get { return this._force; }
    }
    public Vector2 velocity
    {
        set { this._velocity = value; }
        get { return this._velocity; }
    }
    public float max_velocity
    {
        set { this._max_velocity = value; }
        get { return this._max_velocity; }
    }

    public Player_Move (Players player, GameObject game_object,Base_Behaviour base_behaviour)
    {
        this._player = player;
        this._game_object = game_object;
        this._offset = new Vector2(0, -game_object.GetComponent<CircleCollider2D>().radius);
        this._base_behaviour = base_behaviour;
    }
    private void move_left()
    {
        if (this._player.control.Left_Key())
        {
            walk(this.walk_force * new Vector2(-1, 0));
        }
    }
    private void move_right()
    {
        if (this._player.control.Right_Key())
        {
            walk(this.walk_force * new Vector2(1, 0));
        }
    }
    private void jump()
    {
        if (this._player.control.Jump_Key_Up())
        {
            this.jumping_timer = 0;
            this._jumping = false;
            return;
        }
        if (this._hit.collider != null || this._jumping)
        {
            this._jumpable = true;
        }
        else
        {
            this._jumping = false;
            this._jumpable = false;
            return;
        }
        if (this._jumpable)
        {

            if (this._player.control.Jump_Key())
            {
                if (this.jumping_timer < this._jump_time)
                {
                    this.force += this.jump_force * new Vector2(0, 1);
                    this.jumping_timer += Time.deltaTime;
                    this._jumping = true;
                }
                else
                {
                    this._jumping = false;
                    this._jumpable = false;
                }
            }

        }
    }
    private void walk(Vector2 force)
    {
        if (force.x / this._rigidbody.velocity.x < 0) if (!stand(force)) return;
        if (Mathf.Abs(this._rigidbody.velocity.x) < _max_velocity)
        {
            this.force += force;
        }
    }
    private bool stand(Vector2 force)
    {
        if (Mathf.Abs(this._rigidbody.velocity.x) > 0.2)
        {
            this.force += new Vector2(-this._rigidbody.velocity.x, 0).normalized * (this._stop_force);
            return false;
        }
        else
        {
            this._velocity = new Vector2(0, this._rigidbody.velocity.y);
        }
        return true;
    }

    public int Processing(GameObject game_object)
    {
        this.force = Vector2.zero;
        try
        {
            try
            {
                this._rigidbody = game_object.GetComponent<Rigidbody2D>();
                this._hit = Physics2D.CircleCastAll(this._rigidbody.position, 0.8f, new Vector2(0, 0))[1]; //还要修改
            }
            catch
            {
                this._hit = new RaycastHit2D();
                //return 2;
            }
            this.jump();
            this.move_left();
            this.move_right();
        }
        catch
        {
            return 1;
        }
        return -1;
    }
}
