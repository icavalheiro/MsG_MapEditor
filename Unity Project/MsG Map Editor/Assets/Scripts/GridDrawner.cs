using UnityEngine;
using System.Collections;

public class GridDrawner : MonoBehaviour
{
    #region inspector
    public float yIndex = 0;
    #endregion

    #region private data
    private Camera _camera;
	private GUIStyle _style;
	private Material _lineMaterial;
	#endregion

	void Awake()
	{
		//create material for drawing with opengl
		_lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" + "SubShader { Pass { " + "    Blend SrcAlpha OneMinusSrcAlpha " + "    ZWrite Off Cull Off Fog { Mode Off } " + "    BindChannels {" + "      Bind \"vertex\", vertex Bind \"color\", color }" + "} } }");
		_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		_lineMaterial.color = new Color(33/255,76/255,64/255,0.3f);

		//setup class variables
		_camera = this.gameObject.GetComponent<Camera>();
		_style = new GUIStyle();
		_style.normal.textColor = Color.white;
		_style.alignment = TextAnchor.MiddleCenter;
		_style.fontSize = 9;
	}
	 
	void OnGUI()
	{
		//draw (0,0) mark
		Vector3 __screenPosition = _camera.WorldToScreenPoint(Vector3.zero + (0.4f * Vector3.up));
		Rect __drawRect = new Rect(__screenPosition.x - 30, (Screen.height - __screenPosition.y) - 10, 60, 20);
		GUI.Label(__drawRect, "(0,0)", _style);

	}

    //draw the grid lines
	void OnPostRender()
	{
        //set material to be used by opengl
		_lineMaterial.SetPass(0);

        //finds out where is the center of the screen, from where we will start drawing
		Ray __centerOfScreenRay = _camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 1));
		Vector3 __centerOfScreen = __centerOfScreenRay.origin + (((-__centerOfScreenRay.origin.y)/__centerOfScreenRay.direction.y) * __centerOfScreenRay.direction);
		__centerOfScreen = new Vector3(Mathf.Round(__centerOfScreen.x), 0, Mathf.Round(__centerOfScreen.z));

        //start drawing
		int __numberOfGridsToDraw = 50;
		int __numberOfGridsToDrawHeight = (int)(__numberOfGridsToDraw * 0.5f);
		for(int l = (int)(-__numberOfGridsToDrawHeight * 0.5f); l < (__numberOfGridsToDrawHeight * 0.5f); l++)
			for(int c = (int)(-__numberOfGridsToDraw * 0.5f); c < (__numberOfGridsToDraw * 0.5f); c++)
				DrawSquare(new Vector2(__centerOfScreen.x + c, __centerOfScreen.z + l), 1);
	}

	//draw a line with opengl
	private void DrawLine(Vector2 p_start, Vector2 p_end)
	{
		GL.PushMatrix();
		GL.Begin(GL.LINES);
		{
			GL.Color(new Color(1,1,1,0.07f));
            GL.Vertex3(p_start.x, yIndex - 0.1f, p_start.y);
            GL.Vertex3(p_end.x, yIndex - 0.1f, p_end.y);
		}
		GL.End();
		GL.PopMatrix();
	}
	
	//draw a square with opengl lines
	private void DrawSquare(Vector2 p_point, float p_size)
	{
		float __halfSize = p_size * 0.5f;

		//top
		DrawLine(new Vector2(p_point.x - __halfSize, p_point.y - __halfSize), new Vector2(p_point.x + __halfSize, p_point.y - __halfSize));
		
		//bot
		DrawLine(new Vector2(p_point.x - __halfSize, p_point.y + __halfSize), new Vector2(p_point.x + __halfSize, p_point.y + __halfSize));
		
		//left
		DrawLine(new Vector2(p_point.x - __halfSize, p_point.y - __halfSize), new Vector2(p_point.x - __halfSize, p_point.y + __halfSize));
		
		//right
		DrawLine(new Vector2(p_point.x + __halfSize, p_point.y - __halfSize), new Vector2(p_point.x + __halfSize, p_point.y + __halfSize));
	}
}
