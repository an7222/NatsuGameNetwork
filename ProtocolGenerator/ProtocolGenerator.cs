using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class ProtocolGenerator {
    public static void Generate() {
        string rootFolder = Directory.GetCurrentDirectory();
        rootFolder = rootFolder.Substring(0,
                    rootFolder.IndexOf(@"\AntoriGameNetwork\", StringComparison.Ordinal) + @"\AntoriGameNetwork\".Length);


        string readFilePath = Path.Combine(rootFolder, "ProtocolGenerator");
        readFilePath = Path.Combine(rootFolder, "IDL");
        Console.WriteLine(readFilePath);

        string writeFilePath = Path.Combine(rootFolder, "Server");
        writeFilePath = Path.Combine(rootFolder, "Protocol");
        Console.WriteLine(writeFilePath);

        string[] readFilePaths = Directory.GetFiles(readFilePath, "*.idl",
                                         SearchOption.TopDirectoryOnly);

        for (int i = 0; i < readFilePaths.Length; ++i) {
            using (var readFile = new FileStream(readFilePaths[i], FileMode.Open)) {
                string[] temp = readFilePaths[i].Split("\\");
                if (temp.Length == 0)
                    continue;

                string fileName = temp[temp.Length - 1];

                //using (var writeFile = new FileStream(writeFilePaths[i], FileMode.OpenOrCreate))
                using (var sr = new StreamReader(readFile, Encoding.UTF8)) {
                    while (false == sr.EndOfStream) {
                        processReadLine(sr.ReadLine());
                    }
                }
            }

        }
    }

    static void processReadLine(string one_line) {
        var temp = one_line.Split(" ");
        for(int i = 0; i < temp.Length; ++i) {
            switch (temp[i]) {
                case "PACKET":
                    i++;
                    writeClass(temp[i]);
                    break;
            }
        }

    }

    static void writeClass(string class_name) {

    }
}

