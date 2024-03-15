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

    public class TetrisGame
    {
        public Board board = new Board();
        public Tetromino currentTetromino;

        public int score { get; set; }

        public bool gameOver { get; private set; }

        public void MoveTetromino(Vector2Int direction)
        {
            if (this.gameOver)
            {
                return;
            }

            Vector2Int newPosition = this.currentTetromino.GetPosition() + direction;
            if (this.IsValidMove(this.currentTetromino.GetShape(), newPosition))
            {
                this.currentTetromino.SetPosition(newPosition);
            }
            else
            {
                if (direction == Vector2Int.down)
                {
                    this.CopyTetrominoToBoard();
                    this.GenerateNewTetromino();
                }
            }
        }

        public void GenerateNewTetromino()
        {
            var allTypes = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
            var randomIndex = UnityEngine.Random.Range(0, allTypes.Length);
            TetrominoType tetrominoType = allTypes[randomIndex];
            var newTetromino = new Tetromino(tetrominoType);
            this.currentTetromino = newTetromino;
            if (!this.IsValidMove(newTetromino.GetShape(), newTetromino.GetPosition()))
            {
                this.gameOver = true;
            }
        }

        public void RotateTetromino(int direction)
        {
            if (this.gameOver)
            {
                return;
            }

            var rotationIndex = this.currentTetromino.GetRotationIndex();
            var targetRotationIndex = (byte)((rotationIndex + 1) % 4);
            var key = (byte)((rotationIndex * 10) + targetRotationIndex);
            var wallKicks = Config.configs[TetrominoType.I].wallKicks[key];
            var shape = this.currentTetromino.GetShape();
            var rotated = Utils.RotateArray(shape, direction > 0);

            foreach (var wk in wallKicks)
            {
                var newPosition = this.currentTetromino.GetPosition() + wk;
                if (this.IsValidMove(rotated, newPosition))
                {
                    this.currentTetromino.SetShape(rotated);
                    this.currentTetromino.SetPosition(newPosition);
                    this.currentTetromino.SetRotationIndex(targetRotationIndex);
                    return;
                }
            }
        }

        public void HardDrop()
        {
            if (this.gameOver)
            {
                return;
            }

            var direction = Vector2Int.down;
            var newPosition = this.currentTetromino.GetPosition() + direction;
            while (this.IsValidMove(this.currentTetromino.GetShape(), newPosition))
            {
                this.currentTetromino.SetPosition(newPosition);
                newPosition = this.currentTetromino.GetPosition() + direction;
            }

            this.CopyTetrominoToBoard();
            this.GenerateNewTetromino();
        }

        private void CheckAndClearRows()
        {
            var yMin = this.currentTetromino.GetPosition().y;
            var shape = this.currentTetromino.GetShape();
            var size = Utils.GetSize(shape);
            var height = size.y;
            var yMax = yMin + height;
            yMin = Mathf.Max(yMin, 0);
            yMax = Mathf.Min(yMax, Config.boardSize.y);
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

            this.score = this.score + fullRows.Count;
        }

        private bool IsValidMove(byte[,] shape, Vector2Int position)
        {
            var size = Utils.GetSize(shape);

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

        private void CopyTetrominoToBoard()
        {
            Vector2Int position = this.currentTetromino.GetPosition();
            var shape = this.currentTetromino.GetShape();
            var size = Utils.GetSize(shape);

            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    if (shape[x, y] == 1)
                    {
                        Vector2Int boardPosition = position + new Vector2Int(x, y);
                        this.board.SetValue(boardPosition, 1);
                    }
                }
            }

            this.CheckAndClearRows();

            this.currentTetromino = null;
        }
    }
}
