using RedLoader;
using Sons.Crafting.Structures;
using Sons.Gameplay.GameSetup;
using Sons.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using UnityEngine;

namespace SimpleElevator
{
    internal class GenericFunctions
    {
        internal static void PostLogsToConsole(string message)
        {
            if (!Config.DebugLogging.Value) { return; }
            RLog.Msg(message);
        }
        internal static void PostErrorToConsole(string message)
        {
            RLog.Error(message);
        }

        public enum SimpleElevatorSaveGameType
        {
            SinglePlayer,
            Multiplayer,
            MultiplayerClient,
            NotIngame,
        }

        public static SimpleElevatorSaveGameType? hostMode
        {
            get { return GetHostMode(); }
        }

        private static SimpleElevatorSaveGameType? GetHostMode()
        {
            if (!LocalPlayer.IsInWorld) { return SimpleElevatorSaveGameType.NotIngame; }
            var saveType = GameSetupManager.GetSaveGameType();
            switch (saveType)
            {
                case SaveGameType.SinglePlayer:
                    return SimpleElevatorSaveGameType.SinglePlayer;
                case SaveGameType.Multiplayer:
                    return SimpleElevatorSaveGameType.Multiplayer;
                case SaveGameType.MultiplayerClient:
                    return SimpleElevatorSaveGameType.MultiplayerClient;
            }
            return SimpleElevatorSaveGameType.NotIngame;
        }

        internal static GameObject FindNodePrefabFromStructureRecipeDatabase(int desiredRecipeId)
        {
            StructureRecipe foundRecipe;

            bool success = LocalPlayer.StructureCraftingSystem._recipeDatabase.TryGetRecipeById(desiredRecipeId, out foundRecipe);

            if (success)
            {
                GenericFunctions.PostLogsToConsole($"Found recipe with ID {foundRecipe.Id}"); // Assuming StructureRecipe has an Id property
                return foundRecipe._structureNodePrefab;

            }
            else
            {
                GenericFunctions.PostLogsToConsole($"Recipe with ID {desiredRecipeId} not found.");
                return null;
            }
        }
    }
}
