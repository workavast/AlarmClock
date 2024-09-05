using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public static class NtpTime
    {
        private const string NtpServer1 = "pool.ntp.org1";//invalid ntp address for demonstration of
                                                          //full work of this class (valid address is pool.ntp.org)
        private const string NtpServer2 = "ntp3.ntp-servers.net";
        private const string NtpServer3 = "3.ru.pool.ntp.org";

        private const int SendTimeOut = 200;
        private const int ReceiveTimeOut = 200;
        
        public static async Task<ClockTime> GetNetworkClockTime()
        {
            var dateTime = await GetLocalNetworkDateTime();
            return new ClockTime(dateTime);
        }

        public static async Task<DateTime> GetLocalNetworkDateTime()
        {
            var dateTime = await GetNetworkDateTime();
            return dateTime.ToLocalTime();
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
                task = GetNetworkTimeFromNtp(NtpServer2);
                await task;
                if (task.Result.Item1)
                    dateTime = task.Result.Item2;
                else
                {
                    task = GetNetworkTimeFromNtp(NtpServer3);
                    await task;
                    if (task.Result.Item1)
                        dateTime = task.Result.Item2;
                    else
                        Debug.LogError("Cant connect to the any ntp server");
                }
            }
            
            return dateTime;
        }

        /// <returns>
        ///     Item1 - request is success <br/>
        ///     Item2 - DateTime from the NTP server
        /// </returns>
        private static async Task<(bool, DateTime)> GetNetworkTimeFromNtp(string ntpServer)
        {
            try
            {
                var ntpData = new byte[48];
                ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

                var getHostAddressesTask = Dns.GetHostAddressesAsync(ntpServer);
                await getHostAddressesTask;
                if (!getHostAddressesTask.IsCompletedSuccessfully)
                    throw new Exception($"Some problem with getHostAddressesTask: {getHostAddressesTask.Exception?.Message}");

                var addresses = getHostAddressesTask.Result;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.SendTimeout = SendTimeOut;

                    var connectTask = socket.ConnectAsync(ipEndPoint);
                    await connectTask;
                    if (!connectTask.IsCompletedSuccessfully)
                        throw new Exception($"Some problem with connectTask: {connectTask.Exception?.Message}");

                    var sendTask = socket.SendAsync(new ArraySegment<byte>(ntpData), SocketFlags.None);
                    await sendTask;
                    if (!sendTask.IsCompletedSuccessfully)
                        throw new Exception($"Some problem with sendTask: {sendTask.Exception?.Message}");

                    var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(ntpData), SocketFlags.None);
                    await Task.WhenAny(receiveTask, Task.Delay(ReceiveTimeOut)); //socket.ReceiveTimeout doesnt work with socket.ReceiveAsync(...)
                    if (!receiveTask.IsCompletedSuccessfully)
                        throw new Exception($"Some problem with receiveTask: {receiveTask.Exception?.Message}");
                }

                var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 |
                              (ulong)ntpData[43];
                var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 |
                                (ulong)ntpData[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .AddMilliseconds((long)milliseconds);

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