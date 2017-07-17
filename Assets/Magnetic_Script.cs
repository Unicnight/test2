using UnityEngine;
using System.Collections;

public class Magnetic_Script : MonoBehaviour
{
    public GameObject[] _game_objects;
    public bool show_debug = false;
    private Magnetic_Object _magnetic_object;
    public Magnetic_Object magnetic_object
    {
        private set { this._magnetic_object = value; }
        get { return this._magnetic_object; }
    }
    // Use this for initialization
    void Start()
    {
        _magnetic_object = new Magnetic_Sector(
            new Magnetic_Sector.Sector_Effective_Range(this.gameObject.GetComponent<Rigidbody2D>().position, new Vector2(0, 1), new Vector2(0, 1), 1, 10, 10, sync: true)
            );
        System.Collections.Generic.List<Magnetic_Object> magnetic_objects = new System.Collections.Generic.List<Magnetic_Object>();
        try
        {
            foreach(GameObject _g in _game_objects)
            {
                magnetic_objects.Add(_g.GetComponent<Magnetic_Script>()._magnetic_object);
            }
        }catch
        {

        }
        this._magnetic_object.add_magnetic_objects(magnetic_objects.ToArray());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        magnetic_object.Processing(this.gameObject);
    }
    void OnGUI()
    {
        if (this.show_debug)
        {
            GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), magnetic_object.d2.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 50), new Vector2(500, 50)), magnetic_object.d1.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 100), new Vector2(500, 50)), magnetic_object.d3.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 150), new Vector2(50, 50)), magnetic_object.d4.ToString());
            GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), gameObject.GetComponent<Rigidbody2D>().position.ToString());
            //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
        }
    }
}
