using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    /// <summary>
    /// This page opens when launching the application. This is where the user chooses a file to open or creates a new file.
    /// </summary>
    public sealed partial class EntryPage : Page
    {
        List<string> filesList = new List<string>();
        UserProject project;


        public EntryPage()
        {
            this.InitializeComponent();
            
            List<(int, string)> projects = App.GetProjectNames();
            foreach ((int,string) project in projects)
            {
                ProjectLoadCard projCard = new ProjectLoadCard(project.Item2, project.Item1);
                ProjectList.Children.Add(projCard);
            }
        }

        private void NewProj_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectArea.Visibility = Visibility.Visible;
        }

        private void filesOrNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (filesOrNo.SelectedIndex)
            {
                case 0:
                    //browse for files to add
                    browseArea.Visibility = Visibility.Visible;
                    fromScratchArea.Visibility = Visibility.Collapsed;
                    break;


                case 1:
                    //start a new project from scratch
                    fromScratchArea.Visibility = Visibility.Visible;
                    browseArea.Visibility = Visibility.Collapsed;
                    break;


                default:
                    break;
            }
        }


        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.m_window;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".rtf");

            // Open the picker for the user to pick a file
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("");
                foreach (StorageFile file in files)
                {
                    output.Append(file.Name + "\n");
                    filesList.Add(file.Path);
                }
                FileNames.Text = output.ToString();
                CreateButton.Visibility = Visibility.Visible;
                Debug.WriteLine("filesList length: " + filesList.Count);
            }

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            project = new UserProject(projNameEntry.Text, filesList);
            Frame.Navigate(typeof(DocPage), project, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void CreateFromScratchButton_Click(object sender, RoutedEventArgs e)
        {
            project = new UserProject(projNameEntry.Text);
            Frame.Navigate(typeof(DocPage), project, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
