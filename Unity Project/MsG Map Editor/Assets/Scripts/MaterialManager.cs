using System;
using System.Collections.Generic;
using UnityEngine;

class MaterialManager : MonoBehaviour
{
    #region structs
    private class MaterialHolder
    {
        public Material material;
        public bool useTransparency = false;
        public MaterialHolder(Material p_material, bool p_bool)
        {
            this.material = p_material;
            this.useTransparency = p_bool;
        }
    }
    #endregion

    #region inspector
    public Material baseMaterial = null;
    #endregion

    #region private data
    private static MaterialManager _singleton;
    private Dictionary<Texture, MaterialHolder> _loadedMaterials = new Dictionary<Texture, MaterialHolder>();
    #endregion

    void Awake()
    {
        _singleton = this;
    }

    public static Material GetMaterial(Texture p_texture, bool p_useTransparency)
    {
        if (p_texture == null)
        {
            Debug.LogError("DUMB ASS! You cant get a fucking material if you dont gimme a fucking texture! p_texture is null, dumbass!");
            return null;
        }

        if (_singleton == null)
        {
            Debug.LogError("You forgot to put a MaterialManager prefab in this scene!");
            return null;
        }

        MaterialHolder __toReturn = null;

        //check to see if we have such a material already loaded
        if (_singleton._loadedMaterials.ContainsKey(p_texture))
            __toReturn = _singleton._loadedMaterials[p_texture];
        else
        {//if not, we create a new one
            Material __newMaterial = new Material(_singleton.baseMaterial);
            __newMaterial.mainTexture = p_texture;
            __toReturn = new MaterialHolder(__newMaterial, false);
            _singleton._loadedMaterials.Add(p_texture, __toReturn);
        }

        //check to see if the transparency matches
        if (__toReturn.useTransparency != p_useTransparency)
        {
            __toReturn.useTransparency = p_useTransparency;
            if (__toReturn.useTransparency)
            {//set transparent propierties
                __toReturn.material.SetInt("_SrcBlend", 5);
                __toReturn.material.SetInt("_DstBlend", 10);
                __toReturn.material.SetInt("_ZWrite", 0);
                __toReturn.material.DisableKeyword("_ALPHATEST_ON");
                __toReturn.material.EnableKeyword("_ALPHABLEND_ON");
                __toReturn.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                __toReturn.material.renderQueue = 3000;
            }
            else
            {//set opaque propierties
                __toReturn.material.SetInt("_SrcBlend", 1);
                __toReturn.material.SetInt("_DstBlend", 0);
                __toReturn.material.SetInt("_ZWrite", 1);
                __toReturn.material.DisableKeyword("_ALPHATEST_ON");
                __toReturn.material.DisableKeyword("_ALPHABLEND_ON");
                __toReturn.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                __toReturn.material.renderQueue = -1;
            }
        }

        return __toReturn.material;
    }
}
