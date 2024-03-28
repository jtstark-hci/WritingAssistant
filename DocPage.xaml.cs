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
using System.Runtime;
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

        private void CreateEmptyStoryTab()
        {
            //DocumentTab tab = new DocumentTab();
            //tabsView.TabItems.Add(tab);
            //tabsView.SelectedItem = tab;
            //weird formatting going on here, must fix
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //update this so it puts all files into the list instead of opening all
            if (e.Parameter is UserProject)
            {

                Debug.WriteLine("parameter was a project");
                UserProject project = (UserProject)e.Parameter;
                App.activeProject = project;
                ProjectTitle.Text = project.Name;

                if (project.isNew)
                {
                    App.SaveNewProject(project);
                    if (!(project.StoryFiles.Count >= 1))
                    {
                        //open blank file
                        CreateEmptyStoryTab();
                    }
                    project.isNew = false;
                }


                foreach (string file in project.StoryFiles)
                {
                    Debug.WriteLine("found a file");
                    StorageFile f = await StorageFile.GetFileFromPathAsync(file);
                    FileListItem listItem = new FileListItem(f, this);

                    StoryFilesList.Children.Add(listItem);

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
            if (args.Tab.Content.GetType() == typeof(DocumentTab))
            {
                DocumentTab toClose = (DocumentTab)args.Tab.Content;
                if (toClose != null)
                {
                    toClose.Close();
                }
            }

            sender.TabItems.Remove(args.Tab);
        }

        internal DocumentTab OpenTab(FileListItem item)
        {
            DocumentTab dt = null;
            if (item.alreadyOpen)
            {
                Debug.WriteLine("already open");
                //find the right tab and set selecteditem
                foreach(TabViewItem tab in tabsView.TabItems)
                {
                    DocumentTab temp = (DocumentTab)tab.Content;
                    Debug.WriteLine("checking tab");
                    if (ReferenceEquals(temp, item.tab))
                    {
                        Debug.WriteLine("found tab match");
                        dt = temp;
                        tabsView.SelectedItem = tab;
                        break;
                    }
                }

            } else
            {
                TabViewItem tab = new TabViewItem();
                tab.Header = item.file.DisplayName;
                tab.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
                dt = new DocumentTab(item);
                tab.Content = dt;
                item.alreadyOpen = true;

                tabsView.TabItems.Add(tab);
                tabsView.SelectedItem = tab;
            }


            return dt;
        }
    }
}
