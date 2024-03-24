using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Light;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += onNavigationFailed;
            m_window.Content = rootFrame;

            checkFillDirectory();

            rootFrame.Navigate(typeof(EntryPage), args.Arguments);
            m_window.Activate();
        }

        void onNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load page " + e.SourcePageType.FullName);
        }

        void checkFillDirectory()
        {
            appDataPath = @"c:\writing_assistant_files";
            try
            {
                if (!Directory.Exists(appDataPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(appDataPath);
                    Debug.WriteLine(di.ToString());

                    List<(int,string)> projectIds = new List<(int,string)>();
                    File.WriteAllText(appDataPath + @"\projectIDs.json", JsonConvert.SerializeObject(projectIds));

                    File.WriteAllText(appDataPath + @"\projects.json", JsonConvert.SerializeObject(new ProjectJson()));
                    File.WriteAllText(appDataPath + @"\profiles.json", JsonConvert.SerializeObject(new ProfileJson()));
                    File.WriteAllText(appDataPath + @"\comments.json", JsonConvert.SerializeObject(new CommentJson()));


                }
            } catch (IOException e)
            {
                Debug.WriteLine(e.ToString());
            }


        }

        internal static void SaveNewProject(UserProject project)
        {
            string fileContents = File.ReadAllText(appDataPath + @"\projects.json");
            ProjectJson contentObject = JsonConvert.DeserializeObject<ProjectJson>(fileContents);
            project.Id = contentObject.next_id;
            contentObject.projects.Add(project);
            contentObject.next_id++;

            File.WriteAllText(appDataPath + @"\projects.json", JsonConvert.SerializeObject(contentObject));

            fileContents = File.ReadAllText(appDataPath + @"\projectIDs.json");
            List<(int,string)> contentList = JsonConvert.DeserializeObject<List<(int,string)>>(fileContents);
            contentList.Add((project.Id, project.Name));
            File.WriteAllText(appDataPath + @"\projectIDs.json", JsonConvert.SerializeObject(contentList));

        }

        internal static void SaveProject(UserProject project)
        {
            //TO DO: overwrite project in projects.json
        }

        internal static List<(int,string)> GetProjectNames()
        {
            string fileContents = File.ReadAllText(appDataPath + @"\projectIDs.json");
            List<(int, string)> contentList = JsonConvert.DeserializeObject<List<(int, string)>>(fileContents);
            return contentList;
        }


        public static Window m_window;
        static string appDataPath;
        internal static UserProject activeProject;

        struct ProjectJson {

            public ProjectJson() { }

            public int next_id = 0;
            public List<UserProject> projects = new List<UserProject>();
        }

        struct ProfileJson
        {
            public ProfileJson() { }

            public int next_id = 0;
            public List<Profile> profiles = new List<Profile>();
        }

        struct CommentJson
        {
            public CommentJson() { }

            public int next_id = 0;
            public List<Comment> comments = new List<Comment>();
        }
    }
}
