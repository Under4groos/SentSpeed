using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SentSpeed.lib
{
    public class Network
    {
        public NetworkInterface[] NetworkInterfaces;
        NetworkInterface NetworkInterface;
        IPv4InterfaceStatistics interfaceStats;

        long BytesSent = 0;
        long BytesReceived = 0;

        int id = 0;
        public int BytesSentSpeed
        {
            get;set;
        }
        public int BytesReceivedSpeed
        {
            get; set;
        }
        public int IDInterfaces
        {
            set
            {
                id = value;
                int count = NetworkInterfaces.Length;

                NetworkInterface = NetworkInterfaces[id >= count ? count : id <= 0 ? 0 : id];
                
            }
        }


        public Network()
        {
            NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            
        }

        public string ConvertTo(long l , int mode)
        {
            if (mode == 0)
            {
                return $"{l / 1024} KB";
            }
            else if (mode == 1)
            {
                return $"{l / 1024 / 1024} MB";
            }
            else if (mode == 2)
            {
                return $"{l / 1024 / 1024 / 1024} GB";
            }else if (mode == 3)
            {
                return $"{l / 1024 / 1024 / 1024 / 1024} TB";
            }
            else
            {
                
                return $"{l} Byte\\s";
            }
        }

        public void Update()
        {
            if (NetworkInterface == null)
                return;
            interfaceStats = NetworkInterface.GetIPv4Statistics();

            BytesSentSpeed = (int)(interfaceStats.BytesSent - BytesSent) / 1024;
            BytesReceivedSpeed = (int)(interfaceStats.BytesReceived - BytesReceived) / 1024;

            BytesReceived = interfaceStats.BytesReceived;
            BytesSent = interfaceStats.BytesSent;
        }
        public string GetUploadSpeed()
        {
            return $" {BytesSentSpeed} kb/s";
        }
        public string GetbytesSentAndReceived()
        {
            return $"Sent: {BytesSent} Received: {BytesReceived}";
        }
        public (long,long) GetSentAndReceivedbytes()
        {
            return (BytesSent,BytesReceived);
        }

        public string GetDownloadSpeed()
        {
            return $" {BytesReceivedSpeed} kb/s";
        }


        public string GetInterfaceType()
        {
            return NetworkInterface.NetworkInterfaceType.ToString()??"NULL";
        }

    }
}
