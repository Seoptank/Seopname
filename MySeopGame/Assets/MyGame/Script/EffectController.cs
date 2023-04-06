using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroyEffect", 0.5f);
    }

    void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
