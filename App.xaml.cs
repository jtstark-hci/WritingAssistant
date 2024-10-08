﻿using Microsoft.UI.Xaml;
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
using Newtonsoft.Json.Serialization;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Animation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

        static void checkFillDirectory()
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

                    File.WriteAllText(appDataPath + @"\projects.json", JsonConvert.SerializeObject(new ProjectJsonFile()));
                    File.WriteAllText(appDataPath + @"\profiles.json", JsonConvert.SerializeObject(new ProfileJson()));
                    File.WriteAllText(appDataPath + @"\comments.json", JsonConvert.SerializeObject(new CommentJson()));

                    string miscPath = App.appDataPath + @"\misc";
                    DirectoryInfo subDir = Directory.CreateDirectory(miscPath);
                    FileStream fs = File.Create(miscPath + @"\Untitled.rtf");
                    fs.Close();

                }
            } catch (IOException e)
            {
                Debug.WriteLine(e.ToString());
            }


        }

        internal static void CreateProjectDirectory(UserProject project)
        {
            string projectPath = App.appDataPath + @"\project" + project.Id + @"\story_files";
            DirectoryInfo di = Directory.CreateDirectory(projectPath);
        }

        internal static Task<UserProject> CopyProjectFilesAsync(UserProject project)
        {
            return Task.Run(() => CopyProjectFiles(project));
        }


        internal static async Task<UserProject> CopyProjectFiles(UserProject project)
        {
            try
            {
                string projectPath = App.appDataPath + @"\project" + project.Id + @"\story_files";
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(projectPath);

                List<StorageFile> files = new List<StorageFile>();
                foreach (StorageFile file in project.StoryFiles)
                {
                    StorageFile copy = await file.CopyAsync(folder, file.Name, NameCollisionOption.ReplaceExisting);

                    files.Add(copy);


                }

                project.StoryFiles = files.ToList();

            } catch(COMException e)
            {
                Debug.WriteLine("exception");
            }

            return project;

        }


        internal static async void SaveNewProject(UserProject project)
        {
            Debug.WriteLine("saving project");
            string fileContents = File.ReadAllText(appDataPath + @"\projects.json");
            ProjectJsonFile fileJson = JsonConvert.DeserializeObject<ProjectJsonFile>(fileContents);

            project.Id = fileJson.next_id;
            CreateProjectDirectory(project);
            UserProject projAgain = await CopyProjectFiles(project);


            //Deserialize all projects
            List<UserProject> projectsList = new List<UserProject>();
            foreach(string proj in fileJson.projects)
            {
                projectsList.Add(await ProjectJsonReader.DeserializeProjectAsync(proj));
            }
            projectsList.Add(project);


            //Re-serialzie all projects, including the new one
            fileJson.projects = new List<string>();
            foreach(UserProject p in projectsList)
            {
                fileJson.projects.Add(ProjectJsonReader.SerializeProject(p));
            }

            fileJson.next_id++;

            File.WriteAllText(appDataPath + @"\projects.json", JsonConvert.SerializeObject(fileJson));

            fileContents = File.ReadAllText(appDataPath + @"\projectIDs.json");
            List<(int,string)> contentList = JsonConvert.DeserializeObject<List<(int,string)>>(fileContents);
            contentList.Add((project.Id, project.Name));
            File.WriteAllText(appDataPath + @"\projectIDs.json", JsonConvert.SerializeObject(contentList));


        }

        internal static void SaveProject(UserProject project)
        {
            //TO DO: overwrite project in projects.json
            //TO DO: save file contents
        }

        internal static List<(int,string)> GetProjectNames()
        {
            string fileContents = File.ReadAllText(appDataPath + @"\projectIDs.json");
            List<(int, string)> contentList = JsonConvert.DeserializeObject<List<(int, string)>>(fileContents);
            return contentList;
        }


        internal static Task<UserProject> OpenProjectAsync(int id)
        {
            return Task.Run(() => OpenProject(id));
        }

        internal static async Task<UserProject> OpenProject(int id)
        {
            string fileContents = File.ReadAllText(appDataPath + @"\projects.json");
            ProjectJsonFile fileJson = JsonConvert.DeserializeObject<ProjectJsonFile>(fileContents);
            

            if (fileJson.projects.Count > 0)
            {
                //Deserialize all projects until we find the right one
                int index = 0;
                bool found = false;
                UserProject project = null;

                while (index < fileJson.projects.Count && !found)
                {
                    string proj = fileJson.projects[index];
                    project = await ProjectJsonReader.DeserializeProjectAsync(proj);
                    if (project.Id == id)
                    {
                        found = true;
                    } else
                    {
                        index++;
                    }
                }
                activeProject = project;
                return project;
            }
            return null;

        }

        internal static void UserControlNavigationHelper()
        {
            (m_window.Content as Frame).Navigate(typeof(EditorPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }


        public static Window m_window;
        public static string appDataPath;
        internal static UserProject activeProject;

        struct ProjectJsonFile {
            public ProjectJsonFile() { }

            public int next_id = 0;
            public List<string> projects = new List<string>();

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
