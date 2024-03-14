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
        private readonly Color emptyColor = Color.black;
        private readonly Color filledColor = Color.blue;
        private readonly float screenBorder = 0.08f;
        private readonly float fontSizeFactor = 0.03f;

        private readonly float timeStep = 1f;

        private TetrisGame game = new TetrisGame();
        private float timer;

        private Texture2D texture;

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
                this.UpdateTexture();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                this.game.MoveTetromino(Vector2Int.right);
                this.UpdateTexture();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                this.game.MoveTetromino(Vector2Int.down);
                this.UpdateTexture();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                this.game.RotateTetromino(1);
                this.UpdateTexture();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.game.HardDrop();
                this.UpdateTexture();
            }
            else
            {
                this.timer += Time.deltaTime;
                if (this.timer > this.timeStep)
                {
                    this.timer -= this.timeStep;
                    this.game.MoveTetromino(Vector2Int.down);
                    this.UpdateTexture();
                }
            }
        }

        private void UpdateTexture()
        {
            if (this.texture == null)
            {
                var width = Config.boardSize.x;
                var height = Config.boardSize.y;
                this.texture = new Texture2D(width, height);
                this.texture.filterMode = FilterMode.Point;
            }

            // board
            for (var x = 0; x < Config.boardSize.x; x++)
            {
                for (var y = 0; y < Config.boardSize.y; y++)
                {
                    Gizmos.color = this.game.board.GetValue(new Vector2Int(x, y)) == 0 ? this.emptyColor : this.filledColor;

                    this.texture.SetPixel(x, y, Gizmos.color);
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
                            Gizmos.color = this.filledColor;

                            this.texture.SetPixel(tetrominoPosition.x + x, tetrominoPosition.y + y, Gizmos.color);
                        }
                    }
                }
            }

            this.texture.Apply();
        }

        private void OnGUI()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if (this.texture == null)
            {
                this.UpdateTexture();
            }

            // border
            var border = screenWidth * this.screenBorder;
            Rect textureRect = new Rect(border, border, screenWidth - (2 * border), screenHeight - (2 * border));
            Rect full = new Rect(0, 0, 1, 1);
            GUI.DrawTextureWithTexCoords(textureRect, this.texture, full);

            // label
            var fontSize = (int)(screenHeight * this.fontSizeFactor);
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = fontSize;
            GUILayout.Label($"Score:{this.game.score}", labelStyle);

            // button
            GUIStyle buttonStyle = new GUIStyle("button");
            buttonStyle.fontSize = fontSize;
            if (GUILayout.Button("Restart", buttonStyle))
            {
                this.game = new TetrisGame();
                this.timer = 0f;

                if (this.game.currentTetromino == null)
                {
                    this.game.GenerateNewTetromino();
                }

                this.UpdateTexture();
            }
        }
    }
}
