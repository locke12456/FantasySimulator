using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.utility
{
    class PostitonDection
    {
        public enum Position
        {
            None = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomLeft = 3,
            BottomRight = 4,
        };
        private GameObject Role;
        private float top_ref = 0.6f;
        private float bottom_ref = -0.6f;
        private bool onLeft = false;
        private bool onRight = true;
        private bool onTop = false;
        private bool onBottom = false;
        private Transform transform;
        private Position position;
        public Position GetPosition()
        {
            return position;
        }
        public PostitonDection(GameObject target)
        {
            transform = target.transform;
            position = position_detection();
            switch (position)
            {
                case Position.TopLeft:
                case Position.BottomLeft:
                    onLeft = true;
                    onRight = false;
                    break;
                case Position.BottomRight:
                case Position.TopRight:
                    onLeft = false;
                    onRight = true;
                    break;
            }
        }


        public Vector3 GetScaleHint(Quaternion rotation, float scale_base)
        {
            Vector3 scale = Vector3.zero;
            position = position_detection();
            switch (position)
            {
                case Position.TopLeft:
                    if (!onLeft)
                    {
                        scale = (new Vector3(0, scale_base * -2f, 0));
                        onLeft = true;
                        onRight = false;
                    }
                    break;
                case Position.BottomLeft:
                    if (!onLeft)
                    {
                        scale = (new Vector3(0, scale_base * -2f, 0));
                        onLeft = true;
                        onRight = false;
                    }
                    break;
                case Position.BottomRight:
                    if (!onRight)
                    {
                        scale = (new Vector3(0, scale_base * 2f, 0));
                        onLeft = false;
                        onRight = true;
                    }
                    break;
                case Position.TopRight:
                    if (!onRight)
                    {
                        scale = (new Vector3(0, scale_base * 2f, 0));
                        onLeft = false;
                        onRight = true;
                    }
                    break;
            }
            return scale;
        }
        private Position position_detection()
        {
            if (transform.rotation.z < 0)
            {
                onTop = false;
                onBottom = true;
            }
            else
            {
                onTop = true;
                onBottom = false;
            }
            if (onBottom)
            {
                if (transform.rotation.z > bottom_ref)
                {
                    return Position.BottomRight;
                }
                if (transform.rotation.z < bottom_ref)
                {
                    return Position.BottomLeft;
                }
            }
            if (onTop)
            {
                if (transform.rotation.z < top_ref)
                {
                    return Position.TopRight;
                }
                if (transform.rotation.z > top_ref)
                {
                    return Position.BottomLeft;
                }
            }
            return Position.None;
        }
    }
}

