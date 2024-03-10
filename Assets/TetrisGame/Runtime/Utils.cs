// -----------------------------------------------------------------------
// <copyright file="Utils.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoTech.Game
{
    using System;
    using UnityEngine;

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

        public static void RotateArrayInPlace<T>(T[,] array, bool clockwise)
        {
            var width = array.GetLength(0);
            var height = array.GetLength(1);
            if (width != height)
            {
                throw new InvalidOperationException();
            }

            var radius = width / 2;

            for (var r = 0; r < radius; r++)
            {
                var first = r;
                var last = width - r - 1;

                for (var i = first; i < last; i++)
                {
                    var offset = i - first;

                    T temp = array[first, i];

                    // left -> top
                    array[first, i] = clockwise ? array[last - offset, first] : array[first, last - offset];

                    // bottom -> left
                    array[last - offset, first] = clockwise ? array[last, last - offset] : array[i, last];

                    // right -> bottom
                    array[last, last - offset] = clockwise ? array[i, last] : array[last - offset, first];

                    // top -> right
                    array[i, last] = clockwise ? temp : array[first, i];
                }
            }
        }

        public static T[,] Copy<T>(T[,] sourceArray)
        {
            var width = sourceArray.GetLength(0);
            var height = sourceArray.GetLength(1);

            var copy = new T[width, height];
            Array.Copy(sourceArray, copy, sourceArray.Length);
            return copy;
        }

        public static Vector2Int GetSize<T>(T[,] array)
        {
            var width = array.GetLength(0);
            var height = array.GetLength(1);
            return new Vector2Int(width, height);
        }
    }
}
