
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
    public const string EQUAL = "=";

    //Write Element
    public const string SYSTEM_IO = "using System.IO;";
    public const string SYSTEM_TEXT = "using System.Text;";

    public const string IMPLEMENT_I_PROTOCOL = SPACE + COLON + SPACE + "IProtocol" + SPACE + BRACE_START;

    public const string PACKET_LENGTH_DEFINE = TAB + PUBLIC + SPACE + "int PACKET_LENGTH = 0;";
    public const string PROTOCOL_ID_DEFINE = TAB + PUBLIC + SPACE + "int PROTOCOL_ID";

    public const string SET_PACKET_LENGTH_DEFINE = TAB + PUBLIC + SPACE + "void SetPacketLength()" + SPACE + BRACE_START;
    public const string SET_PACKET_LENGTH_INNER_START = DOUBLE_TAB + "PACKET_LENGTH =";
    public const string GET_PACKET_LENGTH = TAB + "public int GetPacketLength() {" + NEW_LINE + DOUBLE_TAB + "return PACKET_LENGTH;" + NEW_LINE + TAB + BRACE_END;
    public const string GET_PROTOCOL_ID = TAB + "public int GetProtocol_ID() {" + NEW_LINE + DOUBLE_TAB + "return PROTOCOL_ID;" + NEW_LINE + TAB + BRACE_END;
    
    public const string BINARY_READ_DEFINE = TAB + PUBLIC + SPACE + "void Read(BinaryReader br)" + SPACE + BRACE_START;
    public const string BINARY_READER_INSTANCE_CALL = "br.";
    public const string BINRAY_READER_READ_STRING = "ReadString()";
    public const string BINRAY_READER_READ_INT = "ReadInt32()";
    public const string BINRAY_READER_READ_LONG = "ReadInt64()";
    public const string BINRAY_READER_READ_FLOAT = "ReadSingle()";

    public const string BINARY_WRITE_DEFINE = TAB + PUBLIC + SPACE + "void Write(BinaryWriter bw)" + SPACE + BRACE_START;
    public const string BINARY_WRITER_INSTANCE_CALL = DOUBLE_TAB + "bw.";

    public const string BINARY_WRITER_7BIT_ENCODING_PREFIX = "CommonUtil.get7BitEncodingLength(";
    public const string BINARY_WRITER_ENCODING_GET_BYTE_COUNT = "Encoding.Default.GetByteCount(";
}
