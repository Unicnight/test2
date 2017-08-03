using UnityEngine;
using System.Collections;

public class Camera_Move : MonoBehaviour
{
    public GameObject _player_1;
    public GameObject _player_2;
    public Vector3 _offset;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = (this._player_1.transform.position + this._player_2.transform.position) / 2 + (Vector3)this._offset;
    }
}
