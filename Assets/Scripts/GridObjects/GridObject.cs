using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GridObject : MonoBehaviour
{
    public VisualEffect mPoufEffect;

    public virtual void Pouf()
    {
        mPoufEffect.SendEvent("StartPouf");
    }

}
