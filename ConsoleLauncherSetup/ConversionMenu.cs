using ConsoleLauncher;
using ConsoleLauncher.GUI.MenuItems;
using CurrencyApp.Services;

namespace CurrencyApp.ConsoleLauncherSetup;

public static class ConversionMenu
{
    public static void Show()
    {
        Launcher.Menu
            .AddItem(new MenuItem("HUF → EUR",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.HUFtoEUR)))
            .AddItem(new MenuItem("HUF → USD",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.HUFtoUSD)))
            .AddItem(new MenuItem("EUR → HUF",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.EURtoHUF)))
            .AddItem(new MenuItem("USD → HUF",
                () => _ = GetCurrencyData.CurrencyCalculator(ConversionType.USDtoHUF)))
            .AddReturnItem()
            .SetPointerCharacter('>')
            .Print();
    }
}