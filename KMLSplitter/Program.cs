using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMLSplitter
{
    class Program
    {
        private static int PLACEMARKS_IN_A_FILE = 500;
        private static string HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
                                        "<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n" +
                                        "<Document><Folder><name>export</name>\n";
        private static string FOOTER = "</Folder>" +
"<Schema name=\"export\" id=\"export\">" +
    "<SimpleField name=\"PPID\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"EFFDT\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"FEATURECD\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"TWP\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"RGE\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"MER\" type=\"string\"></SimpleField>" +
    "<SimpleField name=\"TRM\" type=\"string\"></SimpleField>" +
"</Schema>" +
"</Document></kml>";

        static void Main(string[] args)
        {
            if (args.Length < 1)
                Console.Error.WriteLine("Usage: KMLSplitter source.kml [destination]");
            string sourceFile = args[0];
            string destination = args.Length > 1 ? Path.IsPathRooted(args[1]) ? args[1] : Path.Combine(Environment.CurrentDirectory, args[1]) : Environment.CurrentDirectory;

            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine("Source file not found.");
                return;
            }

            StringBuilder builder = new StringBuilder();
            bool initialRead = true;
            using (var stream = File.OpenRead(sourceFile))
            using (var bufferedStream = new BufferedStream(stream))
            using (var streamReader = new StreamReader(bufferedStream))
            {
                int polygons = 0;
                string s;
                while ((s = streamReader.ReadLine()) != null)
                {
                    if (!initialRead)
                        builder.Append(s + "\n");
                    if (s.Contains("</Placemark>"))
                    {
                        if (polygons >= PLACEMARKS_IN_A_FILE)
                        {
                            WriteOutFile(builder, destination);
                            builder.Clear();
                            polygons = 0;
                        }
                    }
                    else if (s.Contains("<Placemark>"))
                    {
                        initialRead = false;
                        polygons++;
                    }

                }
            }
            Console.ReadLine();
        }

        static int fileCounter = 0;
        private static void WriteOutFile(StringBuilder builder, string destination)
        {
            fileCounter++;
            Directory.CreateDirectory(destination);
            File.WriteAllText(Path.Combine(destination, String.Format("output{0}.kml", fileCounter)), HEADER + builder + FOOTER);
        }
    }
}
