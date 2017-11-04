using System;
using System.Collections.Generic;
using System.Linq;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;

namespace EndlessVoxelExample.World
{
    public class WorldGenerator : SyncScript
    {
        public TransformComponent Player;

        private const int ViewDistance = 30;

        private IDictionary<Coordinate, List<Entity>> _existingVoxelModels;
        Prefab _voxelPrefab;
        private Coordinate _oldPosition;
        private int _frameCount;

        public override void Start()
        {
            base.Start();

            _existingVoxelModels = new Dictionary<Coordinate, List<Entity>>();
            _voxelPrefab = Content.Load<Prefab>("Prefabs/VoxelPrefab");
        }

        public override void Update()
        {
            Coordinate roundedPlayerPosition = new Coordinate(Player.Position.X, Player.Position.Y, Player.Position.Z);

            if (_oldPosition != roundedPlayerPosition)
            {
                _oldPosition = roundedPlayerPosition;
            }

            DeleteOutOfRangeVoxels(roundedPlayerPosition);
            CreateMissingVoxels(roundedPlayerPosition);


            if (_frameCount % 10 == 0)
            {
                Console.WriteLine("Number of visible voxels: " + _existingVoxelModels.Count);
            }

            _frameCount++;
        }

        private void CreateMissingVoxels(Coordinate roundedPlayerPosition)
        {
            int halfDistance = ViewDistance / 2;
            for (int x = -halfDistance; x <= halfDistance; x++)
            {
                for (int z = -halfDistance; z <= halfDistance; z++)
                {
                    Coordinate surroundingCoordinate = new Coordinate(roundedPlayerPosition.X + x, 0, roundedPlayerPosition.Z + z);
                    CreateVoxelIfNotExists(surroundingCoordinate);
                }
            }
        }

        private void DeleteOutOfRangeVoxels(Coordinate roundedPlayerPosition)
        {
            foreach (Coordinate coordinate in _existingVoxelModels.Keys.ToList())
            {
                if (_existingVoxelModels.ContainsKey(coordinate) && IsOutOfRange(coordinate, roundedPlayerPosition))
                {
                    foreach (Entity entity in _existingVoxelModels[coordinate])
                    {
                        SceneSystem.SceneInstance.RootScene.Entities.Remove(entity);
                    }
                    _existingVoxelModels.Remove(coordinate);
                }
            }
        }

        private bool IsOutOfRange(Coordinate coordinate, Coordinate roundedPlayerPosition)
        {
            return Math.Abs(coordinate.X - roundedPlayerPosition.X) > ViewDistance / 2 ||
                   Math.Abs(coordinate.Z - roundedPlayerPosition.Z) > ViewDistance / 2;
        }

        private void CreateVoxelIfNotExists(Coordinate coordinate)
        {
            if (!_existingVoxelModels.ContainsKey(coordinate))
            {
                List<Entity> entities = _voxelPrefab.Instantiate();
                SceneSystem.SceneInstance.RootScene.Entities.AddRange(entities);

                int y = TerrainGenerator.GetTerrainHeight(coordinate.X, coordinate.Z);
                foreach (var entity in entities)
                {
                    entity.Transform.Position = new Vector3(coordinate.X, y, coordinate.Z);
                }
                _existingVoxelModels[coordinate] = entities;
            }
        }
    }

    public struct Coordinate
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coordinate(float x, float y, float z)
            : this((int)x, (int)y, (int)z)
        {
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Coordinate && Equals((Coordinate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !left.Equals(right);
        }
    }
}