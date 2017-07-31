using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//基础类

    //还要修改，添加扇形基类

public class Pole   //极性
{
    public const int N = 1;
    public const int S = -1;
    public const int Null = 0;

    private int _state;
    public int State
    {
        private set
        {
            if (value == 1 || value == -1)
            {
                this._state = value;
            }
            else
            {
                this._state = 1;
            }
        }
        get { return this._state; }
    }
    public Pole()
    {
        this.State = 1;
    }
    public Pole(int state)
    {
        this.State = state;
    }
    public int Reverse()
    {
        if (this.State == Pole.N)
        {
            this.State = Pole.S;
            return this.State;
        }
        if (this.State == Pole.S)
        {
            this.State = Pole.N;
            return this.State;
        }
        return this.State;
    }
    public bool is_Null
    {
        set
        {
            if (value) this._state = 0;
            else this._state = N;
        }
    }
    public override string ToString()
    {
        switch (this._state)
        {
            case N:
                return "N";
            case S:
                return "S";
            case Null:
                return "Null";
        }
        return "";
    }

    public static Pole operator !(Pole pole)
    {
        if (pole.State == Pole.N) pole.State = Pole.S;
        if (pole.State == Pole.S) pole.State = Pole.N;
        return pole;
    }
    public static Vector2 operator *(Pole pole, Vector2 vector)
    {
        return (float)pole.State * vector;
    }
    public static Vector2 operator *(Vector2 vector, Pole pole)
    {
        return vector * (float)pole.State;
    }
    public static int operator *(Pole pole_1, Pole pole_2)
    {
        return pole_1.State * pole_2.State;
    }
}

public class Line   //线形
{
    private Vector2 _start_point = new Vector2(0, 0);
    private Vector2 _end_point = new Vector2(0, 0);
    private Vector2 _centre = new Vector2(0, 0);
    private Vector2 _normal_vector;
    private Vector2 _tangent_vector;
    private float _length;
    private float _rotation;
    private bool _opposite;

    public Vector2 start_point
    {
        set
        {
            this._start_point = value;
            this.tangent_vector = this.end_point - this.start_point;
            this.centre = (this.start_point + this.end_point) / 2;
            if (!this.opposite) this.normal_vector = new Vector2(-this.tangent_vector.y, this.tangent_vector.x);
            else this.normal_vector = new Vector2(this.tangent_vector.y, -this.tangent_vector.x);
            this.length = Vector2.Distance(this.start_point, this.end_point);
        }
        get { return this._start_point; }
    }
    public Vector2 end_point
    {
        set
        {
            this._end_point = value;
            this.tangent_vector = this.end_point - this.start_point;
            this.centre = (this.start_point + this.end_point) / 2;
            if (!this.opposite) this.normal_vector = new Vector2(-this.tangent_vector.y, this.tangent_vector.x);
            else this.normal_vector = new Vector2(this.tangent_vector.y, -this.tangent_vector.x);
            this.length = Vector2.Distance(this.start_point, this.end_point);
        }
        get { return this._end_point; }
    }
    public Vector2 centre
    {
        set
        {
            Vector2 temp = value - this.centre;
            this.start_point = this.start_point + temp;
            this.end_point = this.end_point + temp;
            this._centre = value;
        }
        get { return this._centre; }
    }
    public Vector2 normal_vector
    {
        private set { this._normal_vector = value.normalized; }
        get { return this._normal_vector; }
    }
    public Vector2 tangent_vector
    {
        private set { this._tangent_vector = value.normalized; }
        get { return this._tangent_vector; }
    }
    public float length
    {
        private set
        {
            this._length = value;
        }
        get { return this._length; }
    }
    public float rotation
    {
        set { this._rotation = value;
            Vector2 temp_start = this.start_point - this.centre;
            Vector2 temp_end = this.end_point - this.centre;
            temp_start= Quaternion.AngleAxis(value, new Vector3(0, 0, 1)) * temp_start;
            temp_end = Quaternion.AngleAxis(value, new Vector3(0, 0, 1)) * temp_end;
            this.start_point = temp_start + this.centre;
            this.end_point = temp_end + this.centre;
        }
        get { return this._rotation; }
    }
    public bool opposite
    {
        set
        {
            this._opposite = value;
            if (!value) this.normal_vector = new Vector2(-this.tangent_vector.y, this.tangent_vector.x);
            else this.normal_vector = new Vector2(this.tangent_vector.y, -this.tangent_vector.x);
        }
        get { return this._opposite; }
    }
    public Line(Vector2 start_point,Vector2 end_point,bool opposite =false)
    {
        this.start_point = start_point;
        this.end_point = end_point;
        this.opposite = opposite;
    }
    public static float Distance(Vector2 point,Line line)
    {
        Vector2 v1 = point - line.start_point;
        Vector2 v2 = line.end_point - line.start_point;
        return Vector2.Dot(v1, v2) / v2.magnitude;
    }
    public static bool On_Line(Vector2 point, Line line)
    {
        Vector2 temp_v = (line.start_point - point).normalized;
        if (temp_v == line.tangent_vector || temp_v == -line.tangent_vector) return true;
        else return false;
    }
    public static bool Inside_Line(Vector2 point, Line line)
    {
        Vector2 temp_v = (point - line.start_point).normalized;
        if (temp_v == line.tangent_vector)
            if (temp_v.magnitude <= line.length) return true;
            else return false;
        else return false;
    }
}

