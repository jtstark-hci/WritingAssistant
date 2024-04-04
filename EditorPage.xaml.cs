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
using System.Threading;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
    
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditorPage : Page
    {

        //Comment comment = null;

        public EditorPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //update this so it puts all files into the list instead of opening all
            if (e.Parameter is UserProject)
            {

                Debug.WriteLine("parameter was a project");
                UserProject project = (UserProject)e.Parameter;
                App.activeProject = project;
                ProjectTitle.Text = project.Name;
                LoadFiles();
            }
        }

        public void LoadFiles()
        {
            if (App.activeProject != null)
            {
                foreach (StorageFile file in App.activeProject.StoryFiles)
                {
                    Debug.WriteLine("found a file");
                    StoryFileListItem listItem = new StoryFileListItem(file, this);

                    StoryFilesList.Children.Add(listItem);

                }

                ProfileListItem profileListItem = new ProfileListItem(new Profile(), this);
                CharactersList.Children.Add(profileListItem);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveHelper();
        }

        private async void SaveHelper()
        {
            //Debug.WriteLine("Number of files: " + App.activeProject.StoryFiles.Count);
            //loop through all project files, save
            UserProject project = App.activeProject;
            if (project != null)
            {
                foreach(TabViewItem tab in tabsView.TabItems)
                {
                    if (tab.Content.GetType() == typeof(DocumentTab))
                    {
                        DocumentTab dt = (DocumentTab)tab.Content;
                        StorageFile file = dt.GetFileListItem().file;

                        using (IRandomAccessStream randAccStream =
                        await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            dt.GetEditor().Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                        }
                    } else if (tab.Content.GetType() == typeof(ProfileTab))
                    {
                        //save the profile
                    }
                }
            }

            //Later: loop through all character profiles, save
            //Later: save comments

            //Project and filenames saved immediately when changed
        }


        private void RenameProject_Click(object sender, RoutedEventArgs e)
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
            } else if (args.Tab.Content.GetType() == typeof(ProfileTab))
            {
                ProfileTab toClose = (ProfileTab)args.Tab.Content;
                if (toClose != null)
                {
                    toClose.Close();
                }
            }

            sender.TabItems.Remove(args.Tab);
        }


        internal DocumentTab OpenStoryDocTab(StoryFileListItem item)
        {
            DocumentTab tab = null;
            if (item.alreadyOpen)
            {
                Debug.WriteLine("already open");
                //find the right tab and set selecteditem
                foreach (TabViewItem tabItem in tabsView.TabItems)
                {
                    DocumentTab temp = (DocumentTab)tabItem.Content; //will break if there are profile tabs open
                    Debug.WriteLine("checking tab");
                    if (ReferenceEquals(temp, item.tabPage))
                    {
                        Debug.WriteLine("found tab match");
                        tab = temp;
                        tabsView.SelectedItem = tabItem;
                        break;
                    }
                }

            }
            else
            {
                TabViewItem tabItem = new TabViewItem();
                tabItem.Header = item.file.DisplayName;
                tabItem.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
                tab = new DocumentTab(item);
                tabItem.Content = tab;
                item.alreadyOpen = true;

                tabsView.TabItems.Add(tabItem);
                tabsView.SelectedItem = tabItem;
            }

            return tab;
        }


        internal ProfileTab OpenProfileTab(ProfileListItem item)
        {
            ProfileTab tab = null;
            if (item.alreadyOpen)
            {
                Debug.WriteLine("already open");
                //find the right tab and set selecteditem
                foreach (TabViewItem tabItem in tabsView.TabItems)
                {
                    ProfileTab temp = (ProfileTab)tabItem.Content; //breaks if there are doc tabs open
                    Debug.WriteLine("checking tab");
                    if (ReferenceEquals(temp, item.tabPage))
                    {
                        Debug.WriteLine("found tab match");
                        tab = temp;
                        tabsView.SelectedItem = tabItem;
                        break;
                    }
                }

            }
            else
            {
                TabViewItem tabItem = new TabViewItem();
                tabItem.Header = "temp";
                tabItem.IconSource = new SymbolIconSource() { Symbol = Symbol.Document };
                tab = new ProfileTab(item);
                
                tab.LoadProfile(item.profile);
                tabItem.Content = tab;
                item.alreadyOpen = true;

                tabsView.TabItems.Add(tabItem);
                tabsView.SelectedItem = tabItem;
            }

            return tab;
        }
    }
}
