using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class ProtocolGenerator {
    public static void Generate() {
        var rootFolder = Directory.GetCurrentDirectory();
        rootFolder = rootFolder.Substring(0,
                    rootFolder.IndexOf(@"\ProtocolGenerator\", StringComparison.Ordinal) + @"\ProtocolGenerator\".Length);

        rootFolder = Path.Combine(rootFolder, "IDL");
        rootFolder = Path.Combine(rootFolder, "ClientSession");
        Console.WriteLine(rootFolder);


        //var PathToData = Path.GetFullPath(Path.Combine(rootFolder, "IDL\\ClientSession\\"));

        //var Parser = Parser();
        //var d = new FileStream(Path.Combine(PathToData, $"{dataFileName}.txt"), FileMode.Open);
        //var fs = new StreamReader(d, Encoding.UTF8);
    }
}

