using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public abstract class Magnetic_Object
{

    public Vector2 d1;
    public float d2;
    public float d3;
    public float d4;




    public class Pole
    {
        public static readonly int N = 1;
        public static readonly int S = -1;

        private int _state;
        public int State
        {
            private set {
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
            if (this.State == Pole.N) this.State = Pole.S;
            if (this.State == Pole.S) this.State = Pole.N;
            return this.State;
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
    public abstract class Effective_Range { }

    private Magnetic_Object[] _magnetic_objects;
    private int _magnet_num;
    private Vector2 _force;
    private Pole _pole;
    private GameObject _game_object;
    private Rigidbody2D _rigidbody;
    protected Effective_Range _effective_range;

    public Vector2 force
    {
        set { this._force = value; }
        get { return this._force; }
    }
    public Pole pole
    {
        private set { this._pole = value; }
        get { return this._pole; }
    }
    public GameObject game_object
    {
        private set { this._game_object = value; }
        get { return this._game_object; }
    }
    public Rigidbody2D rigidbody
    {
        private set { this._rigidbody = value; }
        get { return this._rigidbody; }
    }
    public Effective_Range effective_range
    {
        private set { this._effective_range = value; }
        get { return this._effective_range; }
    }

    private delegate int Effects_Handler();//定义委托
    private Effects_Handler effects;

    public Magnetic_Object(Effective_Range effective_range, int state = 1)
    {
        try
        {
            this.rigidbody = this.game_object.GetComponent<Rigidbody2D>();
        }
        catch
        {

        } 
        this.effective_range = effective_range;
        this.pole = new Pole(state);
        //MonoBehaviour.print("inited");
    }    
    protected int calculate_effects()
    {
        int n = 0;
        this.force = Vector2.zero;
        while (n < this._magnet_num)
        {
            try
            {
                this._magnetic_objects[n].magnetic_effect(this);
            }
            catch
            {
                return n;
            }
            n++;
        }
        //this.rigidbody.AddForce(this.force);
        return -1;
    }
    protected abstract bool magnetic_effect(Magnetic_Object magnetic_object);
    
    public int Processing(GameObject game_object)
    {
        try
        {
            try
            {
                this.rigidbody = game_object.GetComponent<Rigidbody2D>();
            }
            catch
            {
                return 2;
            }
            effects();
        }catch
        {
            return 1;
        }
        return -1;
    }
    public bool add_magnetic_objects(Magnetic_Object[] magnetic_objects)
    {
        try
        {
            this._magnetic_objects = magnetic_objects;
            this._magnet_num = this._magnetic_objects.Length;
            if (this._magnet_num > 0) effects += calculate_effects;
        }
        catch
        {
            return false;
        }
        //MonoBehaviour.print("added");
        return true;
    }
    

}
public class Magnetic_Sector : Magnetic_Object
{
    public class Sector_Effective_Range : Effective_Range
    {
        private Vector2 _start_edge;
        private Vector2 _end_edge;
        private float _max_radius;
        private float _min_radius;
        public Vector2 centre { set; get; }
        public Vector2 start_edge
        {
            set { this._start_edge = value.normalized; }
            get { return this._start_edge; }
        }
        public Vector2 end_edge
        {
            set { this._end_edge = value.normalized; }
            get { return this._end_edge; }
        }
        public float max_radius
        {
            set
            {
                if (value < this._min_radius)
                {
                    this._max_radius = this._min_radius + 0.1f;
                }
                else this._max_radius = value;
            }
            get { return this._max_radius; }
        }
        public float min_radius
        {
            set
            {
                if (this._min_radius < 0)
                {
                    this._min_radius = 0f;
                }
                else this._min_radius = value;
            }
            get { return this._min_radius; }
        }
        public float magnitude { set; get; }
        public float k { set; get; }
        public float precision { set; get; }
        public float resolation { set; get; }
        public bool minor_arc { set; get; }
        public bool sync { set; get; }

        public Sector_Effective_Range(Vector2 centre, Vector2 start_edge, Vector2 end_edge, float min_radius = 0f, float max_radius = 1f, float magnitude = 1f, float precision = 0.3927f, bool minor_arc = true,bool sync=false)
        {
            this.centre = centre;
            this.start_edge = new Vector2 (0,1);
            this.end_edge = new Vector2(0, 1);
            this.min_radius = min_radius;
            this.max_radius = max_radius;
            this.magnitude = magnitude;
            this.k = this.magnitude / (1 - this.min_radius * this.min_radius / this.max_radius / this.max_radius);
            this.precision = precision;
            this.resolation = Mathf.Deg2Rad / precision;
            if (start_edge == end_edge)
            {
                this.minor_arc = false;
            }
            else this.minor_arc = minor_arc;
            this.sync = sync;
            //MonoBehaviour.print("inited1");
        }
        public Vector2 sync_centre(Vector2 centre)
        {
            if(this.sync)
                this.centre = centre;
            return this.centre;
        }
    }

    public new Sector_Effective_Range effective_range
    {
        private set { base._effective_range = value; }
        get { return (Sector_Effective_Range)base._effective_range; }
    }

    public Magnetic_Sector(Sector_Effective_Range sector_effective_range, int state = 1) : base(sector_effective_range, state)
    {
    }
    protected override bool magnetic_effect(Magnetic_Object magnetic_object)
    {
        this.effective_range.sync_centre(this.rigidbody.position);
        float distance = Vector2.Distance(this.effective_range.centre, magnetic_object.rigidbody.position);
        Vector2 direction = (magnetic_object.rigidbody.position - this.effective_range.centre).normalized * this.pole * magnetic_object.pole;
        float angle = Vector2.Angle(direction, new Vector2(0, 1));
        float sign;
        if (direction.x != 0)
        {
             sign = direction.x / Mathf.Abs(direction.x);
        }else
        {
            sign = 1;
        }
        float angle_range= Vector2.Angle(this.effective_range.start_edge, this.effective_range.end_edge);
        if (distance > this.effective_range.max_radius)
        {
            return false;
        } else
        {
            if (this.effective_range.minor_arc) {
                if (Vector2.Angle(direction, this.effective_range.start_edge) > angle_range || Vector2.Angle(direction, this.effective_range.end_edge) > angle_range)
                    return false;
            }else 
            {
                if (Vector2.Angle(direction, this.effective_range.start_edge) < angle_range && Vector2.Angle(direction, this.effective_range.end_edge) < angle_range)
                    return false;
            }
        }
        direction = new Vector2(sign * Mathf.Sin(Mathf.Round(angle * this.effective_range.resolation) * this.effective_range.precision), Mathf.Cos(Mathf.Round(angle * this.effective_range.resolation) * this.effective_range.precision)).normalized;
        magnetic_object.force += this.effective_range.k * (1 - distance * distance / this.effective_range.max_radius / this.effective_range.max_radius) * direction;
        magnetic_object.d1 = direction;
        magnetic_object.d2 = this.effective_range.k;
        magnetic_object.d3 = sign;
        magnetic_object.d4 = distance;
        return true;        
    }
}