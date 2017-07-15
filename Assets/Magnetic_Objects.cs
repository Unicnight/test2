using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public abstract class Magnetic_Object {   
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

    private delegate int Effects_Handler();//定义委托
    private Effects_Handler effects;

    public Magnetic_Object(ref GameObject game_object, int state = 1)
    {
        this.game_object = game_object;
        this.pole = new Pole(state);
    }    
    protected int calculate_effects()
    {
        int n = 0;
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
        return -1;
    }
    protected abstract void magnetic_effect(Magnetic_Object magnetic_object);

    public int Processing()
    {
        try
        {
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
public class Magnetic_Ball : Magnetic_Object
{
    public Magnetic_Ball(ref GameObject game_object, int state = 1) : base(ref game_object, state)
    {
    }
    protected override void magnetic_effect(Magnetic_Object magnetic_object)
    {
        int i = 0;
    }
        //    if (magnetic_object.rigidbody == null) { MonoBehaviour.print("Rigidbody2D is null!");return; }

        //    float distance = Vector2.Distance(magnetic_object.rigidbody.position, this.rigidbody.position);
        //    Vector2 direction = (magnetic_object.rigidbody.position - this.rigidbody.position).normalized * (magnetic_object.pole * this.pole);
        //    int sign = (int)(direction.x / Mathf.Abs(direction.x));
        //    if (distance > 0)
        //    {
        //        float angle = Vector2.Angle(direction, new Vector2(0, 1));
        //        Vector2 f = new Vector2();
        //        if (angle < 11.25)
        //        {
        //            f = new Vector2(0, 1);
        //        }
        //        else if (angle >= 168.75)
        //        {
        //            f = new Vector2(0, -1);
        //        }
        //        else
        //        {
        //            int i = (int)((angle - 11.25) / 22.5);
        //            switch (i)
        //            {
        //                case 0:
        //                    f = new Vector2(sign, 2.4142f).normalized;
        //                    break;
        //                case 1:
        //                    f = new Vector2(sign, 1f).normalized;
        //                    break;
        //                case 2:
        //                    f = new Vector2(sign, 0.4142f).normalized;
        //                    break;
        //                case 3:
        //                    f = new Vector2(sign, 0f).normalized;
        //                    break;
        //                case 4:
        //                    f = new Vector2(sign, -0.4142f).normalized;
        //                    break;
        //                case 5:
        //                    f = new Vector2(sign, -1f).normalized;
        //                    break;
        //                case 6:
        //                    f = new Vector2(sign, -2.4142f).normalized;
        //                    break;
        //            }
        //        }
        //            force = f;
        //            Rigidbody_1.AddForce(f / distance / distance * k);
        //            Rigidbody_2.AddForce(-f / distance / distance * k);
        //            Rigidbody_1.gravityScale = 1f - n / distance / distance;
        //        }
        //        else
        //        {
        //            force = (Rigidbody_2.position - Rigidbody_1.position).normalized / distance / distance * k;
        //            Rigidbody_1.AddForce((Rigidbody_2.position - Rigidbody_1.position).normalized / distance / distance * k);
        //            Rigidbody_2.AddForce((Rigidbody_1.position - Rigidbody_2.position).normalized / distance / distance * k);
        //            Rigidbody_1.gravityScale = 1f - n / distance / distance;
        //        }
        //        //			print (distance);
        //    }
        //}

    }