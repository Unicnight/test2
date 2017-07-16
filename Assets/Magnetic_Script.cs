using UnityEngine;
using System.Collections;

public class Magnetic_Script : MonoBehaviour
{
    public GameObject[] game_objects;
    
    public Magnetic_Object magnetic_object
    {
        private set;
        get;
    }
    // Use this for initialization
    void Start()
    {
        magnetic_object = new Magnetic_Sector(
            new Magnetic_Sector.Sector_Effective_Range(this.gameObject.GetComponent<Rigidbody2D>().position, new Vector2(0, 1), new Vector2(0, 1), 1, 10, 10)
            );
        System.Collections.Generic.List<Magnetic_Object> magnetic_objects = new System.Collections.Generic.List<Magnetic_Object>();
        try
        {
            foreach(GameObject g in game_objects)
            {
                magnetic_objects.Add(g.GetComponent<Magnetic_Script>().magnetic_object);
            }
        }catch
        {

        }
        magnetic_object.add_magnetic_objects(magnetic_objects.ToArray());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        magnetic_object.Processing(this.gameObject);
    }
    void OnGUI()
    {
        //GUI.TextArea(new Rect(new Vector2(0, 0), new Vector2(500, 50)), magnetic_object.distance.ToString());
        //GUI.TextArea(new Rect(new Vector2(0, 50), new Vector2(500, 50)), Mathf.Round(angle * 0.044f).ToString());
        //GUI.TextArea(new Rect(new Vector2(0, 100), new Vector2(500, 50)), force.ToString());
        //GUI.TextArea(new Rect(new Vector2(0, 150), new Vector2(50, 50)), force.magnitude.ToString());
        //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), angle.ToString());
        //GUI.TextArea(new Rect(new Vector2(0, 200), new Vector2(50, 50)), b2.ToString());
    }
}
