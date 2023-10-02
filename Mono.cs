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
            internal static int maxFloor = 15;

            public float moveSpeed = 2f;
            private Vector3 targetPosition;
            private bool shouldMoveElevator = false;
            private Vector3 startPosition;
            private bool elevatorStopped = false;

            private float[] floorHeights = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };  // Add more values if needed. 

            public void InitializeController(GameObject elevatorObject)
            {
                preFabGameObject = elevatorObject;
                startPosition = preFabGameObject.transform.position;
                GenericFunctions.PostLogsToConsole($"Initialized start position: {startPosition}");
            }

            private void Update()
            {

                // For movement of the elevator, it shouldn't matter if the button/UI is active.
                MoveElevator();

                bool isUIPanelActive = SimpleElevatorUi.IsPanelActive();
                if (this != activeButtonController && isUIPanelActive)
                {
                    return;
                }

                if (!button.IsActive && !isUIPanelActive) { return; }
                if (isUIPanelActive && !button.IsActive)
                {
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                    return;
                }
                if (isUIPanelActive && button.IsActive)
                {
                    HandleUserInputsWhileUIIsActive();
                }
                if (Input.GetKeyDown(KeyCode.E) && button.IsActive && !isUIPanelActive)
                {
                    HandleEKeyPressed();
                }
            }

            private void MoveElevator()
            {
                if (shouldMoveElevator)
                {
                    if (GenericFunctions.hostMode == GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer)
                    {
                        float step = moveSpeed * Time.deltaTime;
                        Vector3 moveDirection = (targetPosition.y > preFabGameObject.transform.position.y) ? Vector3.up : Vector3.down;
                        preFabGameObject.transform.position += moveDirection * step;
                    }
                }

                if (!elevatorStopped && Vector3.Distance(preFabGameObject.transform.position, targetPosition) <= 0.5f)
                {
                    GenericFunctions.PostLogsToConsole("shouldMoveElevator = false 0.5f");
                    shouldMoveElevator = false;
                    elevatorStopped = true; // set the flag so we don't log again
                }
            }

            private void HandleUserInputsWhileUIIsActive()
            {
                float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

                if (scrollDelta > 0f && gotoFloor < maxFloor)
                {
                    gotoFloor++;
                    SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                }
                else if (scrollDelta < 0f && gotoFloor > 0)
                {
                    gotoFloor--;
                    SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    SimpleElevatorUi.ClosePanel("ElevatorUi");
                    switch (GenericFunctions.hostMode)
                    {
                        case GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer:
                            SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                            currentFloor = gotoFloor;
                            SimpleElevatorUi.SetFloorNumber(currentFloor);
                            MoveElevatorToFloor(gotoFloor);
                            break;

                        case GenericFunctions.SimpleElevatorSaveGameType.Multiplayer:
                            break;
                        case GenericFunctions.SimpleElevatorSaveGameType.MultiplayerClient:
                            break;
                        case GenericFunctions.SimpleElevatorSaveGameType.NotIngame:
                            break;
                    }
                }
            }

            private void HandleEKeyPressed()
            {
                activeButtonController = this;
                SimpleElevatorUi.SetFloorNumber(currentFloor);
                SimpleElevatorUi.TogglePanelUi("ElevatorUi");
            }

            private void MoveElevatorToFloor(int floorNumber)
            {
                if (floorNumber < 0 || floorNumber > maxFloor)
                {
                    GenericFunctions.PostLogsToConsole("Invalid floor number!");
                    return;
                }

                float desiredHeight = startPosition.y + (floorNumber * 2);
                targetPosition = new Vector3(startPosition.x, desiredHeight, startPosition.z);
                shouldMoveElevator = true;
            }

        }
    }
}
