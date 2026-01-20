using ConsoleLauncher;
using ConsoleLauncher.GUI.MenuItems;

namespace CurrencyApp.ConsoleLauncherSetup;

public static class MainMenu
{
    public static void Show()
    {
        Launcher.Menu
            .AddItem(new MenuItem("Convert", ConversionMenu.Show))
            .AddExitItem()
            .SetPointerCharacter('^')
            .Print();
    }
}