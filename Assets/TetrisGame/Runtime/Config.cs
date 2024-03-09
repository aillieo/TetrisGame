// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Config
    {
        public static readonly Vector2Int boardSize = new Vector2Int(10, 20);

        public static readonly Vector2Int spawnPosition = new Vector2Int(3, 17);

        public static readonly Dictionary<TetrominoType, Config> configs = new Dictionary<TetrominoType, Config>();

        public byte[,] shape;

        static Config()
        {
            configs[TetrominoType.I] = new Config()
            {
                /*
                 *  [ ][ ][ ][ ]
                 *  [x][x][x][x]
                 *  [ ][ ][ ][ ]
                 *  [ ][ ][ ][ ]
                 */
                shape = new byte[4, 4]
                {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                },
            };

            configs[TetrominoType.J] = new Config()
            {
                /*
                 *  [x][ ][ ]
                 *  [x][x][x]
                 *  [ ][ ][ ]
                 */
                shape = new byte[3, 3]
                {
                    { 0, 1, 1 },
                    { 0, 1, 0 },
                    { 0, 1, 0 },
                },
            };
        }
    }
}
