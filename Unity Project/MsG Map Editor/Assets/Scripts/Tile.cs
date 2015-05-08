using System;
using System.Collections.Generic;
using UnityEngine;

class Tile
{
    #region events
    public event Action onDeleted;
    public event Action onTextureChanged;
    public event Action onTransparencyChanged;
    #endregion

    public Texture texture;
    public bool useTransparency;

    public void SetTexture(Texture p_texture)
    {
        texture = p_texture;
        if (onTextureChanged != null) onTextureChanged();
    }

    public void SetTransparency(bool p_useTransparency)
    {
        this.useTransparency = p_useTransparency;
        if (onTransparencyChanged != null) onTransparencyChanged();
    }

    public void Delete()
    {
        if (onDeleted != null) onDeleted();
        GameObject.DestroyObject(texture);
    }
}
