using System.IO;
using System.Text;

class Login_REQ_C2S : IProtocol {
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
class Login_RES_S2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 2;
	//MEMBER
	public long UserID;
	public long ServerTimeUnix;
	public string SessionToken;
	public string BattleServerIp;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(long) + 1 + Encoding.Default.GetByteCount(SessionToken) + 1 + Encoding.Default.GetByteCount(BattleServerIp);
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
		BattleServerIp = br.ReadString();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(UserID);
		bw.Write(ServerTimeUnix);
		bw.Write(SessionToken);
		bw.Write(BattleServerIp);
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
class NewBattleUser_REQ_C2B : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 4;
	//MEMBER
	public long UserID;
	public int Level;
	public int HP;
	public int TODOUserInfo;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(int) + sizeof(int) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		UserID = br.ReadInt64();
		Level = br.ReadInt32();
		HP = br.ReadInt32();
		TODOUserInfo = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(UserID);
		bw.Write(Level);
		bw.Write(HP);
		bw.Write(TODOUserInfo);
	}
}
class NewBattleUser_RES_C2B : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 5;
	//MEMBER
	public long ObjectIDList;
	public int TODOStatusList;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		ObjectIDList = br.ReadInt64();
		TODOStatusList = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(ObjectIDList);
		bw.Write(TODOStatusList);
	}
}
class MoveStart_C2B : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 6;
	//MEMBER
	public int Direction;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		Direction = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(Direction);
	}
}
class MoveEnd_C2B : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 7;
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
class MoveStart_B2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 8;
	//MEMBER
	public long ObjectID;
	public int Direction;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		ObjectID = br.ReadInt64();
		Direction = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(ObjectID);
		bw.Write(Direction);
	}
}
class MoveEnd_B2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 9;
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
class ChangePos_B2C : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 10;
	//MEMBER
	public long ObjectID;
	public int Pos_x;
	public int Pos_y;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + sizeof(long) + sizeof(int) + sizeof(int);
	}
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
	public void Read(BinaryReader br) {
		ObjectID = br.ReadInt64();
		Pos_x = br.ReadInt32();
		Pos_y = br.ReadInt32();
	}
	public void Write(BinaryWriter bw) {
		SetPacketLength();
		bw.Write(PACKET_LENGTH);
		bw.Write(PROTOCOL_ID);
		bw.Write(ObjectID);
		bw.Write(Pos_x);
		bw.Write(Pos_y);
	}
}