public class Controls   //控制
{
    private KeyCode JUMP_KEY;
    private KeyCode LEFT_KEY;
    private KeyCode RIGHT_KEY;
    private KeyCode POLE_KEY;

    public bool initialized { private set; get; }

    public Controls() { }
    public void initialize()    //need modify!!
    {
        JUMP_KEY = KeyCode.W;
        LEFT_KEY = KeyCode.A;
        RIGHT_KEY = KeyCode.D;
        POLE_KEY = KeyCode.S;
        this.initialized = true;
    }
    public void initialize(KeyCode jump, KeyCode left, KeyCode right,KeyCode pole)    //need modify!!
    {
        JUMP_KEY = jump;
        LEFT_KEY = left;
        RIGHT_KEY = right;
        POLE_KEY = pole;
        this.initialized = true;
    }
    public bool Jump_Key_Down()
    {
        return Input.GetKeyDown(JUMP_KEY);
    }
    public bool Left_Key_Down()
    {
        return Input.GetKeyDown(LEFT_KEY);
    }
    public bool Right_Key_Down()
    {
        return Input.GetKeyDown(RIGHT_KEY);
    }
    public bool Pole_Key_Down()
    {
        return Input.GetKeyDown(POLE_KEY);
    }
    public bool Jump_Key()
    {
        return Input.GetKey(JUMP_KEY);
    }
    public bool Left_Key()
    {
        return Input.GetKey(LEFT_KEY);
    }
    public bool Right_Key()
    {
        return Input.GetKey(RIGHT_KEY);
    }
    public bool Pole_Key()
    {
        return Input.GetKey(POLE_KEY);
    }
    public bool Jump_Key_Up()
    {
        return Input.GetKeyUp(JUMP_KEY);
    }
    public bool Left_Key_Up()
    {
        return Input.GetKeyUp(LEFT_KEY);
    }
    public bool Right_Key_Up()
    {
        return Input.GetKeyUp(RIGHT_KEY);
    }
    public bool Pole_Key_Up()
    {
        return Input.GetKeyUp(POLE_KEY);
    }
}

public class Players    //玩家类
{
    private Controls _control;

    public Controls control
    {
        get { return this._control; }
    }
    public bool initialized { private set; get; }

    public Players() {
        this._control = new Controls();
    }
    public void initialize()
    {
        this._control.initialize();
        this.initialized = true;
    }
    public void initialize(KeyCode jump,KeyCode left,KeyCode right,KeyCode pole)
    {
        this._control.initialize(jump,left,right,pole);
        this.initialized = true;
    }
    public void sync() { }
}

public abstract class Base_Behaviour    //脚本基类，还要修改，可能还要添加一个初始化函数
    : MonoBehaviour
{
    private bool _updating = false;
    private bool _fixed_updating = false;

    public bool updating
    {
        set
        {
            if (value != this._updating)
            {
                if (value) update_delegate += custom_update;
                else update_delegate -= custom_update;
                this._updating = value;
            }
        }
        get { return this._updating; }
    }
    public bool fixed_updating
    {
        set
        {
            if (value != this._fixed_updating)
            {
                if (value) this.fixed_update_delegate += this.custom_fixed_update;
                else this.fixed_update_delegate -= this.custom_fixed_update;
                this._fixed_updating = value;
            }
        }
        get { return this._fixed_updating; }
    }

    protected delegate void Update_Delegate();//定义委托
    protected  delegate void Fixed_Update_Delegate();//定义委托
    protected Update_Delegate update_delegate;
    protected Update_Delegate fixed_update_delegate;

    private void Update()
    {
        try
        {
            this.update_delegate();
        }
        catch
        {

        }
    }
    private void FixedUpdate()
    {
        try
        {
            this.fixed_update_delegate();
        }
        catch
        {

        }
    }

    public abstract bool initialize();
    protected abstract void custom_update();
    protected abstract void custom_fixed_update();
}

