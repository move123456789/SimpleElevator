using UnityEngine;
using RedLoader;
using Sons.Gui.Input;
using TheForest.Utils;

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
                        Vector3 nextStepPosition = preFabGameObject.transform.position + moveDirection * step;

                        // Check if the next step would overshoot the target
                        if (moveDirection == Vector3.up && nextStepPosition.y > targetPosition.y || moveDirection == Vector3.down && nextStepPosition.y < targetPosition.y)
                        {
                            preFabGameObject.transform.position = targetPosition;
                            shouldMoveElevator = false;
                        }
                        else
                        {
                            preFabGameObject.transform.position += moveDirection * step;
                        }
                    }
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

            public void MoveElevatorToPosition(float desiredHeight)
            {
                targetPosition = new Vector3(startPosition.x, desiredHeight, startPosition.z);
                shouldMoveElevator = true;
            }

        }


        [RegisterTypeInIl2Cpp]
        internal class ElevatorCallButton : MonoBehaviour
        {
            public float maxRayDistance = 100.0f;  // Maximum distance for raycasting.
            internal LinkUiElement callButton;
            internal GameObject preFabGameObjectControlPanel;
            private static ElevatorButtonController lastContactedElevator;

            public void InitializeController(GameObject elevatorControlObject)
            {
                preFabGameObjectControlPanel = elevatorControlObject;
                GenericFunctions.PostLogsToConsole($"Initialized elevatorControlObject");
            }

            private void Update()
            {
                if (!LocalPlayer.IsInWorld) { return; }
                if (callButton == null) { return; }
                if (callButton.IsActive && Input.GetKeyDown(KeyCode.E))
                {
                    CallElevator();
                }
            }

            public void CallElevator()
            {
                // Raycast upwards
                RaycastHit[] hitsUp = Physics.RaycastAll(transform.position, Vector3.up, maxRayDistance);
                foreach (RaycastHit hit in hitsUp)
                {
                    // Log the hit object's name
                    GenericFunctions.PostLogsToConsole("Raycast hit (Upwards): " + hit.collider.name);
                    if (hit.collider.name == "ElevatorFloor" || hit.collider.name == "MainElevator(Clone)")
                    {
                        // Check the distance to preFabGameObjectControlPanel
                        float distanceToControlPanel = Vector3.Distance(hit.collider.gameObject.transform.position, preFabGameObjectControlPanel.transform.position);
                        if (distanceToControlPanel < 1f)
                        {
                            GenericFunctions.PostLogsToConsole("Elevator and ControlPanel Are Too Close");
                            return;
                        }
                        GenericFunctions.PostLogsToConsole("ElevatorFloor Found");
                        if (hit.collider.name == "ElevatorFloor")
                        {
                            lastContactedElevator = hit.collider.gameObject.transform.GetParent().gameObject.GetComponentInChildren<ElevatorButtonController>();
                        } 
                        else if (hit.collider.name == "MainElevator(Clone)")
                        {
                            lastContactedElevator = hit.collider.gameObject.GetComponentInChildren<ElevatorButtonController>();
                        }
                        
                        if (lastContactedElevator == null)
                        {
                            GenericFunctions.PostErrorToConsole("ElevatorButtonController not found on " + hit.collider.gameObject.name);
                            continue;
                        }
                        MoveElevatorToControlPanelPosition();
                        break; // break out of the loop if we find a hit
                    }
                }

                // Raycast downwards
                RaycastHit[] hitsDown = Physics.RaycastAll(transform.position, Vector3.down, maxRayDistance);
                foreach (RaycastHit hit in hitsDown)
                {
                    // Log the hit object's name
                    GenericFunctions.PostLogsToConsole("Raycast hit (Downwards): " + hit.collider.name);
                    if (hit.collider.name == "ElevatorFloor" || hit.collider.name == "ElevatorFloor(Clone)")
                    {
                        // Check the distance to preFabGameObjectControlPanel
                        float distanceToControlPanel = Vector3.Distance(hit.collider.gameObject.transform.position, preFabGameObjectControlPanel.transform.position);
                        if (distanceToControlPanel < 1f)
                        {
                            GenericFunctions.PostLogsToConsole("Elevator and ControlPanel Are Too Close");
                            return;
                        }
                        GenericFunctions.PostLogsToConsole("ElevatorFloor Found");
                        if (hit.collider.name == "ElevatorFloor")
                        {
                            lastContactedElevator = hit.collider.gameObject.transform.GetParent().gameObject.GetComponentInChildren<ElevatorButtonController>();
                        }
                        else if (hit.collider.name == "MainElevator(Clone)")
                        {
                            lastContactedElevator = hit.collider.gameObject.GetComponentInChildren<ElevatorButtonController>();
                        }
                        if (lastContactedElevator == null)
                        {
                            GenericFunctions.PostErrorToConsole("ElevatorButtonController not found on " + hit.collider.gameObject.name);
                            continue;
                        }
                        MoveElevatorToControlPanelPosition();
                        break; // break out of the loop if we find a hit
                    }
                }
            }

            private void MoveElevatorToControlPanelPosition()
            {
                GenericFunctions.PostLogsToConsole("IN MoveElevatorToControlPanelPosition()");
                if (preFabGameObjectControlPanel == null)
                {
                    GenericFunctions.PostErrorToConsole("preFabGameObjectControlPanel is not assigned.");
                    return;
                }
                if (lastContactedElevator == null)
                {
                    GenericFunctions.PostErrorToConsole("elevator is not assigned.");
                    return;
                }

                float desiredHeight = preFabGameObjectControlPanel.transform.position.y;
                GenericFunctions.PostLogsToConsole($"MoveElevatorToControlPanelPosition() -> desiredHeight: {desiredHeight}");
                // Assuming you have a reference to the ElevatorButtonController class
                lastContactedElevator.MoveElevatorToPosition(desiredHeight);
                GenericFunctions.PostLogsToConsole("lastContactedElevator.MoveElevatorToPosition Started");

            }    
        }
    }
}
