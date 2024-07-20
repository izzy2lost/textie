using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.StartScreen;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace Textie
{
    public sealed partial class App : Application
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public App()
        {
            if (!localSettings.Values.ContainsKey("theme"))
            {
                localSettings.Values.Add("theme", "WD");
            }
            else
            {
                string theme = localSettings.Values["theme"].ToString();
                if (theme != "WD")
                {
                    if (theme == "Dark")
                    {
                        RequestedTheme = ApplicationTheme.Dark;
                    }
                    else if (theme == "Light")
                    {
                        RequestedTheme = ApplicationTheme.Light;
                    }
                }
            }

            if (!localSettings.Values.ContainsKey("transparency"))
            {
                localSettings.Values.Add("transparency", "1");
            }

            if (!localSettings.Values.ContainsKey("TextBoxTheme"))
            {
                localSettings.Values.Add("TextBoxTheme", "Light");
            }

            if (!localSettings.Values.ContainsKey("titleBarColor"))
            {
                localSettings.Values.Add("titleBarColor", "0");
            }

            if (!localSettings.Values.ContainsKey("FontFamily"))
            {
                localSettings.Values.Add("FontFamily", "Segoe UI");
            }

            if (!localSettings.Values.ContainsKey("SearchEngine"))
            {
                localSettings.Values.Add("SearchEngine", "Bing");
            }

            if (!localSettings.Values.ContainsKey("vibrate"))
            {
                localSettings.Values.Add("vibrate", "1");
            }

            if (!localSettings.Values.ContainsKey("autoSave"))
            {
                localSettings.Values.Add("autoSave", "0");
            }

            if (!localSettings.Values.ContainsKey("trimNewLines"))
            {
                localSettings.Values.Add("trimNewLines", "0");
            }

            if (!localSettings.Values.ContainsKey("hotExit"))
            {
                localSettings.Values.Add("hotExit", "0");
            }

            if (!localSettings.Values.ContainsKey("flagsEnabled"))
            {
                localSettings.Values.Add("flagsEnabled", "0");
            }

            if (!localSettings.Values.ContainsKey("highContrast"))
            {
                localSettings.Values.Add("highContrast", "0");
            }

            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.GamepadB)
            {
                e.Handled = true; // Suppress the default back behavior for the B button
            }
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            bool canEnablePrelaunch = ApiInformation.IsMethodPresent("Windows.ApplicationModel.Core.CoreApplication", "EnablePrelaunch");

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate 
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (canEnablePrelaunch)
                {
                   TryEnablePrelaunch();
                }

                rootFrame.Navigate(typeof(MainPage), e.Arguments);

                rootFrame.CacheSize = 2;
            }

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                await ConfigureJumpList();
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        // Restore the saved session state only when appropriate 
                    }

                    Window.Current.Content = rootFrame;
                }

                rootFrame.Navigate(typeof(MainPage), args);
                if (rootFrame.Content == null)
                {
                    if (!rootFrame.Navigate(typeof(MainPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                Window.Current.Activate();
            }

            if(args.Kind == ActivationKind.ToastNotification)
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        // Restore the saved session state only when appropriate 
                    }

                    Window.Current.Content = rootFrame;
                }
                rootFrame.Navigate(typeof(MainPage), args);
                if (rootFrame.Content == null)
                {
                    if (!rootFrame.Navigate(typeof(MainPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                Window.Current.Activate();
            }
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate 
                }

                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(MainPage), args);
            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnResuming(object sender, object e)
        {
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

        private void TryEnablePrelaunch()
        {
            Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
        }

        private async Task ConfigureJumpList()
        {
            if (JumpList.IsSupported() == true)
            {
                JumpList jumpList = await JumpList.LoadCurrentAsync();

                if(jumpList.Items.Count == 0)
                {
                    jumpList.Items.Clear();
                    JumpListItem OpenJumpListItem = JumpListItem.CreateWithArguments("OpenFile", "Open File");
                    jumpList.Items.Add(OpenJumpListItem);

                    try
                    {
                        await jumpList.SaveAsync();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}