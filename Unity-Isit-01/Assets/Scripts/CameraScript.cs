using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float ScrollSpeed = 10f;
    private bool _rotationOn = false;
    private Vector2 _turn;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _rotationOn = true;
        }

        if (_rotationOn)
        {
            _turn.x += Input.GetAxis("Mouse X");
            _turn.y += Input.GetAxis("Mouse Y");

            transform.localRotation = Quaternion.Euler(-_turn.y, _turn.x, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            _rotationOn = false;
        }



        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float previousY = transform.position.y;
            transform.position += transform.forward * MoveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime;
            transform.position += transform.right * MoveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, previousY, transform.position.z);
        }        

        if (Input.GetAxisRaw("Y Axis") != 0)
        {
            transform.position += ScrollSpeed * new Vector3(0, Input.GetAxisRaw("Y Axis"), 0) * Time.deltaTime;
        }
    }
    // Start is called before the first frame update
}
