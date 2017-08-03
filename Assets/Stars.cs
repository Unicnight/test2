using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D co)
    {
        print("adasd");
        if (co.tag=="Player")
            Destroy(gameObject);
    }
}
