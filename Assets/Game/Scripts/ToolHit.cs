using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolHit : MonoBehaviour
{
    public virtual void Hit()
    {
        Debug.Log("Hit");
    }
}
