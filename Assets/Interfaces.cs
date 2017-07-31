using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPlayer_Move
{
    void left();
    void right();
    void jump();
    //void left();
}
public interface IMagnetic
{
    Magnetic_Object magnetic_object
    {
        get;
    }
}
