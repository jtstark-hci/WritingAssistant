using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace WritingAssistant
{
    public class Profile
    {
        internal class NestedObjectAttribute
        {
            internal Dictionary<string, string> simpleAtt = new Dictionary<string, string>();
            internal Dictionary<string, string[]> listAtt = new Dictionary<string, string[]>();

            public NestedObjectAttribute() { }
        }


        internal Dictionary<string, string> simpleAttributes = new Dictionary<string, string>();
        internal Dictionary<string, string[]> listAttributes = new Dictionary<string, string[]>();
        internal Dictionary<string, NestedObjectAttribute> complexAttributes = new Dictionary<string, NestedObjectAttribute>();

        public Profile() {

            Debug.WriteLine("creating profile");
            string fileContents = File.ReadAllText(@"C:\Users\jessi\test_profile.json");
            DeserializeGptProfile(fileContents);
        }

        public Profile(string json) 
        {
            DeserializeGptProfile(json);
        }

        public Profile(string json, bool fromSave)
        {

        }

        public override string ToString()
        {
            string ret = "";

            foreach(string key in simpleAttributes.Keys)
            {
                ret += key + ": " + simpleAttributes[key] + "\n";
            }

            return ret;
        }

        internal void DeserializeGptProfile(string json)
        {
            Debug.WriteLine("Deserializing: " + json);
            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> actualDict = JsonConvert.DeserializeAnonymousType<Dictionary<string, dynamic>>(json, dict);

            foreach(string key in actualDict.Keys)
            {
                dynamic value = actualDict[key];
                if (value.GetType() == typeof(string))
                {
                    Debug.WriteLine("found string");
                    simpleAttributes[key] = value;
                } else
                {
                    //lists are objects so everything else goes together

                }

                //} else if (value is string[])
                //{
                //    Debug.WriteLine("found list");
                //    string[] strings = JsonConvert.DeserializeObject<string[]>(value);
                //    listAttributes[key] = strings;

                //} else if (value is object)
                //{
                //    Debug.WriteLine("found nested object");
                    //Dictionary<string, dynamic> nested = new Dictionary<string, dynamic>();
                    //Dictionary<string, dynamic> nestedDict = JsonConvert.DeserializeAnonymousType<Dictionary<string,dynamic>>(value, nested);
                    //NestedObjectAttribute nestedObjectAttribute = new NestedObjectAttribute();
                    //foreach(string nestedKey in nestedDict.Keys)
                    //{
                    //    dynamic nestedValue = nestedDict[nestedKey];
                    //    if (nestedValue is string)
                    //    {
                    //        nestedObjectAttribute.simpleAtt[nestedKey] = nestedValue;

                    //    }
                    //    else if (nestedValue is string[])
                    //    {
                    //        string[] strings = JsonConvert.DeserializeObject<string[]>(nestedValue);
                    //        nestedObjectAttribute.listAtt[nestedKey] = strings;
                    //    }
                    //}
                    
                    //complexAttributes[key] = nestedObjectAttribute;

                //}
            }
        }


    }
}
