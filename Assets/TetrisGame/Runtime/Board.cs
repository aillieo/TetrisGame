// -----------------------------------------------------------------------
// <copyright file="Board.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using UnityEngine;

    public class Board
    {
        private readonly byte[,] grid;

        public Board()
        {
            var size = Config.boardSize;
            this.grid = new byte[size.x, size.y];
        }

        public void SetValue(Vector2Int position, byte value)
        {
            this.grid[position.x, position.y] = value;
        }

        public byte GetValue(Vector2Int position)
        {
            return this.grid[position.x, position.y];
        }
    }
}
