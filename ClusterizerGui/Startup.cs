using System;
using System.Windows;
using System.Windows.Threading;
using ClusterizerGui.Main;
using ClusterizerGui.Services.Injection;

namespace ClusterizerGui;

internal static class Startup
{
    [STAThread]
    internal static void Main()
    {
        try
        {
            var mainBoot = new ClusterizerGuiBootstrapper();
            var app = new App
            {
                //close the app when the main window is closed (default value is lastWindow)
                ShutdownMode = ShutdownMode.OnMainWindowClose
            };
            app.DispatcherUnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainOnUnhandledException;
            app.Exit += mainBoot.OnExit;
            app.InitializeComponent();

            app.Run(mainBoot.Run<MainWindowView>());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unhandled Exception (will exit after close): {ex} ");
        }
    }

    private static void AppDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show($@"AppDomainOnUnhandledException error in {sender}: Exception - {e}");
    }

    private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"OnUnhandledException error: {e.Exception}");
    }
}