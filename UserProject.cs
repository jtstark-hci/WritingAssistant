using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WritingAssistant
{
    class UserProject
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Comment> Comments { get; }
        public List<StorageFile> StoryFiles { get; }
        public List<Profile> Profiles { get; }

        //TO ADD:
        //notifications list
        //Robo work log

        public UserProject() { }

        public UserProject(string name)
        {
            Name = name;
        }

        public UserProject(string name, List<StorageFile> files)
        {
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
}
