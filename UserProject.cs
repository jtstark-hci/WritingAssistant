using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Diagnostics;
using Windows.Storage;
using Windows.Media.AppRecording;
using Windows.Foundation.Metadata;
using Microsoft.UI.Composition;
using System.IO;
using Windows.Foundation;
using System.ComponentModel;

namespace WritingAssistant
{
    class UserProject
    {

        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public List<Comment> Comments { get; } = null;
        public List<StorageFile> StoryFiles { get; set; } = new List<StorageFile>();
        public List<Profile> Profiles { get; } = new List<Profile>();

        //TO ADD:
        //notifications list
        //Robo work log

        public UserProject() { }

        public UserProject(string name)
        {
            Name = name;
            Debug.WriteLine("creating project");
            App.SaveNewProject(this);

        }

        public UserProject(string name, List<StorageFile> files)
        {
            Name = name;
            StoryFiles = files;
            Profiles.Add(new Profile());

            App.SaveNewProject(this);

        }

        internal UserProject(int id, string name, List<StorageFile> files)
        {
            Id = id;
            Name = name;
            StoryFiles = files;
        }


        public void AddCharacterToProject(Profile profile)
        {
            Profiles.Add(profile);
        }

        public void SaveEditorState()
        {

        }

        public void AddFileToProject(StorageFile file)
        {

        }

        public void AddCommentToProject(Comment comment)
        {

        }
    }

    public class ProjectJsonReader 
    {
        private class TempJson
        {
            public int id;
            public string name;
            public string files;

            public TempJson(int i, string n, string f)
            {
                id = i;
                files = f;
                name = n;                
            }
        }

        internal static string SerializeProject(UserProject proj)
        {
            List<string> filePaths = new List<string>();
            foreach(StorageFile file in proj.StoryFiles)
            {
                filePaths.Add(file.Path);
            }
            string filesJson = JsonConvert.SerializeObject(filePaths.ToArray());

            TempJson temp = new TempJson(proj.Id, proj.Name, filesJson);

            return JsonConvert.SerializeObject(temp);
        }

        internal static Task<UserProject> DeserializeProjectAsync(string json)
        {
            return Task.Run(() => DeserializeProject(json));
        }

        internal static async Task<UserProject> DeserializeProject(string json)
        {
            TempJson temp = JsonConvert.DeserializeObject<TempJson>(json);
            string[] filePaths = JsonConvert.DeserializeObject<string[]>(temp.files);

            UserProject project = await RebuildProject(temp, filePaths);
            return project;


        }

        private static async Task<UserProject> RebuildProject(TempJson temp, string[] filePaths)
        {
            List<StorageFile> files = new List<StorageFile>();
            foreach (string path in filePaths)
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(path);
                files.Add(file);
            }

            UserProject proj = new UserProject(temp.id, temp.name, files);
            return proj;

        }



    }
}
