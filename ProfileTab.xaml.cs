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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileTab : Page
    {
        internal Profile profile;
        ProfileListItem listItem;


        public ProfileTab(ProfileListItem item)
        {
            this.InitializeComponent();
            listItem = item;
        }

        internal void LoadProfile(Profile p)
        {
            Debug.WriteLine("loading profile into tab");
            this.profile = p;
            tempBlock.Text = p.ToString();
            charName.Text = p.simpleAttributes["first_name"] + " " + p.simpleAttributes["last_name"];
        }

        public void Close()
        {
            listItem.alreadyOpen = false;
            listItem.tabPage = null;
        }


        private void NameCancelButton_Click(object sender, RoutedEventArgs e)
        {
            nameEditBox.Text = "";
            editNameFlyout.Hide();
        }

        private void NameConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            charName.Text = nameEditBox.Text;
            //listItem.ChangeCharName(nameEditBox.Text);
            nameEditBox.Text = "";
            editNameFlyout.Hide();
        }

    }
}
