using System;
using System.Collections.Generic;
using UnityEngine;

class Plane : MonoBehaviour
{
    #region private data
    private Material _material;
    private MeshRenderer _mesh;
    #endregion

    void Awake()
    {
        _mesh = this.GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material p_material)
    {
        _material = p_material;
        _mesh.material = _material;
    }
}
