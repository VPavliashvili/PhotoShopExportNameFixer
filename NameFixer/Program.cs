using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameFixer
{
    static class Program
    {
        static void Main(string[] args)
        {

            const string suffix = ".png";

            Console.WriteLine("Enter destination of the folder");
            string folderPath = Console.ReadLine();

            PerformOperation(folderPath, suffix);
        }

        private static void PerformOperation(string folderPath, string suffix)
        {
            ValidatePath(ref folderPath);

            IEnumerable<string> allFiles;
            try
            {
                allFiles = GetFiles(folderPath, suffix);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Wrong folderPath passed to the program");
                return;
            }

            foreach (string file in allFiles)
            {
                FileInfo info = new FileInfo(file);
                string newName = GetFinalName(info.FullName, folderPath, suffix);
                info.Rename(newName);
            }
        }

        private static void Rename(this FileInfo file, string newName)
        {
            File.Move(file.FullName, newName);
        }

        private static IEnumerable<string> GetFiles(string folderPath, string fileExtension) =>
            Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).
                            Where(name => name.EndsWith(fileExtension));

        private static string GetFinalName(string fullname, string folderPath, string fileExtension)
        {

            string sampleName = fullname.Remove(0, folderPath.Length);
            sampleName = sampleName.Remove(sampleName.Length - fileExtension.Length);

            string[] parts = sampleName.Split('_');
            List<string> newParts = new List<string>();

            foreach (string part in parts)
            {
                if (part.IsWord())
                {
                    newParts.Add(part);
                }
            }

            string formattedName = string.Empty;

            for (int i = 0, length = newParts.Count; i < length; i++)
            {
                formattedName += i == 0 ? newParts[i] : $"_{newParts[i]}";
            }
            formattedName += fileExtension;

            return folderPath + formattedName;

        }

        private static bool IsWord(this string s)
        {
            int letCount = 0;
            foreach (char c in s)
            {
                if (char.IsLetter(c))
                    letCount++;
            }

            return letCount >= 2;
        }

        private static void ValidatePath(ref string path)
        {
            if (!path.EndsWith("\\"))
            {
                path += '\\';
            }
        }

    }
}
