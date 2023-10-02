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
            internal static int previousFloor;


            public float moveSpeed = 2f;
            private Vector3 targetPosition;
            private bool shouldMoveElevator = false;
            private Vector3 startPosition;

            public void InitializeController(GameObject elevatorObject)
            {
                preFabGameObject = elevatorObject;
                startPosition = preFabGameObject.transform.position;
                GenericFunctions.PostLogsToConsole($"Initialized start position: {startPosition}");
            }


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

                if (shouldMoveElevator)
                {
                    if (GenericFunctions.hostMode == GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer)
                    {
                        float currentDistance = Vector3.Distance(preFabGameObject.transform.position, targetPosition);
                        GenericFunctions.PostLogsToConsole($"Current Distance to Target: {currentDistance}, Target Position: {targetPosition}");

                        float step = moveSpeed * Time.deltaTime;
                        Vector3 moveDirection = (targetPosition.y > preFabGameObject.transform.position.y) ? Vector3.up : Vector3.down;
                        preFabGameObject.transform.position += moveDirection * step;

                    }
                }

                if (Vector3.Distance(preFabGameObject.transform.position, targetPosition) <= 0.5f)
                {
                    GenericFunctions.PostLogsToConsole("shouldMoveElevator = false 0.5f");
                    shouldMoveElevator = false;
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

                    switch (GenericFunctions.hostMode)
                    {
                        case GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer:
                            // Handle single-player logic here

                            SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                            GenericFunctions.PostLogsToConsole($"SetGotoFloorMessage to: {gotoFloor}");
                            GenericFunctions.PostLogsToConsole($"Before moving elevator: currentFloor = {currentFloor}, gotoFloor = {gotoFloor}");
                            previousFloor = currentFloor;  // Keep track of the previous floor before updating the current floor
                            currentFloor = gotoFloor;
                            SimpleElevatorUi.SetFloorNumber(currentFloor);
                            GenericFunctions.PostLogsToConsole($"SetFloorNumber to: {currentFloor}");

                            // Move the elevator to the selected floor
                            GenericFunctions.PostLogsToConsole($"Going to floor: {gotoFloor}");
                            MoveElevatorToFloor(gotoFloor);



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
                }
                
            }

            private void HandleEKeyPressed()
            {

                // Activate this button
                activeButtonController = this;

                GenericFunctions.PostLogsToConsole("Pressed UI BUTTON");
                //gotoFloor = 0;
                SimpleElevatorUi.SetFloorNumber(currentFloor);
                SimpleElevatorUi.TogglePanelUi("ElevatorUi");
            }

            private void MoveElevatorToFloor(int floorNumber)
            {
                GenericFunctions.PostLogsToConsole("Start position: " + startPosition);

                if (floorNumber == 0)
                {
                    targetPosition = startPosition;
                }
                else
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y + (floorNumber - previousFloor) * 2, startPosition.z);
                }

                GenericFunctions.PostLogsToConsole("Target position: " + targetPosition);

                // Start moving the elevator
                shouldMoveElevator = true;
            }

        }

    }
}
