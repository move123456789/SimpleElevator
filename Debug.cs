using Construction;
using Sons.Crafting.Structures;
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
            else if (prefabName == Assets.MainElevatorStructureNode)
            {
                SpawnElevatorStructureNode(spawnPos, spawnRotation);
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

        internal static GameObject DeerHideRugStructureNode;

        private static void SpawnElevatorStructureNode(Vector3 spawnPos, Quaternion spawnRotation)
        {
            // Instantiate the 3D model and apply the texture

            GameObject game_obj_instance = UnityEngine.Object.Instantiate(Assets.MainElevatorStructureNode, spawnPos, spawnRotation);
            if (game_obj_instance != null)
            {

                GenericFunctions.PostLogsToConsole($"Prefab Spawned In Linked name: {game_obj_instance.name}");

                game_obj_instance.SetActive(true);
                game_obj_instance.layer = LayerMask.NameToLayer("Default");

                if (DeerHideRugStructureNode == null) { GenericFunctions.PostErrorToConsole("DeerHideRugStructureNode == null"); return; }
                GameObject structureInteractionObjects_from_deerHide = DeerHideRugStructureNode.transform.GetChild(1).gameObject;

                // Instantiate a copy of the object
                GameObject copiedObject = UnityEngine.Object.Instantiate(structureInteractionObjects_from_deerHide);

                // Set the parent of the instantiated object to game_obj_instance
                copiedObject.transform.SetParent(game_obj_instance.transform);

                // Set the localPosition of copiedObject to be the same as game_obj_instance
                copiedObject.transform.localPosition = Vector3.zero;

                // Structure Crafting Node
                StructureCraftingNode structure_crafting_node = game_obj_instance.AddComponent<StructureCraftingNode>();
                GameObject ingredient_ui_template = game_obj_instance.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject; // IngredientUiTemplate
                structure_crafting_node._ingredientUiTemplate = ingredient_ui_template;

                //GameObject ingredient_ui = game_obj_instance.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject;  // IngredientUi
                GameObject ingredient_ui = game_obj_instance.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;  // IngredientUi

                // Empty List
                StructureCraftingNodeIngredientUi structureCraftingNodeIngredientUi = ingredient_ui.GetComponent<StructureCraftingNodeIngredientUi>();
                var ingredientList = new Il2CppSystem.Collections.Generic.List<StructureCraftingNodeIngredientUi>();
                ingredientList.Add(structureCraftingNodeIngredientUi);
                structure_crafting_node._ingredientUi = ingredientList;


                StructureCraftingSystem structureCraftingSystem = LocalPlayer.StructureCraftingSystem;
                structureCraftingSystem._ActiveStructureNode_k__BackingField = structure_crafting_node;  // MAY NEED TO BE REMOVED
                structureCraftingSystem.ActiveStructureNode = structure_crafting_node;  // MAY NEED TO BE REMOVED
                structure_crafting_node._structureCraftingSystem = structureCraftingSystem;

                GameObject cancel_Structure_interaction_element = game_obj_instance.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject; // IngredientUiTemplate
                structure_crafting_node._cancelStructureInteractionElement = cancel_Structure_interaction_element;

                

                // Crafting Ingredient Link And List
                var crafting_ingredient_linkList = new Il2CppSystem.Collections.Generic.List<StructureCraftingNode.CraftingIngredientLink>(); // LIST FOR StructureCraftingNode
                StructureCraftingNode.CraftingIngredientLink craftingIngredientLink = new StructureCraftingNode.CraftingIngredientLink();
                StructureCraftingRecipeIngredient structureCraftingRecipeIngredient = new StructureCraftingRecipeIngredient();
                structureCraftingRecipeIngredient.Count = 1;
                structureCraftingRecipeIngredient.ItemId = 472;
                craftingIngredientLink.Ingredient = structureCraftingRecipeIngredient;
                // Adding structureCraftingRecipeIngredient To List So It Can Be Used In StructureRecipie
                var structureCraftingRecipeIngredientLIST = new Il2CppSystem.Collections.Generic.List<StructureCraftingRecipeIngredient>(); 
                structureCraftingRecipeIngredientLIST.Add(structureCraftingRecipeIngredient);

                // Structure Recipie
                StructureRecipe structureRecipe = new StructureRecipe();
                structureRecipe._alignToSurface = true;
                structureRecipe._anchor = StructureRecipe.AnchorType.Bottom;
                structureRecipe._blockDismantleInProximityWithPlayersOrActors = false;
                structureRecipe._builtPrefab = Assets.MainElevator;
                structureRecipe._canBeRotated = true;
                structureRecipe._category = StructureRecipe.CategoryType.Utility;
                structureRecipe._craftCompleteAudioEvent = "event:/ui/ingame/ui_crafting_complete2";
                structureRecipe._displayName = "Elevator";
                structureRecipe._forceUp = false;
                structureRecipe._id = 88;
                structureRecipe._ingredients = structureCraftingRecipeIngredientLIST;
                structureRecipe._localizationId = "BLUEPRINT_ELEVATOR";
                structureRecipe.name = "ElevatorRecipie";
                structureRecipe._placeMode = StructureRecipe.PlaceModeType.Single;
                structureRecipe._recipeImage = DeerHideRugStructureNode.GetComponent<StructureCraftingNode>()._recipe.RecipeImage;

                structure_crafting_node._recipe = structureRecipe;

                // ElevatorVisibleObject GameObject
                GameObject main_elevator_inside_ingredients = game_obj_instance.transform.GetChild(0).GetChild(0).gameObject;
                StructureCraftingNodeIngredient structureCraftingNodeIngredient = main_elevator_inside_ingredients.AddComponent<StructureCraftingNodeIngredient>();
                structureCraftingNodeIngredient.SetId(472);


                // Crafting Ingredient Link And List Continued
                var crafting_ingredient_link_ingrediants_List = new Il2CppSystem.Collections.Generic.List<StructureCraftingNodeIngredient>(); // LIST FOR StructureCraftingNode.CraftingIngredientLink_ingredients
                StructureCraftingNodeIngredient structureCraftingNodeIngredient1 = game_obj_instance.transform.GetChild(0).GetChild(0).gameObject.GetComponent<StructureCraftingNodeIngredient>();
                crafting_ingredient_link_ingrediants_List.Add(structureCraftingNodeIngredient1); // Adds Ingrediant to List
                craftingIngredientLink._ingredients = crafting_ingredient_link_ingrediants_List; // craftingIngredientLink._ingredients requires list of ingrediants

                crafting_ingredient_linkList.Add(craftingIngredientLink); // Add CraftingIngredientLink To LIST

                structure_crafting_node._craftingIngredientLinks = crafting_ingredient_linkList; // ADDS CraftingingredientLink to the StructureCraftingNode

                //StructureRecipe deerHideRug = DeerHideRugStructureNode.GetComponent<StructureCraftingNode>()._recipe.Ingredients;
                //StructureRecipe myRecipe = new StructureRecipe(deerHideRug);
                //structure_crafting_node._recipe = myRecipe;




                // Free From Structure Node Linker
                FreeFormStructureNodeLinker structure_node_linker = game_obj_instance.AddComponent<FreeFormStructureNodeLinker>();

                // Screw Structure Destruction
                ScrewStructureDestruction screw_structure_destruction = game_obj_instance.AddComponent<ScrewStructureDestruction>();


                // FOR ElevatorVisibleObject GameObject
                StructureGhostSwapper structureGhostSwapper = main_elevator_inside_ingredients.AddComponent<StructureGhostSwapper>();


                structure_crafting_node.GrabExit();

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
