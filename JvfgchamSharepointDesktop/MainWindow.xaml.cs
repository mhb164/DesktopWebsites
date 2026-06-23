using Microsoft.Web.WebView2.Core;
using System.Reflection;
using System.Windows;

namespace JvfgchamSharepointDesktop;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string? Version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    private readonly string _app = "JvfgchamSharepointDesktop";
    private readonly string _homeUrl = "https://jvfgcham-my.sharepoint.com/";

    public MainWindow()
    {
        InitializeComponent(); 
        InitializeAsync();
        Title = $"{_app} Wrapper v{Version}";
    }

    private async void InitializeAsync()
    {
        string userDataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"WebsiteWrapperData",_app);
        var environment = await CoreWebView2Environment.CreateAsync(
            null,
            userDataFolder
        );

        await WebView.EnsureCoreWebView2Async(environment);

        WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        WebView.CoreWebView2.Settings.AreDevToolsEnabled = false;

        // Show loading
        LoadingOverlay.Visibility = Visibility.Visible;

        // Page loaded
        WebView.CoreWebView2.NavigationCompleted += (s, e) =>
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
        };

        // Restrict navigation
        WebView.CoreWebView2.NavigationStarting += (s, e) =>
        {
            if (!e.Uri.StartsWith(_homeUrl) && !e.Uri.StartsWith("https://login.microsoftonline.com/"))
            {
                e.Cancel = true;
            }

            LoadingOverlay.Visibility = Visibility.Visible;
        };

        WebView.CoreWebView2.Navigate(_homeUrl);
    }
}