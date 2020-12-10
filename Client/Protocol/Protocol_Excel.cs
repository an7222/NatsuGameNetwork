using System.IO;
using System.Text;

class Zone_Excel : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 9;
	//MEMBER
	public int ZONE_ID;
	public string ZONE_NAME;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(int) + Encoding.Default.GetByteCount(ZONE_NAME).get7BitEncodingLength() + Encoding.Default.GetByteCount(ZONE_NAME);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		ZONE_ID = br.ReadInt32();
		ZONE_NAME = br.ReadString();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(ZONE_ID);
		bw.Write(ZONE_NAME);
	}
}
