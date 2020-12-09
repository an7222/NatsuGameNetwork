using System.IO;
using System.Text;

class Login_REQ_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 10;
	//MEMBER
	public string PID;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + Encoding.Default.GetByteCount(PID).get7BitEncodingLength() + Encoding.Default.GetByteCount(PID);
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
	public int PROTOCOL_ID = 11;
	//MEMBER
	public long USER_ID;
	public long ServerTimeUnix;
	public string SessionToken;
	public int ZONE_ID;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(long) + Encoding.Default.GetByteCount(SessionToken).get7BitEncodingLength() + Encoding.Default.GetByteCount(SessionToken) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		USER_ID = br.ReadInt64();
		ServerTimeUnix = br.ReadInt64();
		SessionToken = br.ReadString();
		ZONE_ID = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(USER_ID);
		bw.Write(ServerTimeUnix);
		bw.Write(SessionToken);
		bw.Write(ZONE_ID);
	}
}
class Login_FIN_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 12;
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
class RestAPI_REQ_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 13;
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
class RestAPI_RES_S2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 14;
	//MEMBER
	public string Info;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + Encoding.Default.GetByteCount(Info).get7BitEncodingLength() + Encoding.Default.GetByteCount(Info);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		Info = br.ReadString();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(Info);
	}
}
