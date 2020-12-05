using System.IO;
using System.Text;

class Field_Excel : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 9;
	//MEMBER
	public int FIELD_ID;
	public string FieldName;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(int) + Encoding.Default.GetByteCount(FieldName).get7BitEncodingLength() + Encoding.Default.GetByteCount(FieldName);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		FIELD_ID = br.ReadInt32();
		FieldName = br.ReadString();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(FIELD_ID);
		bw.Write(FieldName);
	}
}
