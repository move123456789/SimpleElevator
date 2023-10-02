using AssemblyCSharp;
using Sons.Gui.Input;
using SonsSdk;
using SUI;
using TheForest.Utils;
using UnityEngine;
using static SimpleElevator.Mono;

namespace SimpleElevator;

public class SimpleElevator : SonsMod
{
    public SimpleElevator()
    {
        // Don't register any update callbacks here. Manually register them instead.
        // Removing this will call OnUpdate, OnFixedUpdate etc. even if you don't use them.
        HarmonyPatchAll = true;
        OnUpdateCallback = OnUpdate;
    }

    protected override void OnInitializeMod()
    {
        // Do your early mod initialization which doesn't involve game or sdk references here
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        // Do your mod initialization which involves game or sdk references here
        // This is for stuff like UI creation, event registration etc.
        
        SimpleElevatorUi.Create();

        // Adding Ingame CFG
        SettingsRegistry.CreateSettings(this, null, typeof(Config));

        // NOT need due to attribute
        // ClassInjector.RegisterTypeInIl2Cpp<Mono.ElevatorMono>();
    }

    protected override void OnGameStart()
    {
        // This is called once the player spawns in the world and gains control.

        sotfShader = Shader.Find("Sons/HDRPLit");

        GenericFunctions.PostLogsToConsole("ElevatorUIManager instantiated");
    }

    protected void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GenericFunctions.PostLogsToConsole("Pressed LeftArrow");
            SimpleElevatorUi.TogglePanelUi("ElevatorUi");
        }
        if (!LocalPlayer.IsInWorld) { return; }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            GenericFunctions.PostLogsToConsole("Pressed RightControl");
            // Instantiate the 3D model and apply the texture
            Vector3 spawnPosition = LocalPlayer.Transform.position; // Use player's position
            //Quaternion spawnRotation = LocalPlayer.Transform.rotation; // Use player's rotation
            Quaternion spawnRotation = new Quaternion(0, 0, 0, 0); // Use predetermined rotation

            GameObject game_obj_instance = UnityEngine.Object.Instantiate(Assets.MainElevator, spawnPosition, spawnRotation);
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
    }

    internal static Shader sotfShader;
}