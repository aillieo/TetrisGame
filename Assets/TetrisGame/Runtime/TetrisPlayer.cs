// -----------------------------------------------------------------------
// <copyright file="TetrisPlayer.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using UnityEngine;

    public class TetrisPlayer : MonoBehaviour
    {
        private readonly TetrisGame game = new TetrisGame();

        private readonly int gridSize = 20;
        private readonly Color emptyColor = Color.white;
        private readonly Color filledColor = Color.green;

        private void Start()
        {
            if (this.game.currentTetromino == null)
            {
                this.game.GenerateNewTetromino();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                this.game.MoveTetromino(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                this.game.MoveTetromino(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                this.game.MoveTetromino(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                this.game.RotateTetromino(1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.game.HardDrop();
            }
        }

        private void OnDrawGizmos()
        {
            var boardWidth = Config.boardSize.x * this.gridSize;
            var boardHeight = Config.boardSize.y * this.gridSize;
            var basePosition = this.transform.position;
            basePosition.x = basePosition.x - (boardWidth / 2) + (this.gridSize / 2);
            basePosition.y = basePosition.y - (boardHeight / 2) + (this.gridSize / 2);

            // board
            for (var x = 0; x < Config.boardSize.x; x++)
            {
                for (var y = 0; y < Config.boardSize.y; y++)
                {
                    Vector3 position = basePosition + new Vector3(x * this.gridSize, y * this.gridSize, 0);
                    Gizmos.color = this.game.board.GetValue(new Vector2Int(x, y)) == 0 ? this.emptyColor : this.filledColor;
                    Gizmos.DrawCube(position, Vector3.one * this.gridSize * 0.5f);
                }
            }

            // currentTetromino
            if (this.game.currentTetromino != null)
            {
                Vector2Int tetrominoPosition = this.game.currentTetromino.GetPosition();
                var shape = this.game.currentTetromino.GetShape();
                var size = Utils.GetSize(shape);
                for (var x = 0; x < size.x; x++)
                {
                    for (var y = 0; y < size.y; y++)
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

        private void OnGUI()
        {
            GUILayout.Label($"Score:{this.game.score}");
        }
    }
}
