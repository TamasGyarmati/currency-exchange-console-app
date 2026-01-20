using ConsoleLauncher;
using ConsoleLauncher.GUI.MenuItems;
using CurrencyApp.Services;

namespace CurrencyApp.ConsoleLauncherSetup;

public static class ConversionMenu
{
    public static void Show()
    {
        Launcher.Menu
            .AddItem(new MenuItem("EUR → HUF",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.EURtoHUF)))
            .AddItem(new MenuItem("HUF → EUR",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.HUFtoEUR)))
            .AddReturnItem()
            .SetPointerCharacter('>')
            .Print();
    }
}