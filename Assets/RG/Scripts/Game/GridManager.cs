using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace Hussien
{
    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Setup")]

        [SerializeField] private int Row = 0;

        [SerializeField] private int Column = 0;

        [SerializeField] Tile TilePrefab;

        [SerializeField] List<Tile> Tiles = new List<Tile>();

        [Header("Spacing Settings")]

        [SerializeField] private float RowSpacing = 0f;

        [SerializeField] private float ColumnSpacing = 0f;

        public List<int> CheckRowQueue = new List<int>();
        private bool CheckingRoutineStarted = false;
        private bool CheckingRowStarted = false;

        #region Singleton
        public static GridManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        #region Editor Events
        public void BuildGrid()
        {

            if (transform.childCount > 0)
            {
                for (int i = 0; i < Tiles.Count; i++)
                {
                    if (Tiles[i] != null)
                    {
                        DestroyImmediate(Tiles[i].gameObject);
                    }
                }

                Tiles.Clear();
            }

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Tile newTile = Instantiate(TilePrefab, new Vector3(j * (ColumnSpacing + 1), i * (RowSpacing + 1), 0), Quaternion.identity);
                    newTile.transform.SetParent(transform);
                    newTile.name = "Tile " + i + " " + j;
                    newTile.SetPosition(i, j);
                    Tiles.Add(newTile);
                }
            }

            //focus camera on center
            float gridWidth = (Column - 1) * (ColumnSpacing + 1);
            float gridHeight = (Row - 1) * (RowSpacing + 1);
            Camera.main.transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -Row * (RowSpacing + 1));
        }

        public void RandomizeBlocks()
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i] != null)
                {
                    if (i == 0)
                    {
                        Tiles[i].SetRandomBlock(null);
                    }
                    else
                    {
                        Tiles[i].SetRandomBlock(Tiles[i - 1]);
                    }
                }
            }
        }
        #endregion

        #region Grid Manipulation Methods 
        public Tile GetUpperTile(int _Row, int _Column)
        {
            if (_Row == Row - 1) //the is the top row
                return null;

            int index = (_Row + 1) * Column + _Column;
            return Tiles[index];
        }

        public Tile GetTile(int _Row, int _Column)
        {
            int index = _Row * Column + _Column;

            if (index < 0 || index >= Tiles.Count)
            {
                return null;
            }

            return Tiles[index];
        }

        public Tile GetBottomTile(int _Row, int _Column)
        {
            if (_Row < 0) //the is the bottom row
                return null;

            int index = (_Row - 1) * Column + _Column;
            return Tiles[index];
        }

        public bool IsMaxRow(int _Row)
        {
            return _Row == (Row - 1);
        }

        public Tile[] GetTilesInRow(int _Row)
        {
            Tile[] rowTiles = new Tile[Column];

            for (int i = 0; i < Column; i++)
            {
                rowTiles[i] = GetTile(_Row, i);
            }

            return rowTiles;
        }
        #endregion

        #region Gameplay Tile Events

        public void QueueHorizontalCheck(int _Row)
        {
            if (!CheckRowQueue.Contains(_Row))
            {
                CheckRowQueue.Add(_Row);
            }

            if (CheckingRoutineStarted == false)
            {
                StartCoroutine(StartHorizontalCheck());
            } 
        }

        private IEnumerator StartHorizontalCheck()
        {
            CheckingRoutineStarted = true;

            while (CheckRowQueue.Count > 0)
            {
                HorizontalCheck(CheckRowQueue[0]);
                CheckRowQueue.RemoveAt(0);
                yield return new WaitUntil(() => CheckingRowStarted == false);
            }
            CheckingRoutineStarted = false;
        }

        private void HorizontalCheck(int _Row)
        {
            CheckingRowStarted = true;
            Tile[] HorizontalTiles = GridManager.Instance.GetTilesInRow(_Row);

            int StartIndex = 0;
            int CurrentBlockIndex = HorizontalTiles[0].CurrentBlockIndex;

            for (int i = 1; i < HorizontalTiles.Length; i++)
            {
                if (HorizontalTiles[i].CurrentBlockIndex != CurrentBlockIndex)
                {
                    CheckAndProcessGroup(HorizontalTiles, StartIndex, i);
                    StartIndex = i;
                    CurrentBlockIndex = HorizontalTiles[i].CurrentBlockIndex;
                }
            }

            CheckAndProcessGroup(HorizontalTiles, StartIndex, HorizontalTiles.Length);
            CheckingRowStarted = false;
        }

        private void CheckAndProcessGroup(Tile[] tiles, int start, int end)
        {
            int GroupLength = end - start;
            if (GroupLength >= 3)
            {
                for (int j = start; j < end; j++)
                {
                    tiles[j].DestroyBlock();
                }
            }
        }
        #endregion

        #region Validation
        private void OnValidate()
        {
        #if UNITY_EDITOR
            Debug.LogWarning("<color=red>GridManager: Make sure if you change anything in the prefab you must rebuild your grid</color>");
        #endif
        }
        #endregion
    }
}
