using UnityEngine;
using RedLoader;
using Sons.Gui.Input;

namespace SimpleElevator
{
    public class Mono
    {
        [RegisterTypeInIl2Cpp]
        internal class ElevatorButtonController : MonoBehaviour
        {
            internal static ElevatorButtonController activeButtonController;
            internal LinkUiElement button;
            internal GameObject preFabGameObject;
            internal static int currentFloor;
            internal static int gotoFloor;
            internal static int maxFloor = 15; // You might want to adjust this as per your requirement.


            private void Update()
            {
                // Check if the UI Panel is active
                bool isUIPanelActive = SimpleElevatorUi.IsPanelActive();

                if (this != activeButtonController && isUIPanelActive)
                {
                    return; // Do nothing if this is not the active button and UI panel is active.
                }

                // If the button is not active and the UI Panel is not active, exit.
                if (!button.IsActive && !isUIPanelActive) { return; }

                // If the UI Panel is active but the button is not, close the UI Panel.
                if (isUIPanelActive && !button.IsActive)
                {
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                    return; // Exit here so we don't process other inputs when the UI should be closed.
                }

                // If both are active, handle user inputs.
                if (isUIPanelActive && button.IsActive)
                {
                    HandleUserInputsWhileUIIsActive();
                }

                // If E is pressed while the button is active and the UI Panel is off.
                if (Input.GetKeyDown(KeyCode.E) && button.IsActive && !isUIPanelActive)
                {
                    HandleEKeyPressed();
                }
            }

            private void HandleUserInputsWhileUIIsActive()
            {
                float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

                if (scrollDelta > 0f)
                {
                    if (gotoFloor >= maxFloor)
                    {
                        SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                        _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, $"{maxFloor} FLOORS MAX");
                        return;
                    }
                    gotoFloor++;
                    SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                }
                else if (scrollDelta < 0f)
                {
                    if (gotoFloor <= 0)
                    {
                        SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                        _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, "YOU CAN'T GO UNDER FLOOR 0");
                        return;
                    }
                    gotoFloor--;
                    SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, "Moving Started");
                    // Add delay here if necessary
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                }
            }

            private void HandleEKeyPressed()
            {
                // Activate this button
                activeButtonController = this;

                GenericFunctions.PostLogsToConsole("Pressed UI BUTTON");
                SimpleElevatorUi.SetFloorNumber(currentFloor);
                currentFloor = gotoFloor;
                SimpleElevatorUi.TogglePanelUi("ElevatorUi");
                // Here, I assume GenericFunctions.hostMode is some enum indicating the game's state or mode.
                // I've commented it out since you commented it in the provided code, but you can uncomment and adjust as needed.

                /*
                switch (GenericFunctions.hostMode)
                {
                    case GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer:
                        // Handle single-player logic here
                        break;

                    case GenericFunctions.SimpleElevatorSaveGameType.Multiplayer:
                        // Handle multiplayer logic here
                        break;

                    case GenericFunctions.SimpleElevatorSaveGameType.MultiplayerClient:
                        // Handle multiplayer client logic here
                        break;

                    case GenericFunctions.SimpleElevatorSaveGameType.NotIngame:
                        // Handle not-in-game logic here
                        break;
                }
                */
            }
        }

    }
}
