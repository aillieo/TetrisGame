// -----------------------------------------------------------------------
// <copyright file="Tetromino.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using UnityEngine;

    public class Tetromino
    {
        public readonly TetrominoType tetrominoType;

        private byte[,] shape;
        private Vector2Int position;

        public Tetromino(TetrominoType tetrominoType)
        {
            this.tetrominoType = tetrominoType;
            this.shape = Config.configs[tetrominoType].shape;
            this.position = Config.spawnPosition;
        }

        public byte[,] GetShape()
        {
            return this.shape;
        }

        public void SetShape(byte[,] newShape)
        {
            this.shape = newShape;
        }

        public Vector2Int GetPosition()
        {
            return this.position;
        }

        public void Move(Vector2Int offset)
        {
            this.position += offset;
        }
    }
}
