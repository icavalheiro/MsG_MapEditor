using UnityEngine;
using System.Collections;
using System;

public class Interface : MonoBehaviour 
{
	#region inspector
	public GUIStyle basicBtnStyle;
	public GUIStyle iconBtnStyle;
	public GUIStyle panelsBackgroundStyle;
	#endregion

	private event Action onUpdate;

	void OnGUI()
	{
		float __iconBtnSize = 30;
		float __iconBtnMargin = 10;
		Rect __topLeftMenuRect = new Rect(0,0, __iconBtnSize * 3 + __iconBtnMargin * 4, __iconBtnMargin*2 + __iconBtnSize);
		GUI.BeginGroup(__topLeftMenuRect);
		{
			//GUI.color = new Color(1,1,1,0.3f);
			GUI.Box(new Rect(0,0, __topLeftMenuRect.width, __topLeftMenuRect.height), "", panelsBackgroundStyle);
			//GUI.color = Color.white;

			//load tiles
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin, __iconBtnSize,__iconBtnSize), new GUIContent(@"", "Load tiles"), iconBtnStyle))
			{
				System.Windows.Forms.OpenFileDialog __dialog = new System.Windows.Forms.OpenFileDialog();
				__dialog.Filter = "PNG|*.png|JPG|*.jpg";
				__dialog.Title = "Select images to load as tiles:";
				__dialog.Multiselect = true;
				if(__dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					//deal with selected tiles

					System.Windows.Forms.MessageBox.Show("Textures loaded.");
				}
			}

			//save map
			if(GUI.Button(new Rect(__iconBtnSize + __iconBtnMargin + __iconBtnMargin, __iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Save current map"), iconBtnStyle))
			{
				System.Windows.Forms.SaveFileDialog __dialog = new System.Windows.Forms.SaveFileDialog();
				__dialog.Filter = "MSGMAP|*.msgmap";
				__dialog.Title = "Selec where you want to save your map:";
				if(__dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					//deal with saving

					System.Windows.Forms.MessageBox.Show("Map saved.");
				}

			}

			//load map
			if(GUI.Button(new Rect(((__iconBtnSize + __iconBtnMargin) * 2) + __iconBtnMargin, __iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Load map"), iconBtnStyle))
			{
				System.Windows.Forms.OpenFileDialog __dialog = new System.Windows.Forms.OpenFileDialog();
				__dialog.Filter = "MSGMAP|*.msgmap";
				__dialog.Title = "Select map to load:";
				__dialog.Multiselect = false;
				if(__dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					//deal with selected map
					
					System.Windows.Forms.MessageBox.Show("Map loaded.");
				}
			}
		}
		GUI.EndGroup();

		Rect __cameraBtnsRect = new Rect(Screen.width - __iconBtnSize - (__iconBtnMargin*2), 0, __iconBtnSize + (__iconBtnMargin*2), __iconBtnSize*3 + (__iconBtnMargin *4));
		GUI.BeginGroup(__cameraBtnsRect);
		{
			//GUI.color = new Color(1,1,1,0.3f);
			GUI.Box(new Rect(0,0, __cameraBtnsRect.width, __cameraBtnsRect.height), "", panelsBackgroundStyle);
			//GUI.color = Color.white;

			//change camera view
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Change camera view"), iconBtnStyle))
			{

			}

			//change camera zoom in
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin + __iconBtnSize + __iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Zoom in"), iconBtnStyle))
			{
				
			}

			//change camera zoom out
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin + (__iconBtnSize + __iconBtnMargin) * 2, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Zoom out"), iconBtnStyle))
			{
				
			}
		}
		GUI.EndGroup();



		//tooltip
		if(GUI.tooltip != "")
		{
			var __size = panelsBackgroundStyle.CalcSize(new GUIContent(GUI.tooltip));
			
			GUI.Box(new Rect(
				(((Input.mousePosition.x+5) > (Screen.width - __size.x - 30)) ? (Input.mousePosition.x+5 -__size.x - 30) : (Input.mousePosition.x+5)), 
				(Screen.height - Input.mousePosition.y)+15, 
				__size.x + 30, 
				30), GUI.tooltip, panelsBackgroundStyle);
		}
	}

	void Update()
	{
		if(onUpdate != null)
			onUpdate();
	}
}
