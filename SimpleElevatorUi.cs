using SUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SimpleElevator.Mono;
using static SUI.SUI;

namespace SimpleElevator;

public class SimpleElevatorUi
{
    private static readonly Color MainBgBlack = new(0, 0, 0, 0.8f);
    private static readonly Color ComponentBlack = new(0, 0, 0, 0.6f);

    // Data for moving elevator
    internal static SUiElement<SLabelOptions> ElevatorCurrentFloor;
    internal static SUiElement<SLabelOptions> MainMessage;
    internal static SUiElement<SLabelOptions> InfoMessage;
    internal static SUiElement<SLabelOptions> SendMessage;
    internal static SUiElement<SLabelOptions> StartGoFloor;
    internal static Observable<string> textBoxValue = new("");


    public static void Create()
    {
        var panel = RegisterNewPanel("ElevatorUi")
           .Dock(EDockType.Fill).OverrideSorting(100);

        ClosePanel("ElevatorUi");

        var mainContainer = SContainer
            .Dock(EDockType.Fill)
            .Background(MainBgBlack).Margin(400);
        panel.Add(mainContainer);

        ElevatorCurrentFloor = SLabel.Text("Current Floor: 0")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -40)
            .FontSpacing(10);
        ElevatorCurrentFloor.SetParent(mainContainer);

        InfoMessage = SLabel.Text("Scroll Up/Down To Select Floor")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -90)
            .FontSpacing(10);
        InfoMessage.SetParent(mainContainer);

        MainMessage = SLabel.Text("")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -150)
            .FontSpacing(10);
        MainMessage.SetParent(mainContainer);

        StartGoFloor = SLabel.Text("Press Shift To Start")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -190)
            .FontSpacing(10);
        StartGoFloor.SetParent(mainContainer);

        SendMessage = SLabel.Text("")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(40)
            .HFill().Position(null, -240)
            .FontSpacing(10);
        SendMessage.SetParent(mainContainer);
        SendMessage.Visible(false);

    }

    internal static void TogglePanelUi(string panelName)
    {
        TogglePanel(panelName);
    }

    internal static void ClosePanel(string panelName)
    {
        TogglePanel(panelName, false);
    }

    internal static void OpenPanel(string panelName)
    {
        TogglePanel(panelName, true);
    }

    internal static bool IsPanelActive()
    {
        return GetPanel("ElevatorUi").Root.activeSelf;
    }

    internal static async Task SendUiMessage(SUiElement<SLabelOptions> sLabel, string message)
    {
        if (isUiMessageRunning) { return; }
        sLabel.Visible(true);
        sLabel.Text(message);
        isUiMessageRunning = true;
        await Task.Run(Timer);
        sLabel.Visible(false);
    }

    internal static async Task Timer()
    {
        await Task.Delay(2500);
        isUiMessageRunning = false;
    }

    private static bool isUiMessageRunning;

    internal static void SetFloorNumber(int floorNumber)
    {
        ElevatorCurrentFloor.Text($"Floor: {floorNumber}");
    }

    internal static void SetGotoFloorMessage(int message)
    {
        MainMessage.Text($"Selected Floor: {message}");
    }
}