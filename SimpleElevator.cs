using AssemblyCSharp;
using Endnight.Utilities;
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

        Debug.sotfShader = Shader.Find("Sons/HDRPLit");
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
            Debug.SpawnPrefab(Assets.MainElevator);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GenericFunctions.PostLogsToConsole("Pressed DownArrow");
            Debug.SpawnPrefab(Assets.ElevatorControlPanel);
        }
    }

    
}