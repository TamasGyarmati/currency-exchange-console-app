using ConsoleLauncher;
using ConsoleLauncher.GUI.Interfaces;

namespace CurrencyApp.ConsoleLauncherSetup;

public static class App
{
    public static void Run()
    {
        ConfigureLayout();
        MainMenu.Show();
    }

    private static void ConfigureLayout()
    {
        Launcher.Layout.Header.Visible = true;
        Launcher.Layout.Footer.Visible = true;
        Launcher.Layout.Footer.RightItem =
            new ComponentItem("CurrencyApp", true, (ConsoleColor.Black, ConsoleColor.DarkBlue));
    }
}