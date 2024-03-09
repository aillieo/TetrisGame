// -----------------------------------------------------------------------
// <copyright file="TetrisGame.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TetrisGame : MonoBehaviour
    {
        private Board board = new Board();
        private Tetromino currentTetromino;
        private int gridSize = 20;
        private Color emptyColor = Color.white;
        private Color filledColor = Color.green;

        private void Start()
        {
            this.currentTetromino = this.GenerateNewTetromino();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                this.MoveTetromino(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                this.MoveTetromino(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                this.MoveTetromino(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                this.RotateTetromino(1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.HardDrop();
            }
        }

        private void MoveTetromino(Vector2Int direction)
        {
            Vector2Int newPosition = this.currentTetromino.GetPosition() + direction;
            if (this.IsValidMove(this.currentTetromino.GetShape(), newPosition))
            {
                this.currentTetromino.Move(direction);
            }
            else
            {
                if (direction == Vector2Int.down)
                {
                    var reachedBottom = newPosition.y <= 0;
                    var collided = !this.IsValidMove(this.currentTetromino.GetShape(), this.currentTetromino.GetPosition() + Vector2Int.down);
                    if (reachedBottom || collided)
                    {
                        this.CopyTetrominoToBoard();
                        this.CheckAndClearRows();
                        this.currentTetromino = this.GenerateNewTetromino();
                    }
                }
            }
        }

        private void CopyTetrominoToBoard()
        {
            Vector2Int position = this.currentTetromino.GetPosition();
            var shape = this.currentTetromino.GetShape();
            var width = shape.GetLength(0);
            var height = shape.GetLength(1);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (shape[x, y] == 1)
                    {
                        Vector2Int boardPosition = position + new Vector2Int(x, y);
                        this.board.SetValue(boardPosition, 1);
                    }
                }
            }

            this.CheckAndClearRows();
        }

        private Tetromino GenerateNewTetromino()
        {
            var allTypes = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
            var randomIndex = UnityEngine.Random.Range(0, allTypes.Length);
            TetrominoType tetrominoType = allTypes[randomIndex];
            return new Tetromino(tetrominoType);
        }

        private void RotateTetromino(int direction)
        {
            var rotated = Utils.RotateArray(this.currentTetromino.GetShape(), direction > 0);
            if (this.IsValidMove(rotated, this.currentTetromino.GetPosition()))
            {
                this.currentTetromino.SetShape(rotated);
            }
        }

        private bool IsValidMove(byte[,] shape, Vector2Int position)
        {
            var size = new Vector2Int(shape.GetLength(0), shape.GetLength(1));

            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    if (shape[x, y] == 1)
                    {
                        Vector2Int cellPosition = position + new Vector2Int(x, y);
                        if (cellPosition.x < 0 || cellPosition.x >= Config.boardSize.x ||
                            cellPosition.y < 0 || cellPosition.y >= Config.boardSize.y ||
                            this.board.GetValue(cellPosition) != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void OnDrawGizmos()
        {
            var basePosition = this.transform.position;

            // board
            for (var x = 0; x < Config.boardSize.x; x++)
            {
                for (var y = 0; y < Config.boardSize.y; y++)
                {
                    Vector3 position = basePosition + new Vector3(x * this.gridSize, y * this.gridSize, 0);
                    Gizmos.color = this.board.GetValue(new Vector2Int(x, y)) == 0 ? this.emptyColor : this.filledColor;
                    Gizmos.DrawCube(position, Vector3.one * this.gridSize * 0.5f);
                }
            }

            // currentTetromino
            if (this.currentTetromino != null)
            {
                Vector2Int tetrominoPosition = this.currentTetromino.GetPosition();
                var shape = this.currentTetromino.GetShape();
                var width = shape.GetLength(0);
                var height = shape.GetLength(1);
                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        if (shape[x, y] == 1)
                        {
                            Vector3 position = basePosition + new Vector3((tetrominoPosition.x + x) * this.gridSize, (tetrominoPosition.y + y) * this.gridSize, 0);
                            Gizmos.color = this.filledColor;
                            Gizmos.DrawCube(position, Vector3.one * this.gridSize * 0.8f);
                        }
                    }
                }
            }
        }

        private void HardDrop()
        {
            var direction = Vector2Int.down;
            while (this.IsValidMove(this.currentTetromino.GetShape(), this.currentTetromino.GetPosition() + direction))
            {
                this.currentTetromino.Move(direction);
            }
        }

        private void CheckAndClearRows()
        {
            var yMin = this.currentTetromino.GetPosition().y;
            var yMax = yMin + this.currentTetromino.GetShape().GetLength(1);
            yMin = Mathf.Max(yMin, 0);
            var fullRows = new List<int>();
            for (var y = yMin; y < yMax; ++y)
            {
                var rowFull = true;
                for (var x = 0; x < Config.boardSize.x; ++x)
                {
                    if (this.board.GetValue(new Vector2Int(x, y)) == 0)
                    {
                        rowFull = false;
                        break;
                    }
                }

                if (rowFull)
                {
                    fullRows.Add(y);
                }
            }

            for (var y = 0; y < Config.boardSize.y; ++y)
            {
                var shift = 0;
                for (var i = 0; i < fullRows.Count; ++i)
                {
                    if (y <= fullRows[i])
                    {
                        break;
                    }

                    shift++;
                }

                if (shift > 0)
                {
                    for (var x = 0; x < Config.boardSize.x; ++x)
                    {
                        var position = new Vector2Int(x, y);
                        var value = this.board.GetValue(position);
                        position.y -= shift;
                        this.board.SetValue(position, value);
                    }
                }
            }
        }
    }
}
