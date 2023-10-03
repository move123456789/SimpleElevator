using SonsSdk.Attributes;
using System;
using UnityEngine;

namespace SimpleElevator
{
    [AssetBundle("elevator")]
    public static class Assets
    {
        // Prefab With ElevatorFloor And ElevatorControl
        [AssetReference("MainElevator")]
        public static GameObject MainElevator { get; set; }

        [AssetReference("MainElevatorStructureNode")]
        public static GameObject MainElevatorStructureNode { get; set; }

        [AssetReference("ElevatorControlPanel")]
        public static GameObject ElevatorControlPanel { get; set; }

        [AssetReference("cogwheel")]
        public static Texture2D LinkUiIcon { get; set; }

    }
}
