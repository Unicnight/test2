using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public abstract class Magnetic_Object
{   
    public class Pole
    {
        public static readonly int N = 1;
        public static readonly int S = -1;
        public int State
        {
            private set {
                if (value == 1 || value == -1)
                {
                    this.State = value;
                }
                else
                {
                    this.State = 1;
                }
            }
            get { return this.State; }
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

    private Magnetic_Object[] magnetic_objects;
    private int magnet_num;

    public Vector2 force
    {
        set { this.force = value; }
        get { return this.force; }
    }
    public Pole pole
    {
        private set { this.pole = value; }
        get { return this.pole; }
    }
    public GameObject game_object
    {
        private set { this.game_object = value; }
        get { return this.game_object; }
    }
    public Rigidbody2D rigidbody
    {
        private set { this.rigidbody = value; }
        get { return this.rigidbody; }
    }
    public Effective_Range effective_range
    {
        private set { this.effective_range = value; }
        get { return this.effective_range; }
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
    }    
    protected int calculate_effects()
    {
        int n = 0;
        this.force = Vector2.zero;
        while (n < this.magnet_num)
        {
            try
            {
                this.magnetic_objects[n].magnetic_effect(this);
            }
            catch
            {
                return n;
            }
            n++;
        }
        this.rigidbody.AddForce(this.force);
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
            this.magnetic_objects = magnetic_objects;
            this.magnet_num = this.magnetic_objects.Length;
            if (this.magnet_num > 0) effects += calculate_effects;
            return true;
        }
        catch
        {
            return false;
        }
    }
    

}
public class Magnetic_Sector : Magnetic_Object
{
    public class Sector_Effective_Range : Effective_Range
    {
        public Vector2 centre { set; get; }
        public Vector2 start_edge { set; get; }
        public Vector2 end_edge { set; get; }
        public float max_radius
        {
            set
            {
                if (this.max_radius < this.min_radius)
                {
                    this.max_radius = this.min_radius + 0.1f;
                }
                else this.max_radius = value;
            }
            get { return this.max_radius; }
        }
        public float min_radius
        {
            set
            {
                if (this.min_radius < 0)
                {
                    this.min_radius = 0f;
                }
                else this.max_radius = value;
            }
            get { return this.max_radius; }
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
            this.start_edge = start_edge;
            this.end_edge = end_edge;
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
        }
        public Vector2 sync_centre(Vector2 centre)
        {
            if(this.sync)
                this.centre = centre;
            return this.centre;
        }
    }

    public Sector_Effective_Range effective_range
    {
        private set { this.effective_range = value; }
        get { return this.effective_range; }
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
        float sign = direction.x / Mathf.Abs(direction.x);
        float angle_range= Vector2.Angle(this.effective_range.start_edge, this.effective_range.end_edge);
        if (distance > this.effective_range.max_radius)
        {
            return false;
        } else
        {
            if (Vector2.Angle(direction, this.effective_range.start_edge) > angle_range || Vector2.Angle(direction, this.effective_range.end_edge) > angle_range)
            {
                if (this.effective_range.minor_arc) return false;
            }else
            {
                if (!this.effective_range.minor_arc) return false;
            }
        }
        direction = new Vector2(sign * Mathf.Sin(Mathf.Round(angle * this.effective_range.precision) * this.effective_range.precision), Mathf.Cos(Mathf.Round(angle * this.effective_range.precision) * this.effective_range.precision)).normalized;
        this.force += this.effective_range.k * (1 - distance * distance / this.effective_range.max_radius / this.effective_range.max_radius) * direction;
        return true;
        MonoBehaviour.print(direction.ToString());
    }
}