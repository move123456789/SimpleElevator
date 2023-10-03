using Sons.Gui.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using UnityEngine;
using static SimpleElevator.Mono;

namespace SimpleElevator
{
    internal class Debug
    {
        internal static Shader sotfShader;

        public static void SpawnPrefab(GameObject prefabName, Vector3 spawnPos = default, Quaternion spawnRotation = default)
        {
            
            if (spawnPos == default(Vector3))
            {
                Vector3 spawnPosition = LocalPlayer.Transform.position; // Use player's position
                spawnPos = spawnPosition;
            }
            if (spawnRotation == default(Quaternion))
            {
                Quaternion spawnRot = Quaternion.Euler(0, 0, 0); // Use player's position
                spawnRotation = spawnRot;
            }
            if (prefabName == Assets.MainElevator)
            {
                SpawnElevator(spawnPos, spawnRotation);
            } 
            else if (prefabName == Assets.ElevatorControlPanel)
            {
                //Quaternion ElevatorSpawn = new Quaternion(270, 0, 0, 0);
                //Quaternion ElevatorSpawn = LocalPlayer.Transform.rotation;
                Quaternion rotation = Quaternion.Euler(270, 0, 0);
                SpawnElevatorControlPanel(spawnPos, rotation);
            }
        }

        private static void SpawnElevator(Vector3 spawnPos, Quaternion spawnRotation)
        {
            // Instantiate the 3D model and apply the texture

            GameObject game_obj_instance = UnityEngine.Object.Instantiate(Assets.MainElevator, spawnPos, spawnRotation);
            if (game_obj_instance != null)
            {
                ElevatorButtonController script_for_ui_element = game_obj_instance.transform.GetChild(1).gameObject.AddComponent<ElevatorButtonController>();

                script_for_ui_element.InitializeController(game_obj_instance); // ADDS INITILISED GAMEOBJECT
                GenericFunctions.PostLogsToConsole($"Prefab Spawned In Linked name: {game_obj_instance.name}");

                game_obj_instance.SetActive(true);
                game_obj_instance.layer = LayerMask.NameToLayer("Default");

                if (sotfShader == null)
                {
                    GenericFunctions.PostErrorToConsole("SOTF Shader Not been found yet");
                    return;
                }
                //Assets.ElevatorMeterial.shader = sotfShader;

                LinkUiElement linkUi = game_obj_instance.transform.GetChild(1).gameObject.AddComponent<LinkUiElement>();
                linkUi._applyMaterial = false;
                linkUi._applyText = false;
                linkUi._applyTexture = true;
                linkUi._texture = Assets.LinkUiIcon;
                linkUi._maxDistance = 2;
                linkUi._worldSpaceOffset = new Vector3(0, (float)0.5, 0);
                linkUi._text = "";
                linkUi._uiElementId = "screen.take";
                linkUi.enabled = false;
                linkUi.enabled = true;

                script_for_ui_element.button = linkUi;

            }
            else
            {
                GenericFunctions.PostErrorToConsole("Failed to instantiate model!");
            }
        }

        private static void SpawnElevatorControlPanel(Vector3 spawnPos, Quaternion spawnRotation)
        {
            // Instantiate the 3D model and apply the texture

            GameObject game_obj_instance = UnityEngine.Object.Instantiate(Assets.ElevatorControlPanel, spawnPos, spawnRotation);
            if (game_obj_instance != null)
            {
                ElevatorCallButton script_for_elevator_call = game_obj_instance.AddComponent<ElevatorCallButton>();

                script_for_elevator_call.InitializeController(game_obj_instance); // ADDS INITILISED GAMEOBJECT
                GenericFunctions.PostLogsToConsole($"Prefab Spawned In Linked name: {game_obj_instance.name}");

                game_obj_instance.SetActive(true);
                game_obj_instance.layer = LayerMask.NameToLayer("Default");

                if (sotfShader == null)
                {
                    GenericFunctions.PostErrorToConsole("SOTF Shader Not been found yet");
                    return;
                }
                //Assets.ElevatorMeterial.shader = sotfShader;

                LinkUiElement linkUi = game_obj_instance.AddComponent<LinkUiElement>();
                linkUi._applyMaterial = false;
                linkUi._applyTexture = true;
                linkUi._texture = Assets.LinkUiIcon;
                linkUi._maxDistance = 2;
                linkUi._worldSpaceOffset = new Vector3(0, (float)0.5, 0);
                linkUi._text = "Call Elevator";
                linkUi._uiElementId = "screen.take";
                linkUi._applyText = true;
                linkUi.enabled = false;
                linkUi.enabled = true;

                script_for_elevator_call.callButton = linkUi;

            }
            else
            {
                GenericFunctions.PostErrorToConsole("Failed to instantiate model!");
            }
        }
    }
}
