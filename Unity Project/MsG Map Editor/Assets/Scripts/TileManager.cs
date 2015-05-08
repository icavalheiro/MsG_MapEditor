using System;
using System.Collections.Generic;
using UnityEngine;

class TileManager : MonoBehaviour
{
    #region private data
    private static TileManager _instance = null;
    #endregion

    #region inspector
    public GameObject planePrefab = null;
    #endregion

    void Awake()
    {
        _instance = this;
    }

    public static TileObject CreateTile(Vector3 p_where, Tile p_tile)
    {
        if (_instance == null)
        {
            Debug.Log("You forgot to place a TileManager prefab in the scene!");
            return null;
        }

        //create the instance
        GameObject __instance = (GameObject)GameObject.Instantiate(_instance.planePrefab, p_where, Quaternion.identity);

        //set up the plane
        Plane __plane = __instance.GetComponent<Plane>();
        __plane.SetMaterial(MaterialManager.GetMaterial(p_tile.texture, p_tile.useTransparency));

        //set up the tileObj
        TileObject __tileObj = __instance.AddComponent<TileObject>();
        __tileObj.SetPlane(__plane);
        __tileObj.SetPostion(p_where);

        //return the final tile obj
        return __tileObj;
    }
}
