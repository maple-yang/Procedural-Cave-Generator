﻿using UnityEngine;
using CaveGeneration.MapGeneration;
using System;

namespace CaveGeneration.Modules
{
    /// <summary>
    /// Produces maps based on cellular automata. Outermost boundary always consists of wall tiles.
    /// </summary>
    [CreateAssetMenu(fileName = fileName, menuName = rootMenupath + "Cellular Automata (Default)")]
    public sealed class MapGenCellAutomata : MapGenModule, IRandomizable
    {
        [SerializeField] MapParameters properties = new MapParameters();
        public MapParameters Properties
        {
            get { return properties; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                properties = value;
            }
        }

        const int TUNNEL_RADIUS = 1;

        public override Map Generate()
        {
            Map map = MapBuilder.InitializeRandomMap(properties.Length, properties.Width, properties.InitialDensity, properties.Seed);
            map.TransformBoundary((x, y) => Tile.Wall);
             
            return map
                .Smooth()
                .RemoveSmallFloorRegions(properties.MinFloorSize)
                .ConnectFloors(TUNNEL_RADIUS)
                .SmoothOnlyWalls()
                .RemoveSmallWallRegions(properties.MinWallSize)
                .ApplyBorder(properties.BorderSize);
        }

        int IRandomizable.Seed { set { properties.Seed = value; } }

        void Reset()
        {
            properties = new MapParameters();
        }

        void OnValidate()
        {
            properties.OnValidate();
        }
    } 
}
