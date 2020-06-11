using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Protocol {
    class Login {
        public int Protocol_id = 1;
        public string PID;
        public long LoginAt;

        //public int GetProtocolLength() {
        //    return sizeof(int) + Encoding.Default.GetByteCount(PID) + sizeof(long);
        //}

        public void Read(Stream input) {
            using (BinaryReader br = new BinaryReader(input)) {
                PID = br.ReadString();
                LoginAt = br.ReadInt64();
            }
        }

        public void Write(Stream output) {
            using (BinaryWriter bw = new BinaryWriter(output)) {
                bw.Write(PID);
                bw.Write(LoginAt);
            }
        }
    }
}
