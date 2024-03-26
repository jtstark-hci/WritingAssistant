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
using Microsoft.UI.Composition;

namespace WritingAssistant
{
    class UserProject
    {

        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public List<Comment> Comments { get; } = null;
        public List<string> StoryFiles { get; set; } = new List<string>();
        public List<Profile> Profiles { get; } = null;

        public bool isNew { get; set; } = true;

        //TO ADD:
        //notifications list
        //Robo work log

        public UserProject() { }

        public UserProject(string name)
        {
            Name = name;
            Debug.WriteLine("creating project");
        }

        public UserProject(string name, List<string> files)
        {
            Name = name;
            StoryFiles = files;
            Debug.WriteLine("creating project");
        }

        internal UserProject(int id, string name, List<string> files)
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
            public bool isSaved;

            public TempJson(int i, string n, string f, bool b)
            {
                id = i;
                files = f;
                name = n;
                isSaved = b;
                
            }
        }

        internal static string SerializeProject(UserProject proj)
        { 
            string filesJson = JsonConvert.SerializeObject(proj.StoryFiles.ToArray());

            TempJson temp = new TempJson(proj.Id, proj.Name, filesJson, proj.isNew);

            return JsonConvert.SerializeObject(temp);
        }

        internal static UserProject DeserializeProject(string json)
        {
            TempJson temp = JsonConvert.DeserializeObject<TempJson>(json);
            string[] files = JsonConvert.DeserializeObject<string[]>(temp.files);
            UserProject proj = new UserProject(temp.id, temp.name, files.ToList());
            proj.isNew = false;

            return proj;
        }



    }
}
