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
                        var member_type_field_list = new List<KeyValuePair<string, string>>();
                        while (false == sr.EndOfStream) {
                            processReadLine(sr.ReadLine(), sw, member_type_field_list);
                        }
                    }
                }
            }

        }
    }

    static void processReadLine(string one_line, StreamWriter sw, List<KeyValuePair<string, string>> member_type_field_list) {
        var keyword = one_line.Split(TextConst.SPACE);
        for(int i = 0; i < keyword.Length; ++i) {
            switch (keyword[i].Trim()) {
                case "PACKET":
                    sw.Write("class");
                    sw.Write(TextConst.SPACE);

                    i++;
                    sw.Write(keyword[i].Trim()); //Class Name
                    sw.WriteLine(TextConst.IMPLEMENT_I_PROTOCOL);

                    sw.WriteLine("\t//COMMON");
                    sw.WriteLine(TextConst.PACKET_LENGTH_DEFINE);
                    sw.WriteLine(TextConst.PROTOCOL_ID_DEFINE);
                    sw.WriteLine("\t//MEMBER");
                    break;
                case "END":
                    sw.WriteLine(TextConst.SET_PACKET_LENGTH_DEFINE);


                    #region SET_PACKET_LENGTH
                    sw.Write(TextConst.SET_PACKET_LENGTH_INNER_START);

                    sw.Write(TextConst.SPACE);
                    sw.Write("sizeof(int)"); //PACKET_LENGTH
                    sw.Write(TextConst.SPACE);
                    sw.Write(TextConst.PLUS);
                    sw.Write(TextConst.SPACE);
                    sw.Write("sizeof(int)"); //PROTOCOL_ID
                    sw.Write(TextConst.SPACE);
                    sw.Write(TextConst.PLUS);

                    int count = 0;
                    foreach(var kv in member_type_field_list) {
                        sw.Write(TextConst.SPACE);
                        if(kv.Key == "string") {
                            sw.Write("1 + Encoding.Default.GetByteCount(" + kv.Value + ")");
                        } else {
                            sw.Write("sizeof(" + kv.Key + ")");
                        }

                        count++;
                        if (count < member_type_field_list.Count) {
                            sw.Write(TextConst.SPACE);
                            sw.Write(TextConst.PLUS);
                        }
                        //TODO : User Definite Type
                    }
                    sw.WriteLine(TextConst.SEMI_COLON);

                    #endregion


                    sw.WriteLine(TextConst.GET_PACKET_LENGTH);

                    sw.WriteLine(TextConst.GET_PROTOCOL_ID);


                    #region READ

                    #endregion

                    #region WRITE


                    #endregion


                    sw.WriteLine(TextConst.BRACE_END);
                    break;
                case "int":
                case "long":
                case "string":
                    sw.Write(TextConst.TAB);
                    sw.Write(TextConst.PUBLIC);

                    sw.Write(TextConst.SPACE);
                    sw.Write(keyword[i].Trim());
                    sw.Write(TextConst.SPACE);
                    
                    i++;
                    sw.Write(keyword[i].Trim()); //Member Name
                    sw.WriteLine(TextConst.SEMI_COLON);

                    member_type_field_list.Add(new KeyValuePair<string, string>(keyword[i-1].Trim(), keyword[i].Trim()));
                    break;
            }
        }

    }
}

