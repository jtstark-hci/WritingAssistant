using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    public sealed partial class ProfileListItem : UserControl
    {
        internal StorageFile file;
        EditorPage page;
        internal bool alreadyOpen = false;
        internal ProfileTab tabPage; //change this to profile page
        internal string fileString;
        internal Profile profile;

        public ProfileListItem()
        {
            this.InitializeComponent();
            //GetProfileFile();
        }

        public ProfileListItem(Profile p, EditorPage p2)
        {
            Debug.WriteLine("creating list item");
            //GetProfileFile();
            this.InitializeComponent();
            profile = p;
            page = p2;
            charName.Text = "test_profile";
            //profile.DeserializeGptProfile("");
        }

        public ProfileListItem(StorageFile f, EditorPage p)
        {
            this.InitializeComponent();
            //GetProfileFile();
            page = p;
            charName.Text = f.DisplayName;

        }

        //public async void GetProfileFile()
        //{
        //    //temp for testing
        //    file = await StorageFile.GetFileFromPathAsync(@"C:\Users\jessi\test_profile.json");
             
        //}

        public void ProfileListItem_Click(object sender, RoutedEventArgs e)
        {
            tabPage = page.OpenProfileTab(this);
            //tabPage.tempBlock.Text = "";
        }

        public void ChangeCharName(string name)
        {
            charName.Text = name;
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
