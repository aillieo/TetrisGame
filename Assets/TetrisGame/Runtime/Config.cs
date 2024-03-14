// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using System.Collections.Generic;
    using UnityEngine;

    // https://tetris.wiki/Super_Rotation_System
    public class Config
    {
        public static readonly Vector2Int boardSize = new Vector2Int(10, 20);

        public static readonly Vector2Int spawnPosition = new Vector2Int(3, 18);

        public static readonly Dictionary<TetrominoType, Config> configs = new Dictionary<TetrominoType, Config>();

        public byte[,] shape;
        public Vector2Int spawnOffset;
        public Dictionary<string, Vector2Int[]> wallKicks;

        private static readonly Dictionary<string, Vector2Int[]> wallKickValues = new Dictionary<string, Vector2Int[]>
        {
            { "0R", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) } },
            { "R0", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) } },
            { "R2", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) } },
            { "2R", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) } },
            { "2L", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2) } },
            { "L2", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2) } },
            { "L0", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2) } },
            { "0L", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2) } },
        };

        private static readonly Dictionary<string, Vector2Int[]> wallKickValuesI = new Dictionary<string, Vector2Int[]>
        {
            { "0R", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) } },
            { "R0", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(2, 0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) } },
            { "R2", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) } },
            { "2R", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1) } },
            { "2L", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(2, 0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) } },
            { "L2", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) } },
            { "L0", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1) } },
            { "0L", new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) } },
        };

        private static readonly Dictionary<string, Vector2Int[]> wallKickValuesO = new Dictionary<string, Vector2Int[]>
        {
            { "0R", new Vector2Int[] { } },
            { "R0", new Vector2Int[] { } },
            { "R2", new Vector2Int[] { } },
            { "2R", new Vector2Int[] { } },
            { "2L", new Vector2Int[] { } },
            { "L2", new Vector2Int[] { } },
            { "L0", new Vector2Int[] { } },
            { "0L", new Vector2Int[] { } },
        };

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
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValuesI,
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
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValues,
            };

            configs[TetrominoType.L] = new Config()
            {
                /*
                 *  [ ][ ][x]
                 *  [x][x][x]
                 *  [ ][ ][ ]
                 */
                shape = new byte[3, 3]
                {
                    { 0, 1, 0 },
                    { 0, 1, 0 },
                    { 0, 1, 1 },
                },
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValues,
            };

            configs[TetrominoType.O] = new Config()
            {
                /*
                 *  [x][x]
                 *  [x][x]
                 */
                shape = new byte[2, 2]
                {
                    { 1, 1 },
                    { 1, 1 },
                },
                spawnOffset = new Vector2Int(1, 0),
                wallKicks = wallKickValuesO,
            };

            configs[TetrominoType.S] = new Config()
            {
                /*
                 *  [ ][x][x]
                 *  [x][x][ ]
                 *  [ ][ ][ ]
                 */
                shape = new byte[3, 3]
                {
                    { 0, 1, 0 },
                    { 0, 1, 1 },
                    { 0, 0, 1 },
                },
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValues,
            };

            configs[TetrominoType.T] = new Config()
            {
                /*
                 *  [ ][x][ ]
                 *  [x][x][x]
                 *  [ ][ ][ ]
                 */
                shape = new byte[3, 3]
                {
                    { 0, 1, 0 },
                    { 0, 1, 1 },
                    { 0, 1, 0 },
                },
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValues,
            };

            configs[TetrominoType.Z] = new Config()
            {
                /*
                 *  [x][x][ ]
                 *  [ ][x][x]
                 *  [ ][ ][ ]
                 */
                shape = new byte[3, 3]
                {
                    { 0, 0, 1 },
                    { 0, 1, 1 },
                    { 0, 1, 0 },
                },
                spawnOffset = new Vector2Int(0, -1),
                wallKicks = wallKickValues,
            };
        }
    }
}
