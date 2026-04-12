using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Net;

namespace SystemInfoCollector
{
    class Program
    {
        class Config
        {
            public string ServerIP;
            public int CpuLimit;
            public int RamLimit;
            public int DiskLimit;
            //public int NetworkLimit;
        }

        static void Main(string[] args)
        {

           // Console.WriteLine("Agent started...");

            // 1. скан при запуске
            string report = CollectInfo();
            //Console.WriteLine(report);

            SendToServer(report);

            // 2. запуск HTTP сервера
            StartHttpServer();

            /*Config cfg = LoadConfig();

            StringBuilder report = new StringBuilder();

            report.AppendLine("===== SYSTEM INFORMATION =====");
            report.AppendLine("Time: " + DateTime.Now);
            report.AppendLine();

            var hw = GetHardwareInfo();

            report.AppendLine("HARDWARE:");
            report.AppendLine("CPU: " + hw["CPU"]);
            report.AppendLine("RAM: " + hw["RAM_GB"] + " GB");

            report.AppendLine("\nDISKS:");
            foreach (string disk in (List<string>)hw["Disks"])
                report.AppendLine(disk);

            float cpuLoad = GetCpuLoad();
            float ramLoad = GetRamUsage();
            var diskUsage = GetDiskUsage();
            double netUsage = GetNetworkUsagePercent();

            report.AppendLine("\nCPU Load: " + cpuLoad + " %");
            report.AppendLine("RAM Usage: " + ramLoad + " %");
            report.AppendLine("Network Usage: " + netUsage + " %");

            report.AppendLine("\nDISK USAGE:");
            foreach (var d in diskUsage)
                report.AppendLine(d);

            report.AppendLine("\nINSTALLED PROGRAMS:");
            var programs = GetInstalledPrograms();

            foreach (string p in programs)
                report.AppendLine(p);

            report.AppendLine("\nTotal programs: " + programs.Count);

            string output = report.ToString();

            Console.WriteLine(output);

            CheckLimits(cfg, cpuLoad, ramLoad, diskUsage, netUsage);

            string filePath = "system_report.txt";
            File.WriteAllText(filePath, output);

            Console.WriteLine("\nReport saved to: " + Path.GetFullPath(filePath));
            */


            //Console.WriteLine("\nPress any key...");
            //Console.ReadKey();
        }
        static string CollectInfo()
        {
            Config cfg = LoadConfig();

            StringBuilder report = new StringBuilder();

            report.AppendLine("SYSTEM INFORMATION");
            report.AppendLine("Time: " + DateTime.Now);
            report.AppendLine();

            var hw = GetHardwareInfo();

            report.AppendLine("HARDWARE:");
            report.AppendLine("CPU: " + hw["CPU"]);
            report.AppendLine("RAM: " + hw["RAM_GB"] + " GB");

            report.AppendLine("\nDISKS:");
            foreach (string disk in (List<string>)hw["Disks"])
                report.AppendLine(disk);

            float cpuLoad = GetCpuLoad();
            float ramLoad = GetRamUsage();
            var diskUsage = GetDiskUsage();
          //  double netUsage = GetNetworkUsagePercent();

            report.AppendLine("\nCPU Load: " + cpuLoad + " %");
            report.AppendLine("RAM Usage: " + ramLoad + " %");
           // report.AppendLine("Network Usage: " + netUsage + " %");

            report.AppendLine("\nDISK USAGE:");
            foreach (var d in diskUsage)
                report.AppendLine(d);
            // после DISK USAGE

            report.AppendLine("\nINSTALLED PROGRAMS:");
            var programs = GetInstalledPrograms();

            foreach (string p in programs)
                report.AppendLine(p);

            report.AppendLine("\nTotal programs: " + programs.Count);
            //CheckLimits(cfg, cpuLoad, ramLoad, diskUsage, netUsage);

            var warnings = CheckLimits(cfg, cpuLoad, ramLoad, diskUsage);

          //  report.AppendLine("\n===== WARNINGS =====");

            if (warnings.Count == 0)
            {
                report.AppendLine("No warnings");
            }
            else
            {
                foreach (var w in warnings)
                    report.AppendLine(w);
            }

            return report.ToString();
        }
        static void StartHttpServer()
        {
            HttpListener listener = new HttpListener();
           
            listener.Prefixes.Add("http://+:5050/");
            listener.Start();

           // Console.WriteLine("Agent listening on port 5050...");

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                if (request.Url.AbsolutePath == "/getinfo")
                {
                    string report = CollectInfo();

                    byte[] buffer = Encoding.UTF8.GetBytes(report);
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }

                else if (request.Url.AbsolutePath == "/scan")
                {
                   // Console.WriteLine("Scan requested by server");

                    string report = CollectInfo();

                    SendToServer(report);

                    byte[] buffer = Encoding.UTF8.GetBytes("SCAN OK");
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }

                response.Close();
            }
        }
        static void SendToServer(string data)
        {
            try
            {
                string serverIp = LoadConfig().ServerIP;
                string url = "http://" + serverIp + ":5050/agentdata/";

                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.UploadString(url, "POST", data);
                }

               // Console.WriteLine("Data sent to server");
            }
            catch (Exception ex)
            {
               // Console.WriteLine("Send error: " + ex.Message);
            }
        }
        private static Config LoadConfig()
        {
            Config cfg = new Config();

            if (!File.Exists("config.txt"))
            {
               // Console.WriteLine("config.txt not found");
                return cfg;
            }

            foreach (var line in File.ReadAllLines("config.txt"))
            {
                if (line.StartsWith("Server IP"))
                    cfg.ServerIP = line.Split(':')[1].Trim();

                if (line.StartsWith("CPU usage"))
                    cfg.CpuLimit = int.Parse(line.Split(':')[1].Trim());

                if (line.StartsWith("RAM usage"))
                    cfg.RamLimit = int.Parse(line.Split(':')[1].Trim());

                if (line.StartsWith("Disks usage"))
                    cfg.DiskLimit = int.Parse(line.Split(':')[1].Trim());

              //  if (line.StartsWith("Network adapter usage"))
                 //   cfg.NetworkLimit = int.Parse(line.Split(':')[1].Trim());
            }

            return cfg;
        }

        /*private static void CheckLimits(Config cfg, float cpu, float ram, List<string> diskUsage, double net)
        {
            Console.WriteLine("\n===== SYSTEM STATUS =====");

            if (cpu > cfg.CpuLimit)
                Console.WriteLine($"WARNING: CPU usage exceeded! ({cpu}% > {cfg.CpuLimit}%)");
            else
                Console.WriteLine("CPU usage normal");

            if (ram > cfg.RamLimit)
                Console.WriteLine($"WARNING: RAM usage exceeded! ({ram}% > {cfg.RamLimit}%)");
            else
                Console.WriteLine("RAM usage normal");

            if (net > cfg.NetworkLimit)
                Console.WriteLine($"WARNING: Network usage exceeded! ({net}% > {cfg.NetworkLimit}%)");
            else
                Console.WriteLine("Network usage normal");

            foreach (var disk in diskUsage)
            {
                int start = disk.IndexOf("(") + 1;
                int end = disk.IndexOf("%");

                if (start > 0 && end > start)
                {
                    double percent = double.Parse(disk.Substring(start, end - start));

                    if (percent > cfg.DiskLimit)
                        Console.WriteLine("WARNING: Disk usage exceeded! " + disk);
                }
            }
        } */

        private static List<string> CheckLimits(Config cfg, float cpu, float ram, List<string> diskUsage)
        {
            List<string> warnings = new List<string>();

            if (cpu > cfg.CpuLimit)
                warnings.Add($"WARNING: CPU usage exceeded! ({cpu}% > {cfg.CpuLimit}%)");

            if (ram > cfg.RamLimit)
                warnings.Add($"WARNING: RAM usage exceeded! ({ram}% > {cfg.RamLimit}%)");

           // if (net > cfg.NetworkLimit)
           //     warnings.Add($"WARNING: Network usage exceeded! ({net}% > {cfg.NetworkLimit}%)");

            foreach (var disk in diskUsage)
            {
                int start = disk.IndexOf("(") + 1;
                int end = disk.IndexOf("%");

                if (start > 0 && end > start)
                {
                    double percent = double.Parse(disk.Substring(start, end - start));

                    if (percent > cfg.DiskLimit)
                        warnings.Add("WARNING: Disk usage exceeded! " + disk);
                }
            }

            return warnings;
        }

        private static Dictionary<string, object> GetHardwareInfo()
        {
            Dictionary<string, object> hw = new Dictionary<string, object>();

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT Name, NumberOfCores FROM Win32_Processor"))
            {
                foreach (ManagementObject mo in searcher.Get())
                    hw["CPU"] = mo["Name"] + " (" + mo["NumberOfCores"] + " cores)";
            }

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    ulong ram = (ulong)mo["TotalPhysicalMemory"];
                    hw["RAM_GB"] = Math.Round(ram / (1024.0 * 1024 * 1024), 2);
                }
            }

            List<string> disks = new List<string>();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (!d.IsReady) continue;

                long size = d.TotalSize / (1024 * 1024 * 1024);
                disks.Add($"{d.Name} {size} GB");
            }

            hw["Disks"] = disks;

            return hw;
        }

        private static List<string> GetInstalledPrograms()
        {
            List<string> list = new List<string>();

            string[] roots =
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string root in roots)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(root))
                {
                    if (key == null) continue;

                    foreach (string sub in key.GetSubKeyNames())
                    {
                        using (RegistryKey sk = key.OpenSubKey(sub))
                        {
                            if (sk == null) continue;

                            string name = sk.GetValue("DisplayName") as string;

                            if (!string.IsNullOrEmpty(name) && !list.Contains(name))
                                list.Add(name);
                        }
                    }
                }
            }

            list.Sort();
            return list;
        }

        private static float GetCpuLoad()
        {
            using (PerformanceCounter cpuCounter =
                   new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                Thread.Sleep(1000);
                return (float)Math.Round(cpuCounter.NextValue(), 2);
            }
        }

        private static float GetRamUsage()
        {
            using (PerformanceCounter ramCounter =
                   new PerformanceCounter("Memory", "% Committed Bytes In Use"))
            {
                return (float)Math.Round(ramCounter.NextValue(), 2);
            }
        }

        private static List<string> GetDiskUsage()
        {
            List<string> disks = new List<string>();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (!d.IsReady) continue;

                long total = d.TotalSize / (1024 * 1024 * 1024);
                long free = d.TotalFreeSpace / (1024 * 1024 * 1024);
                long used = total - free;

                double percent = Math.Round((double)used / total * 100, 2);

                disks.Add($"{d.Name} Used: {used}GB / {total}GB ({percent}%)");
            }

            return disks;
        }

        private static double GetNetworkUsagePercent()
        {
            string instanceName = GetNetworkInterfaceName();

            using (PerformanceCounter sent =
                new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceName))
            using (PerformanceCounter received =
                new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceName))
            {
                sent.NextValue();
                received.NextValue();

                Thread.Sleep(1000);

                float totalBytes = sent.NextValue() + received.NextValue();

                double currentMbps = (totalBytes * 8) / (1024 * 1024);
                double maxMbps = GetMaxNetworkSpeedMbps();

                if (maxMbps == 0) return 0;

                return Math.Round((currentMbps / maxMbps) * 100, 2);
            }
        }

        private static double GetMaxNetworkSpeedMbps()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    return ni.Speed / 1_000_000.0;
                }
            }
            return 0;
        }

        private static string GetNetworkInterfaceName()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();

            foreach (string name in instances)
            {
                if (!name.ToLower().Contains("loopback") &&
                    !name.ToLower().Contains("isatap"))
                {
                    return name;
                }
            }

            return instances.Length > 0 ? instances[0] : "";
        }
    }
}