using Sons.Gui.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleElevator
{
    public class ModifedPrefabs
    {
        internal static GameObject MainElevatorWithScripts;
        public static void MakeMainElevatorWithScripts()
        {
            // Add Required Components to my AssetPrefab
            MainElevatorWithScripts = UnityEngine.Object.Instantiate(Assets.MainElevator);
            if (MainElevatorWithScripts != null)
            {
                Mono.ElevatorButtonController script_for_ui_element = MainElevatorWithScripts.transform.GetChild(1).gameObject.AddComponent<Mono.ElevatorButtonController>();
                if (script_for_ui_element == null)
                {
                    GenericFunctions.PostErrorToConsole("Failed to add ElevatorButtonController!");
                }

                script_for_ui_element.InitializeController(MainElevatorWithScripts); // ADDS INITILISED GAMEOBJECT
                GenericFunctions.PostLogsToConsole($"Prefab Spawned In Linked name: {MainElevatorWithScripts.name}");

                MainElevatorWithScripts.SetActive(true);
                MainElevatorWithScripts.layer = LayerMask.NameToLayer("Default");

                LinkUiElement linkUi = MainElevatorWithScripts.transform.GetChild(1).gameObject.AddComponent<LinkUiElement>();
                if (linkUi == null)
                {
                    GenericFunctions.PostErrorToConsole("Failed to add LinkUiElement!");
                }
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

                script_for_ui_element.InitializLinkUiElement(linkUi);

                if (script_for_ui_element == null)
                {
                    GenericFunctions.PostErrorToConsole("ElevatorButtonController not added!");
                }
                if (linkUi == null)
                {
                    GenericFunctions.PostErrorToConsole("LinkUiElement not added!");
                }


            }
            else
            {
                GenericFunctions.PostErrorToConsole("Failed to instantiate model!");
            }
        }

        //public static GameObject CreateModifiedPrefabMainElevator2()
        //{
        //    // Instantiate a temporary object from the MainElevator prefab
        //    GameObject MainElevatorWithScripts = UnityEngine.Object.Instantiate(Assets.MainElevator);

        //    if (MainElevatorWithScripts != null)
        //    {
        //        ElevatorButtonController script_for_ui_element = MainElevatorWithScripts.transform.GetChild(1).gameObject.AddComponent<ElevatorButtonController>();
        //        if (script_for_ui_element == null)
        //        {
        //            GenericFunctions.PostErrorToConsole("Failed to add ElevatorButtonController!");
        //        }

        //        LinkUiElement linkUi = MainElevatorWithScripts.transform.GetChild(1).gameObject.AddComponent<LinkUiElement>();
        //        if (linkUi == null)
        //        {
        //            GenericFunctions.PostErrorToConsole("Failed to add LinkUiElement!");
        //        }
        //        linkUi._applyMaterial = false;
        //        linkUi._applyText = false;
        //        linkUi._applyTexture = true;
        //        linkUi._texture = Assets.LinkUiIcon;
        //        linkUi._maxDistance = 2;
        //        linkUi._worldSpaceOffset = new Vector3(0, (float)0.5, 0);
        //        linkUi._text = "";
        //        linkUi._uiElementId = "screen.take";
        //        linkUi.enabled = false;
        //        linkUi.enabled = true;

        //        script_for_ui_element.button = linkUi;
        //        if (script_for_ui_element.button == null)
        //        {
        //            GenericFunctions.PostErrorToConsole("Failed to assign button in ElevatorButtonController!");
        //        }

        //        script_for_ui_element.InitializeController(MainElevatorWithScripts); // ADDS INITILISED GAMEOBJECT
        //        GenericFunctions.PostLogsToConsole($"Prefab Spawned In Linked name: {MainElevatorWithScripts.name}");

        //        MainElevatorWithScripts.SetActive(true);
        //        MainElevatorWithScripts.layer = LayerMask.NameToLayer("Default");

        //        if (script_for_ui_element == null)
        //        {
        //            GenericFunctions.PostErrorToConsole("ElevatorButtonController not added!");
        //        }
        //        if (linkUi == null)
        //        {
        //            GenericFunctions.PostErrorToConsole("LinkUiElement not added!");
        //        }


        //    }
        //    else
        //    {
        //        GenericFunctions.PostErrorToConsole("Failed to instantiate model!");
        //    }
        //    return MainElevatorWithScripts;
        //}
    }
}
