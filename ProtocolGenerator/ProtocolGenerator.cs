using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class ProtocolGenerator {
    static int protocol_id = 1;
    public static void Generate() {
        string rootFolder = Directory.GetCurrentDirectory();
        rootFolder = rootFolder.Substring(0,
                    rootFolder.IndexOf(@"\AntoriGameNetwork\", StringComparison.Ordinal) + @"\AntoriGameNetwork\".Length);


        string readFilePath = Path.Combine(rootFolder, "ProtocolGenerator");
        readFilePath = Path.Combine(readFilePath, "IDL");

        string writeFilePath_Server = Path.Combine(rootFolder, "CSharpServer");
        writeFilePath_Server = Path.Combine(writeFilePath_Server, "Network");

        string writeFilePath_Client = Path.Combine(rootFolder, "Client");
        writeFilePath_Client = Path.Combine(writeFilePath_Client, "Protocol");

        string[] readFilePaths = Directory.GetFiles(readFilePath, "*.idl");

        for (int i = 0; i < readFilePaths.Length; ++i) {
            using (var readFile = new FileStream(readFilePaths[i], FileMode.Open)) {
                string[] temp = readFilePaths[i].Split("\\");
                if (temp.Length == 0)
                    continue;

                string fileNameWithExt = temp[temp.Length - 1];
                string fileName = fileNameWithExt.Split(".")[0];

                using (var writeFile_Server = new FileStream(Path.Combine(writeFilePath_Server, string.Concat(fileName, ".cs")), FileMode.Create))
                using (var writeFile_Client = new FileStream(Path.Combine(writeFilePath_Client, string.Concat(fileName, ".cs")), FileMode.Create))
                using (var sw_Server = new StreamWriter(writeFile_Server))
                using (var sw_Client = new StreamWriter(writeFile_Client)) {
                    sw_Server.WriteLine(TextConst.SYSTEM_IO);
                    sw_Server.WriteLine(TextConst.SYSTEM_TEXT);
                    sw_Server.WriteLine();

                    sw_Client.WriteLine(TextConst.SYSTEM_IO);
                    sw_Client.WriteLine(TextConst.SYSTEM_TEXT);
                    sw_Client.WriteLine();

                    using (var sr = new StreamReader(readFile)) {
                        var member_type_field_list_server = new List<KeyValuePair<string, string>>();
                        var member_type_field_list_client = new List<KeyValuePair<string, string>>();
                        while (false == sr.EndOfStream) {
                            var oneLine = sr.ReadLine();
                            processReadLine(oneLine, sw_Server, member_type_field_list_server);

                            processReadLine(oneLine, sw_Client, member_type_field_list_client);

                            var keyword = oneLine.Split(TextConst.SPACE);
                            foreach (var a in keyword) {
                                if (a.Trim() == "END")
                                    protocol_id++;
                            }
                        }
                    }
                }
            }

        }
    }

    static void processReadLine(string one_line, StreamWriter sw, List<KeyValuePair<string, string>> member_type_field_list) {
        var keyword = one_line.Split(TextConst.SPACE);
        for (int i = 0; i < keyword.Length; ++i) {
            switch (keyword[i].Trim()) {
                case "PACKET":
                    sw.Write("class");
                    sw.Write(TextConst.SPACE);

                    i++;
                    sw.Write(keyword[i].Trim()); //Class Name
                    sw.WriteLine(TextConst.IMPLEMENT_I_PROTOCOL);

                    sw.WriteLine("\t//COMMON");
                    sw.WriteLine(TextConst.PACKET_LENGTH_DEFINE);
                    sw.Write(TextConst.PROTOCOL_ID_DEFINE);
                    sw.Write(TextConst.SPACE + TextConst.EQUAL + TextConst.SPACE);
                    sw.Write(protocol_id);
                    sw.WriteLine(TextConst.SEMI_COLON);
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

                    if(member_type_field_list.Count > 0) {
                        sw.Write(TextConst.SPACE);
                        sw.Write(TextConst.PLUS);
                    }

                    int count = 0;
                    foreach (var kv in member_type_field_list) {
                        sw.Write(TextConst.SPACE);
                        if (kv.Key == "string") {
                            string encodeSyntax = TextConst.BINARY_WRITER_ENCODING_GET_BYTE_COUNT + kv.Value + ")";
                            sw.Write(TextConst.BINARY_WRITER_7BIT_ENCODING_PREFIX + encodeSyntax + ")");

                            sw.Write(TextConst.SPACE);
                            sw.Write(TextConst.PLUS);
                            sw.Write(TextConst.SPACE);

                            sw.Write(encodeSyntax);
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

                    sw.WriteLine(TextConst.TAB + TextConst.BRACE_END);

                    #endregion


                    sw.WriteLine(TextConst.GET_PACKET_LENGTH);

                    sw.WriteLine(TextConst.GET_PROTOCOL_ID);


                    #region READ

                    sw.WriteLine(TextConst.BINARY_READ_DEFINE);
                    foreach (var kv in member_type_field_list) {
                        sw.Write(TextConst.DOUBLE_TAB);
                        sw.Write(kv.Value);
                        sw.Write(TextConst.SPACE);
                        sw.Write(TextConst.EQUAL);
                        sw.Write(TextConst.SPACE);
                        sw.Write(TextConst.BINARY_READER_INSTANCE_CALL);
                        
                        switch (kv.Key) {
                            case "string":
                                sw.Write(TextConst.BINRAY_READER_READ_STRING);
                                break;
                            case "int":
                                sw.Write(TextConst.BINRAY_READER_READ_INT);
                                break;
                            case "long":
                                sw.Write(TextConst.BINRAY_READER_READ_LONG);
                                break;
                            case "float":
                                sw.Write(TextConst.BINRAY_READER_READ_FLOAT);
                                break;
                        }

                        sw.WriteLine(TextConst.SEMI_COLON);
                    }

                    sw.WriteLine(TextConst.TAB + TextConst.BRACE_END);

                    #endregion

                    #region WRITE

                    sw.WriteLine(TextConst.BINARY_WRITE_DEFINE);
                    sw.WriteLine(TextConst.DOUBLE_TAB + "SetPacketLength();");

                    sw.Write(TextConst.BINARY_WRITER_INSTANCE_CALL);//PACKET LENGTH
                    sw.Write("Write(PACKET_LENGTH)");
                    sw.WriteLine(TextConst.SEMI_COLON);
                    sw.Write(TextConst.BINARY_WRITER_INSTANCE_CALL); //PROTOCOL_ID
                    sw.Write("Write(PROTOCOL_ID)");
                    sw.WriteLine(TextConst.SEMI_COLON);


                    foreach (var kv in member_type_field_list) {
                        sw.Write(TextConst.BINARY_WRITER_INSTANCE_CALL);
                        sw.Write("Write(" + kv.Value + ")");
                        sw.WriteLine(TextConst.SEMI_COLON);
                    }

                    sw.WriteLine(TextConst.TAB + TextConst.BRACE_END);

                    #endregion


                    sw.WriteLine(TextConst.BRACE_END);

                    member_type_field_list.Clear();
                    break;
                case "int":
                case "long":
                case "string":
                case "float":
                    sw.Write(TextConst.TAB);
                    sw.Write(TextConst.PUBLIC);

                    sw.Write(TextConst.SPACE);
                    sw.Write(keyword[i].Trim());
                    sw.Write(TextConst.SPACE);

                    i++;
                    sw.Write(keyword[i].Trim()); //Member Name
                    sw.WriteLine(TextConst.SEMI_COLON);

                    member_type_field_list.Add(new KeyValuePair<string, string>(keyword[i - 1].Trim(), keyword[i].Trim()));
                    break;
            }
        }

    }
}

