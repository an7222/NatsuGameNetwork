using System.IO;
using System.Text;

class Login_REQ_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 9;
	//MEMBER
	public string PID;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + 1 + Encoding.Default.GetByteCount(PID);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		PID = br.ReadString();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(PID);
	}
}
class Login_RES_S2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 10;
	//MEMBER
	public long UserID;
	public long ServerTimeUnix;
	public string SessionToken;
	public int FieldId;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(long) + 1 + Encoding.Default.GetByteCount(SessionToken) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		UserID = br.ReadInt64();
		ServerTimeUnix = br.ReadInt64();
		SessionToken = br.ReadString();
		FieldId = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(UserID);
		bw.Write(ServerTimeUnix);
		bw.Write(SessionToken);
		bw.Write(FieldId);
	}
}
class Login_FIN_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 11;
	//MEMBER
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
	}
}
