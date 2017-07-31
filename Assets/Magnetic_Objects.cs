using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public abstract class Magnetic_Object
{

    public Vector2 d1;
    public float d2;
    public float d3;
    public float d4;




    
    public abstract class Effective_Range { }

    private List<Magnetic_Object> _magnetic_objects;
    private int _magnet_num;
    private Vector2 _force;
    private Pole _pole;
    private GameObject _game_object;
    private Rigidbody2D _rigidbody;
    private bool _effects_actived = false;
    private bool _is_effective;
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
    public bool effects_actived //外力影响
    {
        set
        {
            if (value) this.effects_delegate += calculate_effects;
            else this.effects_delegate -= calculate_effects;
            this._effects_actived = value;
        }
        get { return this._effects_actived; }
    }
    public bool is_effective    //本身影响是否对外起作用
    {
        set {
            if (value) this.effective_delegate += magnetic_effect;
            else this.effective_delegate -= magnetic_effect;
            this._is_effective = value; }
        get { return this._is_effective; }
    }
    public List<Magnetic_Object> magnetic_objects {
        private set { this._magnetic_objects = value; }
        get { return this._magnetic_objects; }
    }
    public int magnet_num
    {
        private set
        {
            if (value > 0) this.effects_actived = true;
            else this.effects_actived = false;
            this._magnet_num = value;
        }
        get { return this._magnet_num; }
    }

    private delegate int Effects_Delegate();//定义委托
    private delegate bool Effective_Delegate(Magnetic_Object magnetic_object);//定义委托
    private Effects_Delegate effects_delegate;
    private Effective_Delegate effective_delegate;

   
    private int calculate_effects()
    {
        int n = 0;
        this.force = Vector2.zero;
        while (n < this.magnet_num)
        {
            try
            {
                this.magnetic_objects[n].effection(this);
            }
            catch (Exception ex)
            {
                MonoBehaviour.print(ex);
                return n;
            }
            n++;
        }
        this.rigidbody.AddForce(this.force);
        return -1;
    }
    protected bool effection(Magnetic_Object magnetic_object)   //应写一个异常
    {
        return effective_delegate(magnetic_object);
    }


    protected abstract bool magnetic_effect(Magnetic_Object magnetic_object);


    public Magnetic_Object(Effective_Range effective_range, int state = 1,bool is_effective=true)
    {
        try
        {
            this.rigidbody = this.game_object.GetComponent<Rigidbody2D>();
        }
        catch
        {

        }
        this.magnetic_objects = new List<Magnetic_Object>();
        this.effective_range = effective_range;
        this.pole = new Pole(state);
        this.is_effective = is_effective;
        //MonoBehaviour.print("inited");
    }
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
            effects_delegate();
            //this.rigidbody.AddForce(this.force);
        }catch
        {
            return 1;
        }
        return -1;
    }
    public bool add_magnetic_objects(List<Magnetic_Object> magnetic_objects)
    {
        try
        {
            this.magnetic_objects.AddRange(magnetic_objects);
            this.magnet_num = this.magnetic_objects.Count;
        }
        catch
        {
            return false;
        }
        return true;
    }
    public bool add_magnetic_object(Magnetic_Object magnetic_object)
    {
        try
        {
            this.magnetic_objects.Add(magnetic_object);
            this.magnet_num = this.magnetic_objects.Count;
        }
        catch
        {
            return false;
        }
        //MonoBehaviour.print("added");
        return true;
    }
    public bool remove_magnetic_object_at(int index)
    {
        try
        {
            this.magnetic_objects.RemoveAt(index);
            this.magnet_num = this.magnetic_objects.Count;
        }
        catch
        {
            return false;
        }
        //MonoBehaviour.print("added");
        return true;
    }
    public bool remove_magnetic_object(Magnetic_Object magnetic_object)
    {
        try
        {
            this.magnetic_objects.Remove(magnetic_object);
            this.magnet_num = this.magnetic_objects.Count;
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
        private float _rotation;

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
        public float rotation
        {
            set
            {
                this.start_edge = Quaternion.AngleAxis(value, new Vector3(0, 0, 1)) * this.start_edge;
                this.end_edge = Quaternion.AngleAxis(value, new Vector3(0, 0, 1)) * this.end_edge;
                this.rotation = value;
            }
            get { return this.rotation; }
        }

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
public class Magnetic_Box : Magnetic_Object
{
    public class Box_Effective_Range : Effective_Range
    {
        private float _max_range;
        private float _min_range;

        private Line _line;

        public Line line { private set; get; }
        public Vector2 start_point
        {
            set
            {
                this._line.start_point = value;
            }
            get { return this._line.start_point; }
        }
        public Vector2 end_point
        {
            set
            {
                this._line.end_point = value;
            }
            get { return this._line.end_point; }
        }
        public Vector2 centre
        {
            get { return this._line.centre; }
        }
        public Vector2 normal_vector
        {
            get { return this._line.normal_vector; }
        }
        public Vector2 tangent_vector
        {
            get { return this._line.tangent_vector; }
        }
        public float length
        {
            get { return this._line.length; }
        }
        public float rotation
        {
            set
            {
                this._line.rotation = value;
            }
            get { return this._line.rotation; }
        }
        public float max_range
        {
            set
            {
                if (value < this._min_range)
                {
                    this._max_range = this._min_range + 0.1f;
                }
                else this._max_range = value;
            }
            get { return this._max_range; }
        }
        public float min_range
        {
            set
            {
                if (this._min_range < 0)
                {
                    this._min_range = 0f;
                }
                else this._min_range = value;
            }
            get { return this._min_range; }
        }
        public float magnitude { set; get; }
        public float k { set; get; }
        public bool opposite
        {
            set { this._line.opposite = value; }
            get { return this._line.opposite; }
        }
        public bool sync { set; get; }

        public Box_Effective_Range(Vector2 start_point, Vector2 end_poinnt, float min_range = 0f, float max_range = 1f, float magnitude = 1f, bool opposite = true, bool sync = false)
        {
            this.start_point = start_point;
            this.end_point = end_poinnt;
            this.min_range = min_range;
            this.max_range = max_range;
            this.magnitude = magnitude;
            this.k = this.magnitude / (1 - this.min_range * this.min_range / this.max_range / this.max_range);
            this.opposite = opposite;
            this.sync = sync;
            //MonoBehaviour.print("inited1");
        }
        public Vector2 sync_centre(Vector2 centre)
        {
            if (this.sync)
                this._line.centre = centre;
            return this.centre;
        }
    }

    public new Box_Effective_Range effective_range
    {
        private set { base._effective_range = value; }
        get { return (Box_Effective_Range)base._effective_range; }
    }

    public Magnetic_Box(Box_Effective_Range line_effective_range, int state = 1) : base(line_effective_range, state)
    {
    }
    protected override bool magnetic_effect(Magnetic_Object magnetic_object)
    {
        this.effective_range.sync_centre(this.rigidbody.position);
        float distance = Line.Distance(magnetic_object.rigidbody.position, this.effective_range.line);
        Vector2 direction = this.effective_range.line.normal_vector * this.pole * magnetic_object.pole; 
        float angle = Vector2.Angle(direction, new Vector2(0, 1));
        if (distance > this.effective_range.max_range)
        {
            return false;
        }
        else
        {
            if (!Line.Inside_Line(magnetic_object.rigidbody.position-distance* this.effective_range.line.normal_vector, this.effective_range.line))
            {
                return false;
            }
        }
        magnetic_object.force += this.effective_range.k * (1 - distance * distance / this.effective_range.max_range / this.effective_range.max_range) * direction;


        magnetic_object.d1 = direction;
        magnetic_object.d2 = this.effective_range.k;
        //magnetic_object.d3 = sign;
        magnetic_object.d4 = distance;


        return true;
    }
}