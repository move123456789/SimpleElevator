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
                // Access the UI manager via the singleton instance.
                ElevatorUIManager uiManager = ElevatorUIManager.Instance;



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
        [RegisterTypeInIl2Cpp]
        internal class ElevatorUIManager : MonoBehaviour
        {
            private static ElevatorUIManager instance;

            public static ElevatorUIManager Instance
            {
                get
                {
                    if (instance == null)
                    {
                        GameObject managerObject = new GameObject("ElevatorUIManager");
                        instance = managerObject.AddComponent<ElevatorUIManager>();
                    }
                    return instance;
                }
            }

            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else if (instance != this)
                {
                    Destroy(gameObject); // Ensures multiple instances aren't present.
                }
            }

            private ElevatorButtonController activeElevator;

            public ElevatorButtonController ActiveElevator
            {
                get => activeElevator;
                set
                {
                    if (activeElevator != null)
                    {
                        // Handle deactivation logic if needed
                    }

                    activeElevator = value;

                    if (activeElevator != null)
                    {
                        // Handle activation logic if needed
                    }
                }
            }

        }
    }
}
