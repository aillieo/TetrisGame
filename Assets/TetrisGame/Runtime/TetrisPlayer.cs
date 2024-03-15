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
        private static readonly Color32 emptyColor0 = new Color32(0, 0, 0, 255);
        private static readonly Color32 emptyColor1 = new Color32(20, 20, 20, 255);
        private static readonly Color32 filledColor0 = new Color32(50, 46, 232, 255);
        private static readonly Color32 filledColor1 = new Color32(56, 52, 252, 255);
        private static readonly Color32 filledColor2 = new Color32(86, 102, 255, 255);
        private static readonly float screenBorder = 20f;
        private static readonly float fontSizeFactor = 0.03f;

        private static readonly float timeStep = 1f;

        private Color32[] buffer;

        private TetrisGame game = new TetrisGame();
        private float timer;

        private Texture2D texture;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

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
                if (this.timer > timeStep)
                {
                    this.timer -= timeStep;
                    this.game.MoveTetromino(Vector2Int.down);
                    this.UpdateTexture();
                }
            }
        }

        private void UpdateTexture()
        {
            var width = Config.boardSize.x;
            var height = Config.boardSize.y;

            if (this.texture == null)
            {
                this.texture = new Texture2D(width, height);
                this.texture.filterMode = FilterMode.Point;
            }

            if (this.buffer == null)
            {
                this.buffer = new Color32[width * height];
            }

            // board
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var value = this.game.board.GetValue(new Vector2Int(x, y));

                    if (value == 1)
                    {
                        if ((x + y) % 2 == 0)
                        {
                            this.buffer[(y * width) + x] = filledColor0;
                        }
                        else
                        {
                            this.buffer[(y * width) + x] = filledColor1;
                        }
                    }
                    else
                    {
                        if ((x + y) % 2 == 0)
                        {
                            this.buffer[(y * width) + x] = emptyColor0;
                        }
                        else
                        {
                            this.buffer[(y * width) + x] = emptyColor1;
                        }
                    }
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
                            var xw = tetrominoPosition.x + x;
                            var yw = tetrominoPosition.y + y;

                            this.buffer[(yw * width) + xw] = filledColor2;
                        }
                    }
                }
            }

            this.texture.SetPixels32(this.buffer);

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
            var aspectRatio = Config.boardSize.y / Config.boardSize.x;
            var heightInner = screenHeight - (2 * screenBorder);
            var widthInner = screenHeight - (2 * screenBorder);
            widthInner = Mathf.Min(heightInner / aspectRatio, widthInner);
            heightInner = widthInner * aspectRatio;
            var startX = (screenWidth - widthInner) / 2;
            var startY = (screenHeight - heightInner) / 2;
            var textureRect = new Rect(startX, startY, widthInner, heightInner);
            var full = new Rect(0, 0, 1, 1);
            GUI.DrawTextureWithTexCoords(textureRect, this.texture, full);

            // label
            var fontSize = (int)(screenHeight * fontSizeFactor);
            if (this.labelStyle == null)
            {
                this.labelStyle = new GUIStyle("label");
                this.labelStyle.fontSize = fontSize;
            }

            if (!this.game.gameOver)
            {
                GUILayout.Label($"Score:{this.game.score}", this.labelStyle);
            }
            else
            {
                GUILayout.Label($"Game over, Score:{this.game.score}", this.labelStyle);
            }

            if (this.game.gameOver)
            {
                // button
                if (this.buttonStyle == null)
                {
                    this.buttonStyle = new GUIStyle("button");
                    this.buttonStyle.fontSize = fontSize;
                }

                if (GUILayout.Button("Restart", this.buttonStyle))
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
}
