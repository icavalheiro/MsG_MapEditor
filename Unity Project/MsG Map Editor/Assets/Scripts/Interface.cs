using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour 
{
	#region inspector
	public GUIStyle basicBtnStyle;
	public GUIStyle iconBtnStyle;
	#endregion

	void OnGUI()
	{
		Rect __topLeftMenu = new Rect(0,0, 150, 50);
		GUI.BeginGroup(__topLeftMenu);
		{
			//load tiles
			if(GUI.Button(new Rect(10,10, 40,40), new GUIContent(@"", "Load tiles"), iconBtnStyle))
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
			if(GUI.Button(new Rect(60, 10, 40, 40), new GUIContent(@"", "Save current map"), iconBtnStyle))
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
			if(GUI.Button(new Rect(110, 10, 40, 40), new GUIContent(@"", "Load map"), iconBtnStyle))
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



		//tooltip
		if(GUI.tooltip != "")
			GUI.Box(new Rect(
				(((Input.mousePosition.x+5) > (Screen.width - 300)) ? (Screen.width-300) : (Input.mousePosition.x+5)), 
				(Screen.height - Input.mousePosition.y)+15, 
				300, 
				30), GUI.tooltip);
	}
}
