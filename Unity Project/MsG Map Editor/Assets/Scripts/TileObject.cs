using System;
using System.Collections.Generic;
using UnityEngine;

class TileObject : MonoBehaviour
{
    #region events
    public event Action onDeleted;
    #endregion

    public Tile tile;
    public Plane plane;
    public Vector3 position;

    public void SetTile(Tile p_tile)
    {
        //remove all event listeners from the old tile
        if(tile != null)
        {
            tile.onDeleted -= OnTileDeleted;
            tile.onTextureChanged -= OnTileChangedTexture;
            tile.onTransparencyChanged -= OnTileTransparencyChanged;
        }

        //set new tile
        this.tile = p_tile;

        //add event listeners to the new tile
        if(p_tile != null)
        {
            p_tile.onDeleted += OnTileDeleted;
            p_tile.onTextureChanged += OnTileChangedTexture;
            p_tile.onTransparencyChanged += OnTileTransparencyChanged;
        }
    }

    public void SetPostion(Vector3 p_position)
    {
        this.position = p_position;
    }

    public void SetPlane(Plane p_plane)
    {
        this.plane = p_plane;
    }

    private void OnTileTransparencyChanged()
    {
        UpdatePlaneMaterial();
    }

    private void OnTileChangedTexture()
    {
        UpdatePlaneMaterial();
    }

    private void UpdatePlaneMaterial()
    {
        plane.SetMaterial(MaterialManager.GetMaterial(tile.texture, tile.useTransparency));
    }

    private void OnTileDeleted()
    {
        if (onDeleted != null) onDeleted();

		if(this != null && this.gameObject != null)
        	GameObject.DestroyObject(this.gameObject);
    }

	public void Destroy()
	{
		OnTileDeleted();
	}
}
