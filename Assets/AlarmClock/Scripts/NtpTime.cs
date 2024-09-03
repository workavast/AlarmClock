using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public static class NtpTime
    {
        private const string NtpServer1 = "pool.ntp.org1";
        private const string NtpServer2 = "ntp3.ntp-servers.net";
        private const string NtpServer3 = "3.ru.pool.ntp.org";

        public static async Task<ClockTime> GetNetworkTime()
        {
            var dateTime = await GetNetworkDateTime();

            return new ClockTime()
            {
                Hours = dateTime.Hour,
                Minutes = dateTime.Minute,
                Seconds = dateTime.Second
            };
        }
        
        public static async Task<DateTime> GetNetworkDateTime()
        {
            var dateTime = DateTime.Now;
            
            var task = GetNetworkTimeFromNtp(NtpServer1);
            await task;
            if (task.Result.Item1)
                dateTime = task.Result.Item2;
            else
            {
                Debug.Log("err1");
                task = GetNetworkTimeFromNtp(NtpServer2);
                await task;
                if (task.Result.Item1)
                    dateTime = task.Result.Item2;
                else
                {
                    Debug.Log("err2");
                    task = GetNetworkTimeFromNtp(NtpServer3);
                    await task;
                    if (task.Result.Item1)
                        dateTime = task.Result.Item2;
                    else
                        Debug.LogError("Cant connect to the ntp server");
                }
            }
            
            Debug.Log(dateTime);
            return dateTime;
        }
        
        private static async Task<(bool, DateTime)> GetNetworkTimeFromNtp(string ntpServer)
        {
            try
            {
                var ntpData = new byte[48];
                ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

                var task = Dns.GetHostAddressesAsync(ntpServer);
                await task;
                if (!task.IsCompletedSuccessfully)
                    throw new Exception(task.Exception?.Message);

                var addresses = task.Result;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);

                using (var socket =  new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.SendTimeout = 100;
                    socket.ReceiveTimeout = 100;
                    
                    var connectTask = socket.ConnectAsync(ipEndPoint);
                    await connectTask;
                    if (!connectTask.IsCompletedSuccessfully)
                        throw new Exception(connectTask.Exception?.Message);

                    var sendTask = socket.SendAsync(new ArraySegment<byte>(ntpData), SocketFlags.None);
                    await sendTask;
                    if (!sendTask.IsCompletedSuccessfully)
                        throw new Exception(sendTask.Exception?.Message);
                    
                    var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(ntpData), SocketFlags.None);
                    await receiveTask;
                    if (!receiveTask.IsCompletedSuccessfully)
                        throw new Exception(receiveTask.Exception?.Message);
                }

                ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 |
                                (ulong)ntpData[43];
                ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 |
                                  (ulong)ntpData[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                return (true, networkDateTime);
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while try to take time from the npt server: {e}");
                return (false, new DateTime());
            }
        }
    }
}