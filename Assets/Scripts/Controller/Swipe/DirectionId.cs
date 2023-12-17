using UnityEngine;

namespace GG.Infrastructure.Utils.Swipe
{
    public class DirectionId
    {
        // Direction IDs
        public const string ID_UP = "Up";
        public const string ID_DOWN = "Down";
        public const string ID_LEFT = "Left";
        public const string ID_RIGHT = "Right";
        public const string ID_UP_LEFT = "UpLeft";
        public const string ID_UP_RIGHT = "UpRight";
        public const string ID_DOWN_LEFT = "DownLeft";
        public const string ID_DOWN_RIGHT = "DownRight";

        // Properties
        public readonly string Id;
        public readonly Vector3 Direction;

        // Constructor
        public DirectionId(string id, Vector3 direction)
        {
            Id = id;
            Direction = direction;
        }
    }
}
