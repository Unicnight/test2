using UnityEngine;
using System.Collections.Generic;
using System;

public class Scene_Initialize : Base_Behaviour
{
    public GameObject[] _game_objects;
    private List<Base_Behaviour> _base_behaviours;

    public override bool initialize()
    {
        this._base_behaviours = new List<Base_Behaviour>();
        try
        {
            foreach (GameObject g in this._game_objects)
            {
                Base_Behaviour[] bbb = g.GetComponents<Base_Behaviour>();
                _base_behaviours.AddRange(bbb);
            }
        }
        catch
        {
            return false;
        }
        this.fixed_updating = true;
        return true;
    }
    private void Start()
    {
        this.initialize();
    } 
    // Update is called once per frame
    protected override void custom_update()
    {
        throw new NotImplementedException();
    }

    protected override void custom_fixed_update()
    {
        bool initialized = true;
        foreach (Base_Behaviour b in this._base_behaviours)
        {
            initialized &= b.initialize();
        }
        if (initialized)
        {
            foreach (Base_Behaviour b in this._base_behaviours)
            {
                b.fixed_updating = true;
            }
            Destroy(this);
        }
    }
}
