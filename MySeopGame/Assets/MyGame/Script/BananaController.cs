using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaController : MonoBehaviour
{
    //�ٳ��� ȸ�� �ӵ�
    public float rotateSpeed;
    public float speed;

    public GameObject bananaObj;
    

    

    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }
}
