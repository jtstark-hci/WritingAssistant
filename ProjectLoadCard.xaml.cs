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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    public sealed partial class ProjectLoadCard : UserControl
    {
        public string project = "";
        public int projectId = -1;

        public ProjectLoadCard()
        {
            this.InitializeComponent();
        }

        public ProjectLoadCard(string projectName, int projId)
        {
            this.InitializeComponent();
            this.project = projectName;
            this.projectId = projId;

            projName.Text = project;
        }

        private void projButton_Click(object sender, RoutedEventArgs e)
        {
            //open selected project
        }
    }
}
