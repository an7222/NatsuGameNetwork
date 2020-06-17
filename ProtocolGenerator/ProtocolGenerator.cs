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
        readFilePath = Path.Combine(readFilePath, "IDL");

        string writeFilePath = Path.Combine(rootFolder, "Server");
        writeFilePath = Path.Combine(writeFilePath, "Protocol");

        string[] readFilePaths = Directory.GetFiles(readFilePath, "*.idl");

        for (int i = 0; i < readFilePaths.Length; ++i) {
            using (var readFile = new FileStream(readFilePaths[i], FileMode.Open)) {
                string[] temp = readFilePaths[i].Split("\\");
                if (temp.Length == 0)
                    continue;

                string fileNameWithExt = temp[temp.Length - 1];
                string fileName = fileNameWithExt.Split(".")[0] + "_TEST"; //TODO : remove _TEST

                using (var writeFile = new FileStream(Path.Combine(writeFilePath, string.Concat(fileName, ".cs")), FileMode.OpenOrCreate))
                using (var sw = new StreamWriter(writeFile)) {
                    sw.WriteLine(TextConst.SYSTEM_IO);
                    sw.WriteLine(TextConst.SYSTEM_TEXT);
                    sw.WriteLine();
                    using (var sr = new StreamReader(readFile)) {
                        while (false == sr.EndOfStream) {
                            processReadLine(sr.ReadLine(), sw);
                        }
                    }
                }
            }

        }
    }

    static void processReadLine(string one_line, StreamWriter sw) {
        var keyword = one_line.Split(TextConst.SPACE);
        for(int i = 0; i < keyword.Length; ++i) {
            switch (keyword[i].Trim()) {
                case "PACKET":
                    sw.Write("class");
                    sw.Write(TextConst.SPACE);

                    i++;
                    sw.Write(keyword[i]); //Class Name
                    sw.Write(TextConst.SPACE);
                    sw.Write(TextConst.COLON);
                    sw.Write(TextConst.SPACE);

                    sw.Write(TextConst.I_PROTOCOL);
                    sw.Write(TextConst.SPACE);
                    sw.WriteLine(TextConst.BRACE_START);
                    break;
                case "END":
                    sw.WriteLine(TextConst.BRACE_END);
                    break;
                case "int":
                case "long":
                case "string":
                    sw.Write(TextConst.TAB);
                    sw.Write(TextConst.PUBLIC);

                    sw.Write(TextConst.SPACE);
                    sw.Write(keyword[i]);
                    sw.Write(TextConst.SPACE);

                    i++;
                    sw.Write(keyword[i]); //Member Name
                    sw.WriteLine(TextConst.SEMI_COLON);
                    break;
            }
        }

    }
}

