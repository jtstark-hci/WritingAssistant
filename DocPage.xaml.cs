using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Text;
using Windows.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
    
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocPage : Page
    {

        //Comment comment = null;

        public DocPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //update this so it puts all files into the list instead of opening all
            if (e.Parameter is UserProject)
            {

                Debug.WriteLine("parameter was a project");
                UserProject project = (UserProject)e.Parameter;
                App.activeProject = project;

                if (project.isNew)
                {
                    App.SaveNewProject(project);
                    project.isNew = false;
                }


                foreach (string file in project.StoryFiles)
                {
                    Debug.WriteLine("found a file");
                    StorageFile f = await StorageFile.GetFileFromPathAsync(file);
                    TextBlock fileName = new TextBlock();
                    fileName.Text = f.DisplayName;
                    StoryFilesList.Children.Add(fileName);

                    using (IRandomAccessStream randAccStream =
                        await f.OpenAsync(FileAccessMode.Read))
                    {
                        //editor.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
                        //docTitle.Text = f.DisplayName;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            


        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            
        }

        //    //testing
        //    ITextRange range = editor.Document.GetRange(5, 5);
        //    range.CharacterFormat.Hidden = FormatEffect.On;
        //    range.SetText(TextSetOptions.FormatRtf, "test insertion");
        //    Console.WriteLine(range.StartPosition);





        private void RoboButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tabsView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
