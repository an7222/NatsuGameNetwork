
class TextConst {
    //Atomic Element
    public const string PUBLIC = "public";
    public const string TAB = "\t";
    public const string DOUBLE_TAB = "\t\t";
    public const string NEW_LINE = "\n";
    public const string SPACE = " ";
    public const string COLON = ":";
    public const string SEMI_COLON = ";";
    public const string PLUS = "+";
    public const string BRACE_START = "{";
    public const string BRACE_END = "}";

    //Write Element
    public const string SYSTEM_IO = "using System.IO;";
    public const string SYSTEM_TEXT = "using System.Text;";
    public const string IMPLEMENT_I_PROTOCOL = SPACE + COLON + SPACE + "IProtocol" + SPACE + BRACE_START;
    public const string PACKET_LENGTH_DEFINE = TAB + PUBLIC + SPACE + "int PACKET_LENGTH = 0;";
    public const string PROTOCOL_ID_DEFINE = TAB + PUBLIC + SPACE + "int PROTOCOL_ID = 1;";
    public const string SET_PACKET_LENGTH_DEFINE = TAB + PUBLIC + SPACE + "void SetPacketLength()" + SPACE + BRACE_START;
    public const string SET_PACKET_LENGTH_INNER_START = DOUBLE_TAB + "PACKET_LENGTH =";
    public const string GET_PACKET_LENGTH = TAB + "public int GetPacketLength() {" + NEW_LINE + DOUBLE_TAB + "return PACKET_LENGTH;" + NEW_LINE + TAB + BRACE_END;
    public const string GET_PROTOCOL_ID = TAB + "public int GetProtocol_ID() {" + NEW_LINE + DOUBLE_TAB + "return PROTOCOL_ID;" + NEW_LINE + TAB + BRACE_END;
    public const string BINARY_READ = PUBLIC + SPACE + "void Read(BinaryReader br)" + SPACE + BRACE_START;
}
