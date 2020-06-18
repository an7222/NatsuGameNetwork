using System.IO;
using System.Text;

class Login : IProtocol {
	//COMMON
	public int PACKET_LENGTH = 0;
	public int PROTOCOL_ID = 1;
	//MEMBER
	public string PID;
	public long LoginAt;
	public void SetPacketLength() {
		PACKET_LENGTH = sizeof(int) + sizeof(int) + 1 + Encoding.Default.GetByteCount(PID) + sizeof(long);
	public int GetPacketLength() {
		return PACKET_LENGTH;
	}
	public int GetProtocol_ID() {
		return PROTOCOL_ID;
	}
}
