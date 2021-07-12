using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.utility
{
    public class TilemapUtils
    {
        public Tilemap tilemap;
        public TilemapUtils()
        {

        }
        public Tile GetTileWithPosition(Vector3 target)
        {
            Vector3Int position = getPositionFromTilemap(target);

            Tile tile = tilemap.GetTile<Tile>(position);

            return tile;
        }

        public bool RemoveTileWithPositon(Vector3 target)
        {
            Vector3Int position = getPositionFromTilemap(target);

            bool exist = tilemap.HasTile(position);
            if(exist)
            {
                tilemap.SetTile(position, null);
            }
            return exist;
        }
        private Vector3Int getPositionFromTilemap(Vector3 target)
        {
            Vector3 worldPoint = getWorldPoint(target);

            Vector3Int position = tilemap.WorldToCell(worldPoint);
            return position;
        }
        private Vector3 getWorldPoint(Vector3 target)
        {
            // save the camera as public field if you using not the main camera
            Ray ray = Camera.main.ScreenPointToRay(target);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            return worldPoint;
        }

    }
}
