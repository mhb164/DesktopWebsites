using Microsoft.Web.WebView2.Core;
using System.Reflection;
using System.Windows;

namespace SchuleInfoportal1Desktop;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string? Version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    private readonly string _app = "SchuleInfoportal1";
    private readonly string _homeUrl = "https://schueler.schule-infoportal.de";
    public MainWindow()
    {
        InitializeComponent();
        InitializeAsync();

        Title = $"SchuleInfoportal1 App Wrapper v{Version}";

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
            if (!e.Uri.StartsWith(_homeUrl))
            {
                e.Cancel = true;
            }

            LoadingOverlay.Visibility = Visibility.Visible;
        };

        WebView.CoreWebView2.Navigate(_homeUrl);
    }
}