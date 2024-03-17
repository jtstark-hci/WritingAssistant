using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    /// <summary>
    /// This page opens when launching the application. This is where the user chooses a file to open or creates a new file.
    /// </summary>
    public sealed partial class EntryPage : Page
    {
        public EntryPage()
        {
            this.InitializeComponent();
            //this.RequestedTheme = ElementTheme.Light;
        }

        private void NewDoc_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DocPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            FileOpenPicker opener = new FileOpenPicker();

            var window = App.m_window;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(opener, hWnd);


            opener.SuggestedStartLocation =
                PickerLocationId.DocumentsLibrary;
            opener.FileTypeFilter.Add(".rtf");

            StorageFile file = await opener.PickSingleFileAsync();

            if (file != null)
            {
                Frame.Navigate(typeof(DocPage), file, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });

                //using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                //    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                //{
                //    // Load the file into the Document property of the RichEditBox.
                //    //editor.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                //    Frame.Navigate(typeof(DocPage), randAccStream, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight }); 
                //}
            }
        }
    }
}
