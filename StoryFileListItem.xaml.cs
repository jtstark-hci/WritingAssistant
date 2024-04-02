using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    public partial class StoryFileListItem : UserControl
    {
        internal StorageFile file;
        EditorPage page;
        internal bool alreadyOpen = false;
        internal DocumentTab tabPage;

        public StoryFileListItem()
        {
            this.InitializeComponent();
        }

        public StoryFileListItem(StorageFile f, EditorPage p)
        {
            this.InitializeComponent();
            file = f;
            page = p;
            fileName.Text = f.DisplayName;

        }

        public void FileListItem_Click(object sender, RoutedEventArgs e)
        {
            tabPage = page.OpenStoryDocTab(this);
        }

        public void ChangeFileName(string name)
        {
            fileName.Text = name;
        }

        private void ListItemFlyout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void FlyoutCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        public void FlyoutDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public void FlyoutRename_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
