using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp7;

namespace plcom
{
    public class plc
    {

        string testc;
        S7Client Client = new S7Client();

        public int readPlcDbwValue(string plcIp, int Rack, int Slot, int DbNum, int DbwNum)
        {
            byte[] Buffer = new byte[2];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, DbwNum, 2, Buffer);//读取DbwNum所对应的字的值
            Client.Disconnect();
            return S7.GetIntAt(Buffer, 0);
        }

        public void writePlcDbwValue(string plcIp, int Rack, int Slot, int DbNum, int DbwNum, int writeValue)
        {
            short a = (short)writeValue;//将整形的writeValue强制转换成short类型
            Client.ConnectTo(plcIp, Rack, Slot);
            byte[] buffer = new byte[2];
            S7.SetIntAt(buffer, 0, a);
            Client.DBWrite(DbNum, DbwNum, 2, buffer);//将DbwNum对应的字更新
            Client.Disconnect();
        }



        public float readPlcDbdValues(string plcIp, int Rack, int Slot, int DbNum, int DbdNum)
        {
            byte[] Buffer = new byte[4];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, DbdNum, 4, Buffer);//读取Dbd所对应的值
            Client.Disconnect();
            return S7.GetRealAt(Buffer, 0);
        }
        public void writePlcDbdValue(string plcIp, int Rack, int Slot, int DbNum, int DbdNum, float writeValue)
        {
            short a = (short)writeValue;//将writeValue强制转换成short类型
            Client.ConnectTo(plcIp, Rack, Slot);
            byte[] buffer = new byte[4];
            S7.SetDIntAt(buffer, 0, a);
            Client.DBWrite(DbNum, DbdNum, 4, buffer);//将Dbd更新
            //Client.Disconnect();

        }
        public bool getPlcDbxVaule(string plcIp, int Rack, int Slot, int DbNum, int dbx, int dbxx)
        {
            byte[] Buffer = new byte[1];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, dbx, 1, Buffer);//读取Dbx所对应的值      
            Client.Disconnect();
            return S7.GetBitAt(Buffer, 0, dbxx);
        }

        //20200915 添加读取字符串指令
        public string getCharValue(string ip, int dbnum, int start, int size)
        {
            byte[] Buffer = new byte[size];
            Client.ConnectTo(ip, 0, 3);
            Client.DBRead(dbnum, start, size, Buffer);
            Client.Disconnect();
            return S7.GetCharsAt(Buffer, 0, size);
        }


        //20200917 读取M位
        public bool getPlcMX(string plcIp, int pos, int bit)
        {
            byte[] Buffer = new byte[1];//M6.0 即读取MB6，一个字节的长度即可，所以定义Buffer的长度为1个字节
            Client.ConnectTo(plcIp, 0, 3);
            Client.MBRead(pos, 1, Buffer);
            Client.Disconnect();
            return S7.GetBitAt(Buffer, 0, bit);
        }



        //20201026 复制特定字节从一个PLC到另外一个PLC。 S7-400系列PLC默认机架号为0，插槽为3
        public void copyBytes(string ipone, int dbone, int dbonestart, int oneslot, string iptwo, int dbtwo, int dbtwostart, int twoslot, int bytelength)
        {
            byte[] Buffer = new byte[bytelength];
            Client.ConnectTo(ipone, 0, oneslot);
            Client.DBRead(dbone, dbonestart, bytelength, Buffer);
            Client.Disconnect();
            Client.ConnectTo(iptwo, 0, twoslot);
            Client.DBWrite(dbtwo, dbtwostart, bytelength, Buffer);
            Client.Disconnect();

        }

        //20201027 读取特定字节

        public byte[] readBytes(string ip, int rack, int slot, int dbnum, int start, int length)
        {
            byte[] Buffer = new byte[length];
            Client.ConnectTo(ip, rack, slot);
            Client.DBRead(dbnum, start, length, Buffer);
            Client.Disconnect();
            return Buffer;
        }


        //20201027写入特定字节
        public void writeBytes(string ip, int rack, int slot, int dbnum, int start, int length, byte[] buffer)
        {
            Client.ConnectTo(ip, rack, slot);
            Client.DBWrite(dbnum, start, length, buffer);
            Client.Disconnect();
        }



        //20201029 生成DB块字节数组
        public byte[] getDBbytes(string plcIp, int Rack, int Slot, int DbNum, int Start, int Length)
        {
            byte[] Buffer = new byte[Length];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, Start, Length, Buffer);//读取DbwNum所对应的字的值
            return Buffer;
        }

        //20201120 读取字符串

        public string getChars(string plcIp, int Rack, int Slot, int DbNum, int Start, int Length)
        {
            byte[] Buffer = new byte[Length];
            Client.ConnectTo(plcIp, Rack, Slot);
            Client.DBRead(DbNum, Start, Length, Buffer);//读取DbwNum所对应的字的值
            return S7.GetCharsAt(Buffer, 0, Length);

        }

        //20201120 写入字符串

        public void setChars(string plcIp, int Rack, int Slot, int DbNum, int Start, int Length, string Value)
        {
            byte[] Buffer = new byte[Length];
            Client.ConnectTo(plcIp, Rack, Slot);
            S7.SetCharsAt(Buffer, 0, Value);
            Client.DBWrite(DbNum, Start, Length, Buffer);//将DbwNum对应的字更新
            Client.Disconnect();
        }


        //20201127 写M位，默认1个字节，即1个MB
        public void setPlcMX(string plcIp, int pos, int bit, bool writevalue)
        {

            byte[] Buffer = new byte[1];
            Client.ConnectTo(plcIp, 0, 3);
            S7.SetBitAt(ref Buffer, 0, bit, writevalue); //向1个字节的第bit位写入bool值
            Client.MBWrite(pos, 1, Buffer);//将这个buffer放入pos开头长度为1个字节，如m5.6 ，即将buffer放入字节5开始的MB中
            Client.Disconnect();
        }


    }
}
