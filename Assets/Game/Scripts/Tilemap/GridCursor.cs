namespace SunnyFarm.Game.Tilemap
{
    using SunnyFarm.Game.Entities.Player;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class GridCursor : MonoBehaviour
    {
        private Canvas canvas;
        private Grid grid;
        private Camera mainCamera;
        [SerializeField] private Image cursorPrefab;
        [SerializeField] private RectTransform cursorRectTransform = null;
        [SerializeField] private List<Image> cursorImages = new List<Image>();
        [SerializeField] private Sprite greenCursorSprire = null;
        [SerializeField] private Sprite redCursorSprite = null;

        // test
        [SerializeField] private Player player;

        public bool CursorPositionIsValid { get; set; }
        public int ItemUseGridRadius { get; set; } = 1;
        [field: SerializeField] // test
        public bool CursorIsEnabled { get; private set; }


        [SerializeField] private int rowGrid;
        [SerializeField] private int colGrid;

        private void OnDisable()
        {
            EventHandler.OnAfterSceneLoad -= SceneLoad;
        }
        private void OnEnable()
        {
            EventHandler.OnAfterSceneLoad += SceneLoad;
        }
        private void Start()
        {
            mainCamera = Camera.main;
            canvas = GetComponentInParent<Canvas>();
        }
        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    EnableCursors(rowGrid, colGrid);
            //}
            if (CursorIsEnabled)
            {
                DisplayCursor(rowGrid, colGrid);
            }
        }
        private Vector3Int DisplayCursor(int row, int col)
        {
            if (grid != null)
            {
                // Get grid position for cursor
                Vector3Int gridPosition = GetGridPositionForCursor();

                // Get grid position for player
                Vector3Int playerGridPosition = GetGridPositionForPlayer();

                // Change the grid layout rotation

                // Set cursor sprite
                SetCursorValidity(gridPosition, playerGridPosition);

                // Get rect transform position for cursor
                cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

                return gridPosition;
            }
            else
            {
                return Vector3Int.zero;
            }
        }

        private Vector3 GetRectTransformPositionForCursor(Vector3Int gridPosition)
        {
            Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
            Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
            return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursorGridPosition"></param>
        /// <param name="playerGridPosition"></param>
        private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
        {
            SetCursorToValid();

            // Check item use radius is valid
            if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius
                || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius)
            {
                SetCursorToInvalid();
                return;
            }



            GridPropertiesDetail gridPropertiesDetail =
                GridPropertiesController.Instance.GetGridPropertyDetail(cursorGridPosition.x, cursorGridPosition.y);

            //if (gridPropertiesDetail != null)
            //{
            //    if (gridPropertiesDetail.TileType != TileType.Dug)
            //    {
            //        SetCursorToInvalid();
            //        return;
            //    }
            //}
            //else
            //{
            //    SetCursorToInvalid();
            //    return;
            //}
        }

        private void SetCursorToInvalid()
        {
            SetImageCursors(redCursorSprite);
            CursorPositionIsValid = false;
        }

        private void SetCursorToValid()
        {
            SetImageCursors(greenCursorSprire);
            CursorPositionIsValid = true;
        }
        private void SetImageCursors(Sprite sprite)
        {
            foreach (var cursor in cursorImages)
            {
                cursor.sprite = sprite;
            }
        }
        private void SetColorCursors(Color color)
        {
            foreach (var cursor in cursorImages)
            {
                cursor.color = color;
            }
        }
        public Vector3Int GetGridPositionForPlayer()
        {
            return grid.WorldToCell(player.transform.position);
        }

        public Vector3Int GetGridPositionForCursor()
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                           Input.mousePosition.y,
                           mainCamera.transform.position.z)
                );

            return grid.WorldToCell(worldPosition);
        }

        private void SceneLoad()
        {
            grid = GameObject.FindObjectOfType<Grid>();
        }

        public void DisableCursors()
        {
            SetColorCursors(Color.clear);

            CursorIsEnabled = false;

            rowGrid = 0;
            colGrid = 0;
        }
        private void HideCursor()
        {
            SetColorCursors(Color.clear);
        }
        private void ShowCursor()
        {
            SetColorCursors(new Color(1f, 1f, 1f, 1f));
        }
        public void EnableCursors(int row, int col)
        {
            SetUpLayoutCursor(col);

            int imagesCount = cursorImages.Count;

            if (imagesCount < row * col)
            {
                for (int i = 0; i < row * col - imagesCount; i++)
                {
                    var image = Instantiate(cursorPrefab, cursorRectTransform);
                    cursorImages.Add(image);
                }
            }

            SetColorCursors(new Color(1f, 1f, 1f, 1f));

            CursorIsEnabled = true;

            rowGrid = row;
            colGrid = col;
        }
        private void SetUpLayoutCursor(int col)
        {
            cursorRectTransform.pivot = new Vector2((col / 2) / (float)col, 0);
            cursorRectTransform.GetComponent<GridLayoutGroup>().constraintCount = col;
        }
    }
}
