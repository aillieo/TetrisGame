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
        private byte rotationIndex;

        public Tetromino(TetrominoType tetrominoType)
        {
            this.tetrominoType = tetrominoType;

            var config = Config.configs[tetrominoType];

            this.shape = Utils.Copy(config.shape);
            this.position = Config.spawnPosition + config.spawnOffset;
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

        public void SetPosition(Vector2Int newPosition)
        {
            this.position = newPosition;
        }

        public byte GetRotationIndex()
        {
            return this.rotationIndex;
        }

        public void SetRotationIndex(byte newRotationIndex)
        {
            this.rotationIndex = newRotationIndex;
        }
    }
}
