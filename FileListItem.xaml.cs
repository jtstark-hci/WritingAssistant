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
    public sealed partial class FileListItem : UserControl
    {
        internal StorageFile file;
        DocPage page;
        internal bool alreadyOpen = false;
        internal DocumentTab tab;

        public FileListItem()
        {
            this.InitializeComponent();
        }

        public FileListItem(StorageFile f, DocPage p)
        {
            this.InitializeComponent();
            file = f;
            page = p;
            fileName.Text = f.DisplayName;

        }

        private void FileListItem_Click(object sender, RoutedEventArgs e)
        {
            tab = page.OpenTab(this);
        }

        public void ChangeFileName(string name)
        {
            fileName.Text = name;
        }

        private void ListItemFlyout_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void FlyoutCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FlyoutDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FlyoutRename_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
