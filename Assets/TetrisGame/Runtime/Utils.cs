// -----------------------------------------------------------------------
// <copyright file="Utils.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    public static class Utils
    {
        public static T[,] RotateArray<T>(T[,] inputArray, bool clockwise)
        {
            var width = inputArray.GetLength(0);
            var height = inputArray.GetLength(1);
            var rotatedShape = new T[height, width];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var newX = clockwise ? y : height - y - 1;
                    var newY = clockwise ? width - x - 1 : x;
                    rotatedShape[newX, newY] = inputArray[x, y];
                }
            }

            return rotatedShape;
        }
    }
}
