using System.Net.NetworkInformation;
using KitX.Shared.CSharp.Device;

namespace Kscript.CSharp.Utils
{
    /// <summary>
    /// 提供网络相关的工具方法
    /// </summary>
    public static class NetworkUtils
    {
        /// <summary>
        /// 检查给定的设备是否是本地设备
        /// </summary>
        /// <param name="device">要检查的设备信息</param>
        /// <returns>如果是本地设备返回true，否则返回false</returns>
        public static bool IsLocalDevice(DeviceInfo device)
        {
            if (device.Device.IPv4 == null)
                return false;

            // 获取本机所有IP地址
            var localIPs = GetLocalIPAddresses();
            
            // 检查设备IP是否与本机IP匹配
            return localIPs.Contains(device.Device.IPv4) ||
                   IsLocalNetworkAddress(device.Device.IPv4);
        }

        /// <summary>
        /// 获取本机所有网络接口的IP地址
        /// </summary>
        private static HashSet<string> GetLocalIPAddresses()
        {
            return new HashSet<string>(
                NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                    .SelectMany(n => n.GetIPProperties().UnicastAddresses)
                    .Select(a => a.Address.ToString())
            );
        }

        /// <summary>
        /// 检查给定的IP地址是否为本地网络地址
        /// </summary>
        private static bool IsLocalNetworkAddress(string ip)
        {
            return ip.StartsWith("127.") ||      // Loopback
                   ip.StartsWith("169.254.") ||  // Link-local
                   ip.StartsWith("192.168.") ||  // 私有网络
                   ip.StartsWith("10.") ||       // 私有网络
                   ip.StartsWith("172.") ||      // 私有网络
                   ip.Equals("::1");             // IPv6 loopback
        }

        /// <summary>
        /// 检查给定的IP地址是否为私有网络地址
        /// </summary>
        public static bool IsPrivateNetworkAddress(string ip)
        {
            return ip.StartsWith("192.168.") ||  // Class C private network
                   ip.StartsWith("10.") ||       // Class A private network
                   (ip.StartsWith("172.") &&     // Class B private network
                    TryGetSecondOctet(ip, out var secondOctet) &&
                    secondOctet >= 16 && secondOctet <= 31);
        }

        private static bool TryGetSecondOctet(string ip, out int octet)
        {
            octet = 0;
            var parts = ip.Split('.');
            if (parts.Length < 2) return false;
            return int.TryParse(parts[1], out octet);
        }

        /// <summary>
        /// 判断两个IP地址是否在同一子网内
        /// </summary>
        public static bool IsInSameSubnet(string ip1, string ip2, string subnetMask)
        {
            var ip1Parts = ip1.Split('.').Select(byte.Parse).ToArray();
            var ip2Parts = ip2.Split('.').Select(byte.Parse).ToArray();
            var maskParts = subnetMask.Split('.').Select(byte.Parse).ToArray();

            for (int i = 0; i < 4; i++)
            {
                if ((ip1Parts[i] & maskParts[i]) != (ip2Parts[i] & maskParts[i]))
                    return false;
            }

            return true;
        }
    }
}