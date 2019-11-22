using System;
using System.IO;
using System.Text;
using Structurizr.IO.Json;

namespace Structurizr
{

    public class WorkspaceUtils
    {

        /// <summary>
        /// Loads a workspace from a JSON definition saved as a file
        /// </summary>
        /// <param name="file">a FileInfo object representing the location of the JSON definition</param>
        /// <returns>a Workspace object</returns>
        public static Workspace LoadWorkspaceFromJson(FileInfo file)
        {
            if (file == null) {
                throw new ArgumentException("The path to a JSON file must be specified.");
            }

            if (!file.Exists) {
                throw new ArgumentException("The specified JSON file does not exist.");
            }

            string content = File.ReadAllText(file.FullName, Encoding.UTF8);

            return new JsonReader().Read(new StringReader(content));
        }

        /// <summary>
        /// Saves a workspace to a JSON definition as a file.
        /// </summary>
        /// <param name="file">a FileInfo object representing the location of the file to write the JSON definition</param>
        public static void SaveWorkspaceToJson(Workspace workspace, FileInfo file)
        {
            if (workspace == null) {
                throw new ArgumentException("A workspace must be provided.");
            }

            if (file == null) {
                throw new ArgumentException("The path to a JSON file must be specified.");
            }

            using (StreamWriter writer = new StreamWriter(new FileStream(file.FullName, FileMode.Create)))
            {
                new JsonWriter(true).Write(workspace, writer);
            }
        }

        /// <summary>
        /// Prints the given workspace as an indented JSON document.
        /// </summary>
        /// <param name="workspace">the Workspace object to be printed</param>
        public static void PrintWorkspaceAsJson(Workspace workspace)
        {
            JsonWriter jsonWriter = new JsonWriter(true);
            StringWriter stringWriter = new StringWriter();
            jsonWriter.Write(workspace, stringWriter);
            Console.WriteLine(stringWriter.ToString());
        }

    }
}
