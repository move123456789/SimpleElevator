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
            internal LinkUiElement button;
            internal ElevatorButtonController buttonController;
            internal GameObject preFabGameObject;
            internal static int currentFloor;
            internal static int gotoFloor;
            internal static int maxFloor;

            private void Update()
            {
                // IF BOTH IS NOT ACTIVE
                if (!button.IsActive && !SimpleElevatorUi.IsPanelActive()) { return; }

                // IF UI IS ACTIVE BUT LINKUIELEMENT IS NOT
                //if (SimpleElevatorUi.IsPanelActive() && !button.IsActive)
                //{
                //    SimpleElevatorUi.ClosePanel("ElevatorUi");
                //}

                // IF BOTH ARE ACTIVE
                if (SimpleElevatorUi.IsPanelActive() && button.IsActive)
                {
                    float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

                    if (scrollDelta > 0f)
                    {
                        if (gotoFloor == 15)
                        {
                            SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                            _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, $"15 FLOORS MAX");
                            return;
                        }
                        gotoFloor++;
                        SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                    }
                    else if (scrollDelta < 0f)
                    {
                        if (gotoFloor == 0)
                        {
                            SimpleElevatorUi.SetGotoFloorMessage(gotoFloor);
                            _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, $"YOU CANT GO UNDER FLOOR 0");
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
                        _ = SimpleElevatorUi.SendUiMessage(SimpleElevatorUi.SendMessage, $"Moving Started");
                        // NEED TO ADD DELAY
                        SimpleElevatorUi.ClosePanel("ElevatorUi");
                    }
                }

                // If E is pressed while UiButton Is Active and Panelet er av
                if (Input.GetKeyDown(KeyCode.E) && button.IsActive && !SimpleElevatorUi.IsPanelActive())
                {
                    GenericFunctions.PostLogsToConsole("Pressed UI BUTTON");
                    SimpleElevatorUi.SetFloorNumber(currentFloor);
                    currentFloor = gotoFloor;
                    SimpleElevatorUi.TogglePanelUi("ElevatorUi");

                    //switch (GenericFunctions.hostMode)
                    //{
                    //    case GenericFunctions.SimpleElevatorSaveGameType.SinglePlayer:
                    //        return;
                    //    case GenericFunctions.SimpleElevatorSaveGameType.Multiplayer:
                    //        return;
                    //    case GenericFunctions.SimpleElevatorSaveGameType.MultiplayerClient:
                    //        return;
                    //    case GenericFunctions.SimpleElevatorSaveGameType.NotIngame:
                    //        return;

                    //}
                }

            }
        }
    }
}
