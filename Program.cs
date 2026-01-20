using ConsoleLauncher;
using ConsoleLauncher.GUI.Interfaces;
using ConsoleLauncher.GUI.MenuItems;
using CurrencyApp.Services;

namespace CurrencyApp;

class Program
{
    static void Main()
    {
        var menu = Launcher.Menu
            .AddItem(new MenuItem("Convert", ShowConversionMenu))
            .AddExitItem()
            .SetPointerCharacter('^')
            .Build();

        Launcher.Layout.Header.Visible = true;
        Launcher.Layout.Footer.Visible = true;
        Launcher.Layout.Footer.RightItem = new ComponentItem("CurrencyApp", true, (ConsoleColor.Black, ConsoleColor.Green));

        menu.Print();
    }
    
    private static void ShowConversionMenu()
    {
        Launcher.Menu
            .AddItem(new MenuItem("EUR → HUF", () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.EURtoHUF)))
            .AddItem(new MenuItem("HUF → EUR", () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.HUFtoEUR)))
            .AddReturnItem()
            .AddExitItem()
            .SetPointerCharacter('>')
            .Print();
    }
}