using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Line
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
