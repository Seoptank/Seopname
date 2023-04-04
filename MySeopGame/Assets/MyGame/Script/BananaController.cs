using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaController : MonoBehaviour
{
    //바나나 회전 속도
    public float rotateSpeed;
    public float speed;

    public GameObject bananaObj;
    

    

    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }
}
