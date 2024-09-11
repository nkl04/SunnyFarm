using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCam : MonoBehaviour
{
    void Start()
    {
        SetBoundConfiner();
    }

    /// <summary>
    /// Set the collider that cinemachine uses to define the edges of the screen
    /// </summary>
    private void SetBoundConfiner()
    {
        PolygonCollider2D boundConfiner = GameObject.FindGameObjectWithTag(Tag.BoundConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = boundConfiner;

        //since the confiner bounds have changed need to call this to clear the cache

        confiner.InvalidatePathCache();
    }

}