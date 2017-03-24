﻿using System;

namespace K2Collisions.DataStructures
{
    /// <summary>
    /// Spatial hash for broad phase collision detection between dynamic objects.
    /// </summary>
    public class SpatialHash3d<T> : Spatial3d<T>
    {
        // prime numbers used in hash function
        private const int P1 = 73856093;
        private const int P2 = 19349663;
        private const int P3 = 83492791;

        private double _scale, _scaleInv; // scale of implicit grid


        /// <summary>
        /// 
        /// </summary>
        public SpatialHash3d(int binCount, double scale)
            : base(binCount)
        {
            BinScale = scale;
        }


        /// <summary>
        /// Gets or sets the scale of the implicit grid used to discretize coordinates.
        /// Note that setting the scale clears the map.
        /// </summary>
        public double BinScale
        {
            get { return _scale; }
            set
            {
                if (value <= 0.0)
                    throw new ArgumentOutOfRangeException("The scale must be larger than zero");

                _scale = value;
                _scaleInv = 1.0 / _scale;
                Clear();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        internal override void Discretize(Vec3d point, out int i, out int j, out int k)
        {
            i = (int)Math.Floor(point.x * _scaleInv);
            j = (int)Math.Floor(point.y * _scaleInv);
            k = (int)Math.Floor(point.z * _scaleInv);
        }


        /// <summary>
        /// TODO Test performance of different hash functions.
        /// http://cybertron.cg.tu-berlin.de/eitz/pdf/2007_hsh.pdf
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        internal override int ToIndex(int i, int j, int k)
        {
            return SlurMath.Mod2(i * P1 ^ j * P2 ^ k * P3, BinCount);
        }

    }
}
