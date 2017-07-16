using UnityEngine;
using System.Collections;

public class Magnetic_Script : MonoBehaviour
{
    Magnetic_Sector magnetic_sector;
    // Use this for initialization
    void Start()
    {
        
        magnetic_sector=new Magnetic_Sector(
            new Magnetic_Sector.Sector_Effective_Range(this.gameObject.GetComponent<Rigidbody2D>().position,new Vector2(0,1),new Vector2(0,1),1,10,10)
            )
    }

    // Update is called once per frame
    void Update()
    {

    }
}
