using System;
using System.Collections.Generic;
using UnityEngine;

class TileManager : MonoBehaviour
{
    #region private data
    private static TileManager _instance = null;
	private static List<TileObject> _tilesInScene = new List<TileObject>();
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

		_tilesInScene.Add(__tileObj);

        //return the final tile obj
        return __tileObj;
    }

	public static void DeleteTilesInPosition(Vector3 p_where)
	{
		List<TileObject> __tilesInGivenPosition = new List<TileObject>();
		_tilesInScene.ForEach(x => 
		{
			if(new Vector2(x.position.x, x.position.z) == new Vector2(p_where.x, p_where.z))
				__tilesInGivenPosition.Add(x);
		});

		while(__tilesInGivenPosition.Count > 0)
		{
			TileObject __toDelete = __tilesInGivenPosition[0];
			__tilesInGivenPosition.RemoveAt(0);
			__toDelete.Destroy();
		}
	}
}
