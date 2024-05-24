using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Runtime;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SandboxRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("No arguments provided");
                Environment.Exit(-1);
                return;
            }


            if (!args[0].EndsWith(".exe") || !File.Exists(args[0]))
            {
                Console.Write("First parameter should be full path of the executable trigger.");
                Environment.Exit(-1);
                return;
            }

            RunInSandbox(args[0], args.Skip(1).ToArray());
        }

        static void RunInSandbox(string targetPath, string[] args)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(targetPath);

            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, setup.ApplicationBase));
            permissionSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
            permissionSet.AddPermission(new WebPermission(PermissionState.Unrestricted));
            permissionSet.AddPermission(new DnsPermission(PermissionState.Unrestricted));
            permissionSet.AddPermission(new SocketPermission(PermissionState.Unrestricted));
            permissionSet.AddPermission(new NetworkInformationPermission(PermissionState.Unrestricted));
            permissionSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));

            string configFile = $"{targetPath}.config";
            if (File.Exists(configFile))
            {
                setup.ConfigurationFile = configFile;
            }

            string applicationName = FileVersionInfo.GetVersionInfo(targetPath).ProductName;
            AppDomain sandbox = AppDomain.CreateDomain("ExecutableTriggerSandbox", new Evidence(), setup, permissionSet);
            sandbox.ExecuteAssembly(targetPath, args);
        }
    }
}
