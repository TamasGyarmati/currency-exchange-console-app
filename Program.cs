using ConsoleLauncher;
using ConsoleLauncher.GUI.Interfaces;
using ConsoleLauncher.GUI.MenuItems;

namespace CurrencyApp;

class Program
{
    static void Main()
    {
        var menu = Launcher.Menu
            .AddItem(new MenuItem("Lekérdezés", () => _ = GetCurrencyData.GetData()))
            .AddExitItem()
            .SetPointerCharacter('^')
            .Build();

        Launcher.Layout.Header.Visible = true;
        Launcher.Layout.Footer.Visible = true;
        Launcher.Layout.Footer.RightItem = new ComponentItem("CurrencyApp", true, (ConsoleColor.Black, ConsoleColor.Green));

        menu.Print();
    }
}