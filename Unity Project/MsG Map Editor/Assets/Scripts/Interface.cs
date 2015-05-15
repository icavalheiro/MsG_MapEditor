using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Interface : MonoBehaviour 
{
	private class TileFolder
	{
		public event Action onDelete;

		public List<LoadedTile> tilesInside = new List<LoadedTile>();
		public string name = "New folder";
		public bool isOpened = false;

		public void Delete()
		{
			if(onDelete != null) onDelete();
		}

		public float Draw(float p_height, float p_tileSize, float p_folderSize, float p_margin)
		{
			return 0;
		}
	}

	private class LoadedTile
	{
		public event Action<LoadedTile> onParentFolderRemoved;
		public event Action<LoadedTile, TileFolder> onParentFolderSet;
		public event Action<LoadedTile> onClicked;
		public event Action<LoadedTile, bool> onSelectedChange;

		public bool isSelected = false;
		public Tile tile = null;
		public TileFolder parentFolder = null;

		public void SetSelectedState(bool p_isSelected)
		{
			isSelected = p_isSelected;

			if(onSelectedChange != null)
				onSelectedChange(this, p_isSelected);
		}

		public void SetParentFolder(TileFolder p_folder)
		{
			if(parentFolder != null)
				ClearParentFolder();
			
			parentFolder = p_folder;

			if(parentFolder != null)
				parentFolder.onDelete += ClearParentFolder;

			if(onParentFolderSet != null)
				onParentFolderSet(this, p_folder);
		}

		private void ClearParentFolder()
		{
			if(parentFolder != null)
				parentFolder.onDelete -= ClearParentFolder;

			parentFolder = null;

			if(onParentFolderRemoved != null)
				onParentFolderRemoved(this);
		}

		public float Draw(float p_height, float p_size)
		{
			Rect __rect = new Rect(0, p_height, p_size, p_size);

			if(isSelected)
				GUI.Box(__rect, tile.texture);
			else if(GUI.Button(__rect, tile.texture))
				if(onClicked != null)
					onClicked(this);

			return __rect.y + p_size;
		}
	}

	#region inspector
	public GUIStyle basicBtnStyle;
	public GUIStyle iconBtnStyle;
	public GUIStyle panelsBackgroundStyle;
	public GameObject selectionPrefab;
	public float tileSelectionBtnSize = 75;
	public float tileSelectionBtnMargin = 10;
	public float tileFolderSelectionSize = 36;
	#endregion

	#region private data
	private event Action onUpdate;
	private Transform _selectorTransform;
	private List<LoadedTile> _loadedTiles = new List<LoadedTile>();
	private Vector2 _tileSelectorScrollPosition = new Vector2();
	private List<LoadedTile> _loadedTilesOutOfFolders = new List<LoadedTile>();
	private List<TileFolder> _folders = new List<TileFolder>();
	private float _tileSelectionCurrentHeight = 0;
	private List<Rect> _rectsDrawnedLastFrame = new List<Rect>();
	private List<LoadedTile> _currentSelectedTiles = new List<LoadedTile>();
	#endregion

	void Start()
	{
		_selectorTransform = ((GameObject)GameObject.Instantiate(selectionPrefab)).transform;
	}

	void OnGUI()
	{
		float __iconBtnSize = 30;
		float __iconBtnMargin = 10;

		_rectsDrawnedLastFrame = new List<Rect>();

		//top left menu
		Rect __topLeftMenuRect = new Rect(0,0, __iconBtnSize * 3 + __iconBtnMargin * 4, __iconBtnMargin*2 + __iconBtnSize);
		_rectsDrawnedLastFrame.Add(__topLeftMenuRect);
		GUI.BeginGroup(__topLeftMenuRect);
		{
			GUI.Box(new Rect(0,0, __topLeftMenuRect.width, __topLeftMenuRect.height), "", panelsBackgroundStyle);

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

					foreach(var spritePath in __dialog.FileNames)
					{
						WWW __www = new WWW("file://" + spritePath);
						while(__www.isDone) {}

						Tile __newTile = new Tile();
						__newTile.SetTexture(__www.texture);

						LoadedTile __loadedTile = new LoadedTile();
						__loadedTile.tile = __newTile;

						__loadedTile.onParentFolderSet += (p_loadedTile, p_folder) =>
						{
							if(p_folder != null)
								if(_loadedTilesOutOfFolders.Contains(p_loadedTile))
									_loadedTilesOutOfFolders.Remove(p_loadedTile);
						};

						__loadedTile.onParentFolderRemoved += (p_loadedTile) =>
						{
							if(_loadedTilesOutOfFolders.Contains(p_loadedTile) == false)
								_loadedTilesOutOfFolders.Add(p_loadedTile);
						};

						__loadedTile.onClicked += (p_loadedTile) =>
						{
							p_loadedTile.SetSelectedState(true);

							if(!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
								_loadedTiles.ForEach(x => 
								{
									if(x != p_loadedTile)
										x.SetSelectedState(false);
								});
						};

						__loadedTile.onSelectedChange += (p_loadedTile, p_isSelected) =>
						{
							if(p_isSelected)
							{
								if(_currentSelectedTiles.Contains(p_loadedTile) == false)
									_currentSelectedTiles.Add(p_loadedTile);
							}
							else
								_currentSelectedTiles.Remove(p_loadedTile);

						};

						_loadedTilesOutOfFolders.Add(__loadedTile);
						_loadedTiles.Add(__loadedTile);
					}

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

		//camera btns
		Rect __cameraBtnsRect = new Rect(Screen.width - __iconBtnSize - (__iconBtnMargin*2), 0, __iconBtnSize + (__iconBtnMargin*2), __iconBtnSize*3 + (__iconBtnMargin *4));
		_rectsDrawnedLastFrame.Add(__cameraBtnsRect);
		GUI.BeginGroup(__cameraBtnsRect);
		{
			GUI.Box(new Rect(0,0, __cameraBtnsRect.width, __cameraBtnsRect.height), "", panelsBackgroundStyle);

			//change camera view
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Change camera view"), iconBtnStyle))
			{

			}

			//change camera zoom in
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin + __iconBtnSize + __iconBtnMargin, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Zoom in"), iconBtnStyle))
			{
				Camera.main.fieldOfView -= 5;
			}

			//change camera zoom out
			if(GUI.Button(new Rect(__iconBtnMargin,__iconBtnMargin + (__iconBtnSize + __iconBtnMargin) * 2, __iconBtnSize, __iconBtnSize), new GUIContent(@"", "Zoom out"), iconBtnStyle))
			{
				Camera.main.fieldOfView += 5;
			}

			Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 45, 70);
		}
		GUI.EndGroup();

		//tile sliders
		Rect __tileSelectionAreaRect = new Rect(0, __topLeftMenuRect.y + __topLeftMenuRect.height + 25, 150, Screen.height - (__topLeftMenuRect.y + __topLeftMenuRect.height + 25));
		_rectsDrawnedLastFrame.Add(__tileSelectionAreaRect);
		GUI.BeginGroup(__tileSelectionAreaRect);
		{
			Rect __boxRect = new Rect(0,0, __tileSelectionAreaRect.width, __tileSelectionAreaRect.height);
			GUI.Box(__boxRect, "", panelsBackgroundStyle);
			Rect __viewRect = new Rect(
				0, 
				0, 
				__boxRect.width - 26, 
				_tileSelectionCurrentHeight);//(_loadedTiles.Count * (tileSelectionBtnSize + (tileSelectionBtnMargin))) + (_folders.Count * (tileFolderSelectionSize + tileSelectionBtnMargin)));
			_tileSelectorScrollPosition = GUI.BeginScrollView(new Rect(10,0,__boxRect.width-10, __boxRect.height),
			                                                  _tileSelectorScrollPosition,
			                                                  __viewRect);
			{
				_tileSelectionCurrentHeight = 0;
				foreach(var folder in _folders)
					_tileSelectionCurrentHeight = folder.Draw(_tileSelectionCurrentHeight, tileSelectionBtnSize, tileFolderSelectionSize, tileSelectionBtnMargin);

				foreach(var tile in _loadedTilesOutOfFolders)
					_tileSelectionCurrentHeight = tile.Draw(_tileSelectionCurrentHeight + tileSelectionBtnMargin, tileSelectionBtnSize);

				_tileSelectionCurrentHeight += tileSelectionBtnMargin;
				/*for(int i = 0; i < _loadedTiles.Count; i++)
				{
					Rect __whereToDraw = new Rect(0, i * tileSelectionBtnSize + (i * tileSelectionBtnMargin), tileSelectionBtnSize, tileSelectionBtnSize);
					GUI.DrawTexture(__whereToDraw, _loadedTiles[i].tile.texture);
				}*/
			}
			GUI.EndScrollView();
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
				30), "<i>" + GUI.tooltip + "</i>", panelsBackgroundStyle); 
		}
	}

	void Update()
	{
		if(onUpdate != null)
			onUpdate();

		if(Input.GetKeyDown(KeyCode.Escape))
			_loadedTiles.ForEach(x => x.SetSelectedState(false));

		Ray __ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Vector3 __updatedPosition = __ray.origin + (((-__ray.origin.y)/__ray.direction.y) * __ray.direction);
		__updatedPosition = new Vector3(Mathf.Round(__updatedPosition.x), 0.001f, Mathf.Round(__updatedPosition.z));
		_selectorTransform.position = __updatedPosition + (Vector3.down * 0.1f);

		ManageMouseButton();
	}

	private List<TileObject> _tilesCreatedInCurrentPaint = new List<TileObject>();
	private void ManageMouseButton()
	{
		if(Input.GetMouseButton(0) == false)
		{
			if(_tilesCreatedInCurrentPaint.Count > 0)
				_tilesCreatedInCurrentPaint = new List<TileObject>();
		}
		else
		{
			bool __mouseOverAMenu = false;
			_rectsDrawnedLastFrame.ForEach(x =>
			{
				if(x.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
					__mouseOverAMenu = true;
			});

			if(__mouseOverAMenu)
				return;

			if(_currentSelectedTiles.Count == 0)
			{//DELETE
				TileManager.DeleteTilesInPosition(_selectorTransform.position);
			}
			else
			{
				bool __tileAlreadyPainted = false;
				_tilesCreatedInCurrentPaint.ForEach(x =>
				{
					if(x.position == _selectorTransform.position)
						__tileAlreadyPainted = true;
				});

				if(__tileAlreadyPainted)
					return;

				//CREATE A TILE IN THE POSITION
				Tile __tileToSpawn = _currentSelectedTiles[UnityEngine.Random.Range(0, _currentSelectedTiles.Count)].tile;

				TileObject __createdTile = TileManager.CreateTile(_selectorTransform.position, __tileToSpawn);
				_tilesCreatedInCurrentPaint.Add(__createdTile);
			}

		}
	}
}
