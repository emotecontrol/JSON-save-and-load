using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

// This save/load module uses the Newtonsoft Json library (http://james.newtonking.com/projects/json-net.aspx)
// to serialize and deserialize the SavingsAccount objects into Json text, and back into objects again.

// The purpose of this class is to give beginners to C# an easy way to save and load objects.  It can save and 
// load any List<T> of objects that have properties that can be get and set.  It will ignore saving properties 
// that aren't gettable, and ignore restoring properties that aren't settable.

// To use, add the class to your project.  Then make a SaveAndLoad object, passing in as a parameter the name of
// the file you would like to save to.  
// e.g. SaveAndLoad save = new SaveAndLoad("myfile.txt");  
// Then use the Save<T>() and Load<T>() methods to save and load your data.  So if I wanted to save a List<T> of 
// Customer objects called customerList using the SaveAndLoad object from the example, I would write 
// save.Save<Customer>(customerList);
// Likewise, to load that list into an initialized List<Customer> called newList, I would write 
// save.Load<Customer>(newList);
// As you can see, the generic type T in the save and load methods is replaced by the type of the object in your
// list of objects.  You *must* pass in a list of the correct type of objects, and you can only save one object
// type at a time.

// If you're not familiar with JSON, you should check out the output of the save method.  JSON is cool because it's
// human-readable, and the output file can be easily edited using a text editor.  Check out my other repo:
// https://github.com/emotecontrol/ConsoleBankingApp, to see how I first used this class when I wrote it for a
// school assignment.


namespace SampleNameSpace
{
    class SaveAndLoad
    {
        string filename;
        public SaveAndLoad(string file)
        {
            filename = file;
        }

        // the follewing I/O section is based on some msdn tutorials I found for reading/writing 
        // data, particularly: http://msdn.microsoft.com/en-us/library/system.io.filestream.aspx

        
        public void Save<T>(List<T> listToSave){
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            string output = JsonConvert.SerializeObject(listToSave);
            using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
            {
                AddText(fs, output);
            }
        }
        
        public List<T> Load<T>()
        {
            string openAccountListJson = null;
            List<T> newlist;
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {                
                byte[] b = new byte[1024]; // make a byte array to use as a buffer
                UTF8Encoding temp = new UTF8Encoding(true); // make a UTF8 Encoding object
                while (fs.Read(b, 0, b.Length) > 0) // while the buffer created by the FileStream is bigger than 0
                                                    // i.e. there are still characters in the stream...
                {
                    openAccountListJson += temp.GetString(b); // add the byte array to the Json string in UTF8 encoding
                }
            }
            newlist = JsonConvert.DeserializeObject<List<T>>(openAccountListJson); // Deserialize the Json string to a List<SavingsAccount> object
            return newlist;
        }
        
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
