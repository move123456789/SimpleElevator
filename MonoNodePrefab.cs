using RedLoader;
using Sons.Crafting.Structures;
using TheForest.Utils;
using UnityEngine;
using System.Collections;
using static RedLoader.RLog;

namespace SimpleElevator
{
    internal class MonoNodePrefab
    {
        [RegisterTypeInIl2Cpp]
        internal class NodePrefabController : MonoBehaviour
        {
            private Vector3 playerPos = LocalPlayer.Transform.position;
            private bool isInside = false;
            private float distance = 0f;
            private float maxDistance = 2f;
            internal StructureCraftingNode nodePrefabController;
            internal GameObject nodePrefab;
            public float checkInterval = 0.2f; // Check every 0.2 seconds

            private void Start()
            {
                StartCoroutine(DistanceCheck().WrapToIl2Cpp());
            }

            private IEnumerator DistanceCheck()
            {
                while (true)
                {
                    // Update the player's position
                    playerPos = LocalPlayer.Transform.position;

                    // Calculate distance between player and the nodePrefab
                    distance = Vector3.Distance(playerPos, nodePrefab.transform.position);

                    // Check if the distance is less than 2f and the player was previously outside
                    if (distance < maxDistance && !isInside)
                    {
                        nodePrefabController.GrabEnter();
                        isInside = true;
                    }
                    // Check if the player remains inside the boundary
                    else if (distance < maxDistance && isInside)
                    {
                        nodePrefabController.GrabStay();
                    }
                    // Check if the player moved out of the boundary
                    else if (distance >= maxDistance && isInside)
                    {
                        nodePrefabController.GrabExit();
                        isInside = false;
                    }

                    yield return new WaitForSeconds(checkInterval);
                }
            }
        }
    }
}
