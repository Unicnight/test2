using UnityEngine;
using System.Collections;


public class Player_Info : MonoBehaviour
{
    public readonly static Players _Player_1 = new Players();
    public readonly static Players _Player_2 = new Players();
    // Use this for initialization
    void Start()
    {
        _Player_1.initialize();
        _Player_2.initialize(KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.DownArrow);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Player_1.sync();
        _Player_2.sync();
    }
}
