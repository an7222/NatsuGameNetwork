using System.IO;
using System.Text;

class Login_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 1;
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
class Login_ACK_S2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 2;
	//MEMBER
	public long UserID;
	public long ServerTimeUnix;
	public string SessionToken;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(long) + 1 + Encoding.Default.GetByteCount(SessionToken);
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
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(UserID);
		bw.Write(ServerTimeUnix);
		bw.Write(SessionToken);
	}
}
class Login_FIN_C2S : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 3;
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
class Test : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 4;
	//MEMBER
	public string abcd;
	public int ddd1;
	public long lll2;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + 1 + Encoding.Default.GetByteCount(abcd) + sizeof(int) + sizeof(long);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		abcd = br.ReadString();
		ddd1 = br.ReadInt32();
		lll2 = br.ReadInt64();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(abcd);
		bw.Write(ddd1);
		bw.Write(lll2);
	}
}
