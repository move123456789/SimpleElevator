using RedLoader;
using UnityEngine;

namespace SimpleElevator;

public static class Config
{
    public static ConfigCategory SimpleElevator { get; private set; }

    public static ConfigEntry<KeyCode> ToggleMenuKey { get; private set; }
    public static ConfigEntry<bool> DebugLogging { get; private set; }
    public static ConfigEntry<bool> UiTesting { get; private set; }

    public static void Init()
    {
        SimpleElevator = ConfigSystem.CreateCategory("simpleElevator", "SimpleElevator");

        ToggleMenuKey = SimpleElevator.CreateEntry(
            "menu_key_simple_elevator",
            KeyCode.Keypad3,
            "Toggle Menu Key",
            "The key that toggles the Points Menu.");

        DebugLogging = SimpleElevator.CreateEntry(
            "enable_logging_simple_elevator",
            false,
            "Enable Debug Logs",
            "Enables SimpleElevator Debug Logs of the game to the console.");
        
                UiTesting = SimpleElevator.CreateEntry(
            "enable_ui_on_main_page_simple_elevator",
            false,
            "Enable UI Testing",
            "Enables UI to be opened out of the game.");
    }
}