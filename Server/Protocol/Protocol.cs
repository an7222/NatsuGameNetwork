using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    }

    public int GetPacketLength() {
        return PACKET_LENGTH;
    }

    public int GetProtocol_ID() {
        return PROTOCOL_ID;
    }


    public void Read(BinaryReader br) {
        PID = br.ReadString();
        LoginAt = br.ReadInt64();

        Console.WriteLine("[Login Packet] PID : {0}, LoginAt : {1}", PID, LoginAt);
    }

    public void Write(BinaryWriter bw) {
        SetPacketLength();
        bw.Write(PACKET_LENGTH);
        bw.Write(PROTOCOL_ID);
        bw.Write(PID);
        bw.Write(LoginAt);
    }
}
