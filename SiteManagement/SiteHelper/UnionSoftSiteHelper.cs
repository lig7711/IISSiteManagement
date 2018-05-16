﻿using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace SiteManagement.SiteHelper
{
    public static class UnionSoftSiteHelper
    {
        static string IisIP
        {
            get {
                string name = Dns.GetHostName();
                IPAddress[] iPAddresses = Dns.GetHostAddresses(name);
                foreach (var item in iPAddresses)
                {
                    if (item.IsIPv6LinkLocal)
                        continue;
                    else
                        return "192.168.1.202";
                }
                return "";
            }
        }
        static UnionSoftSiteHelper()
        {
            try
            {
                using (var manager = new ServerManager())
                {
                    try
                    {
                        var testForIis = manager.Sites.FirstOrDefault();
                        //var testForIisExpress = manager.WorkerProcesses.FirstOrDefault();
                    }
                    catch (COMException)
                    {
                        throw new Exception("Looks like IIS is not installed.");
                    }
                    catch (NotImplementedException)
                    {
                        throw new Exception("Servant doesn't support IIS Express.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int TotalSites
        {
            get
            {
                using (var manager = new ServerManager())
                {
                    return manager.Sites.Count;
                }
            }
        }
        public static int TotalApplicationPools
        {
            get
            {
                using (var manager = new ServerManager())
                {
                    return manager.ApplicationPools.Count;
                }
            }
        }

        public static IEnumerable<IisSite> GetSites(bool excludeAppPools = false)
        {
            using (var manager = new ServerManager())
            {
                foreach (var site in manager.Sites)
                {
                    if (site.Bindings.Select(x => x.Protocol).Any(x => x == "ftp")) // Servant doesn't support FTP sites
                        continue;

                    var parsedSite = ParseSite(site, excludeAppPools, manager.ApplicationPools.ToList());
                    if (parsedSite != null)
                        yield return parsedSite;
                }
            }
        }

        public static Microsoft.Web.Administration.Site GetIisSiteById(int iisId)
        {
            using (var manager = new ServerManager())
            {
                return manager.Sites.SingleOrDefault(x => x.Id == iisId);
            }
        }

        public static IisSite GetSiteById(int iisId)
        {
            using (var manager = new ServerManager())
            {
                var iisSite = manager.Sites.SingleOrDefault(x => x.Id == iisId);

                return iisSite == null ? null : ParseSite(iisSite);
            }
        }

        private static IisSite ParseSite(Microsoft.Web.Administration.Site site, bool excludeAppPools = false, List<Microsoft.Web.Administration.ApplicationPool> applicationPools = null)
        {
            if (site == null)
                return null;

            var servantSite = new IisSite
            {
                IisId = (int)site.Id,
                Name = site.Name,
                ApplicationPool = site.Applications[0].ApplicationPoolName,
                SitePath = site.Applications[0].VirtualDirectories[0].PhysicalPath,
                SiteState = site.State,
                LogFileDirectory = site.LogFile.Directory,
                Bindings = GetBindings(site).ToList(),
                Applications = new List<UnionSoftSiteApplication>()
            };

            if (!excludeAppPools)
            {
                if (applicationPools == null)
                {
                    using (var manager = new ServerManager())
                    {
                        applicationPools = manager.ApplicationPools.ToList();
                    }
                }

                ObjectState applicationPoolState = applicationPools.Single(x => x.Name == site.Applications[0].ApplicationPoolName).State;
                //servantSite.ApplicationPoolState = (InstanceState)Enum.Parse(typeof(InstanceState), applicationPoolState.ToString());
            }

            foreach (var directory in site.Applications[0].VirtualDirectories.Skip(1))
            {
                servantSite.Applications.Add(new UnionSoftSiteApplication
                {
                    ApplicationPool = "",
                    Path = directory.Path,
                    DiskPath = directory.PhysicalPath,
                    IsApplication = false
                });
            }

            if (site.Applications.Count > 1)
            {
                foreach (var application in site.Applications.Skip(1))
                {
                    servantSite.Applications.Add(new UnionSoftSiteApplication
                    {
                        ApplicationPool = application.ApplicationPoolName,
                        Path = application.Path,
                        DiskPath = application.VirtualDirectories[0].PhysicalPath,
                        IsApplication = true
                    });
                }
            }

            return servantSite;
        }

        //private static Microsoft.Web.Administration.ApplicationPool UpdateIisApplicationPoolFromServant(this Microsoft.Web.Administration.ApplicationPool iisApplicationPool, ApplicationPool servantApplicationPool)
        //{
        //    ManagedPipelineMode pipelineMode;
        //    Enum.TryParse(servantApplicationPool.PipelineMode, true, out pipelineMode);
        //    LoadBalancerCapabilities loadBalancerCapabilities;
        //    Enum.TryParse(servantApplicationPool.ServiceUnavailableResponseType, true, out loadBalancerCapabilities);

        //    iisApplicationPool.Name = servantApplicationPool.Name;
        //    iisApplicationPool.AutoStart = servantApplicationPool.AutoStart;
        //    iisApplicationPool.ManagedRuntimeVersion = servantApplicationPool.ClrVersion;
        //    iisApplicationPool.ManagedPipelineMode = pipelineMode;
        //    iisApplicationPool.Recycling.DisallowOverlappingRotation = servantApplicationPool.DisallowOverlappingRotation;
        //    iisApplicationPool.Recycling.DisallowRotationOnConfigChange = servantApplicationPool.DisallowRotationOnConfigChange;
        //    iisApplicationPool.Recycling.PeriodicRestart.Time = servantApplicationPool.RecycleInterval;
        //    iisApplicationPool.Recycling.PeriodicRestart.PrivateMemory = servantApplicationPool.RecyclePrivateMemoryLimit;
        //    iisApplicationPool.Recycling.PeriodicRestart.Memory = servantApplicationPool.RecycleVirtualMemoryLimit;
        //    iisApplicationPool.Recycling.PeriodicRestart.Requests = servantApplicationPool.RecycleRequestsLimit;
        //    iisApplicationPool.ProcessModel.IdleTimeout = servantApplicationPool.IdleTimeout;
        //    iisApplicationPool.ProcessModel.MaxProcesses = servantApplicationPool.MaximumWorkerProcesses;
        //    iisApplicationPool.ProcessModel.PingingEnabled = servantApplicationPool.PingingEnabled;
        //    iisApplicationPool.ProcessModel.PingResponseTime = servantApplicationPool.PingMaximumResponseTime;
        //    iisApplicationPool.Failure.LoadBalancerCapabilities = loadBalancerCapabilities;
        //    iisApplicationPool.Failure.RapidFailProtection = servantApplicationPool.RapidFailProtectionEnabled;
        //    iisApplicationPool.Failure.RapidFailProtectionInterval = servantApplicationPool.RapidFailProtectionInterval;
        //    iisApplicationPool.Failure.RapidFailProtectionMaxCrashes = servantApplicationPool.RapidFailProtectionMaxCrashes;
        //    // Husk at opdatere GetDefaultApplicationPool

        //    return iisApplicationPool;
        //}

        private static IEnumerable<UnionSoftBinding> GetBindings(Microsoft.Web.Administration.Site site)
        {
            var allowedProtocols = new[] { "http", "https" };
            var certificates = GetCertificates();

            foreach (var binding in site.Bindings.Where(x => allowedProtocols.Contains(x.Protocol)))
            {
                var unionSoftBinding = new UnionSoftBinding();

                if (binding.Protocol == "https")
                {
                    if (binding.CertificateHash == null)
                        continue;

                    var certificate = certificates.SingleOrDefault(cert => cert.Hash.SequenceEqual(binding.CertificateHash));
                    if (certificate != null)
                    {
                        unionSoftBinding.CertificateName = certificate.Name;
                        unionSoftBinding.CertificateThumbprint = certificate.Thumbprint;
                    }
                    else
                        continue;
                }
                unionSoftBinding.Protocol = binding.Protocol;
                unionSoftBinding.Hostname = binding.Host;
                unionSoftBinding.Port = binding.EndPoint.Port;
                var endPointAddress = binding.EndPoint.Address.ToString();
                unionSoftBinding.IpAddress = endPointAddress == "*"|| endPointAddress=="0.0.0.0" ?  IisIP : endPointAddress;
                if (!unionSoftBinding.Hostname.Equals(string.Empty))
                {
                    unionSoftBinding.ClickLink = unionSoftBinding.Protocol + "://" + unionSoftBinding.Hostname + ":" + unionSoftBinding.Port;
                }
                else
                {
                    unionSoftBinding.ClickLink = unionSoftBinding.Protocol + "://" + unionSoftBinding.IpAddress + ":" + unionSoftBinding.Port;
                }
                yield return unionSoftBinding;
            }
        }

        public static IEnumerable<Certificate> GetCertificates()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly);
            var certs = store.Certificates.Cast<X509Certificate2>().ToList();

            foreach (var cert in certs)
            {
                var name = cert.FriendlyName;
                if (string.IsNullOrWhiteSpace(name)) // Extracts common name if friendly name isn't available.
                {
                    var commonName = cert.Subject.Split(',').SingleOrDefault(x => x.StartsWith("CN"));
                    if (commonName != null)
                    {
                        var locationOfEquals = commonName.IndexOf('=');
                        name = commonName.Substring(locationOfEquals + 1, commonName.Length - (locationOfEquals + 1));
                    }
                }

                yield return new Certificate { Name = name, Hash = cert.GetCertHash(), Thumbprint = cert.Thumbprint };
            }
        }

        public static string GetSitename(Site site)
        {
            if (site == null)
                return "Unknown";

            return site.Name;
        }

        //public static ManageSiteResult UpdateSite(IisSite site)
        //{
        //    var result = new ManageSiteResult { IisSiteId = site.IisId };

        //    using (var manager = new ServerManager())
        //    {
        //        var iisSite = manager.Sites.SingleOrDefault(x => x.Id == site.IisId);

        //        if (iisSite == null)
        //        {
        //            result.Result = SiteResult.UnknownSiteId;
        //            return result;
        //        }

        //        var iisSiteWithSameName = manager.Sites.SingleOrDefault(x => x.Id != site.IisId && x.Name == site.Name);

        //        if (iisSiteWithSameName != null)
        //        {
        //            result.Result = SiteResult.NameAlreadyInUse;
        //            return result;
        //        }

        //        var mainApplication = iisSite.Applications.First();
        //        var rootPathDirectory = mainApplication.VirtualDirectories.SingleOrDefault(x => x.Path == "/");
        //        if (rootPathDirectory == null)
        //        {
        //            mainApplication.VirtualDirectories.Add("/", site.SitePath);

        //        }
        //        else
        //        {
        //            rootPathDirectory.PhysicalPath = site.SitePath;
        //        }

        //        // In some scenarios Microsoft.Web.Administation fails to save site if property-set is detected with same name. 
        //        //I believe it deletes and insert sites on updates and this makes a name conflict. Fixed by the hack below:
        //        if (site.Name != iisSite.Name)
        //            iisSite.Name = site.Name;

        //        // If the application pool does not exists on the server, create it
        //        if (manager.ApplicationPools.SingleOrDefault(x => x.Name == site.ApplicationPool) == null)
        //        {
        //            manager.ApplicationPools.Add(site.ApplicationPool);
        //        }

        //        mainApplication.ApplicationPoolName = site.ApplicationPool;

        //        // Update log file path
        //        iisSite.LogFile.Directory = site.LogFileDirectory;

        //        // Commits bindings
        //        iisSite.Bindings.Clear();
        //        foreach (var binding in site.Bindings)
        //        {
        //            if (binding.Protocol == Protocol.https)
        //            {
        //                var certificate = GetCertificates().Single(x => x.Thumbprint == binding.CertificateThumbprint);
        //                iisSite.Bindings.Add(binding.ToIisBindingInformation(), certificate.Hash, "My");
        //            }
        //            else
        //                iisSite.Bindings.Add(binding.ToIisBindingInformation(), binding.Protocol.ToString());
        //        }

        //        // Deletes virtual applications
        //        var applicationsToDelete = iisSite.Applications.Skip(1).Where(application => !site.Applications.Where(x => x.IsApplication).Select(a => a.Path).Contains(application.Path)).ToList();
        //        foreach (var application in applicationsToDelete)
        //        {
        //            application.Delete();
        //            iisSite.Applications.Remove(application); // Bug in Microsoft.Web.Administration when changing from directory - application
        //        }

        //        // Deletes virtual directories
        //        var directoriesToDelete = mainApplication.VirtualDirectories.Where(directory => directory.Path != "/" && !site.Applications.Where(x => !x.IsApplication).Select(a => a.Path).Contains(directory.Path)).ToList(); // Exclude "/" because it's the root application's directory.
        //        foreach (var directory in directoriesToDelete)
        //        {
        //            directory.Delete();
        //            mainApplication.VirtualDirectories.Remove(directory); // Bug in Microsoft.Web.Administration when changing from directory - application
        //        }

        //        //Intelligently updates virtual applications + directories
        //        foreach (var application in site.Applications)
        //        {
        //            if (!application.Path.StartsWith("/"))
        //                application.Path = "/" + application.Path;

        //            if (application.IsApplication)
        //            {
        //                if (application.Path.EndsWith("/"))
        //                {
        //                    application.Path = application.Path.Substring(0, application.Path.Length - 1);
        //                }

        //                var iisApp = iisSite.Applications.SingleOrDefault(x => x.Path == application.Path);

        //                if (iisApp == null)
        //                {
        //                    iisSite.Applications.Add(application.Path, application.DiskPath);
        //                    iisApp = iisSite.Applications.Single(x => x.Path == application.Path);
        //                }

        //                iisApp.VirtualDirectories[0].PhysicalPath = application.DiskPath;
        //                iisApp.ApplicationPoolName = application.ApplicationPool;
        //            }
        //            else // Directory
        //            {
        //                var virtualDirectory = mainApplication.VirtualDirectories.SingleOrDefault(x => x.Path == application.Path);
        //                if (virtualDirectory == null)
        //                {
        //                    mainApplication.VirtualDirectories.Add(application.Path, application.DiskPath);
        //                }
        //                else
        //                {
        //                    virtualDirectory.PhysicalPath = application.DiskPath;
        //                }
        //            }
        //        }


        //        manager.CommitChanges();
        //    }

        //    return result;
        //}

        public static List<ApplicationPool> GetApplicationPools()
        {
            using (var manager = new ServerManager())
            {
                return manager.ApplicationPools.OrderBy(x => x.Name).ToList();
                //return manager.ApplicationPools.Select(x => new ApplicationPool
                //{
                //    Name = x.Name,
                //    State = (InstanceState)Enum.Parse(typeof(InstanceState), x.State.ToString()),
                //    ClrVersion = x.ManagedRuntimeVersion,
                //    PipelineMode = x.ManagedPipelineMode.ToString().ToLower(),
                //    AutoStart = x.AutoStart,
                //    DisallowOverlappingRotation = x.Recycling.DisallowOverlappingRotation,
                //    DisallowRotationOnConfigChange = x.Recycling.DisallowRotationOnConfigChange,
                //    RecycleInterval = x.Recycling.PeriodicRestart.Time,
                //    RecyclePrivateMemoryLimit = x.Recycling.PeriodicRestart.PrivateMemory,
                //    RecycleVirtualMemoryLimit = x.Recycling.PeriodicRestart.Memory,
                //    RecycleRequestsLimit = x.Recycling.PeriodicRestart.Requests,
                //    IdleTimeout = x.ProcessModel.IdleTimeout,
                //    MaximumWorkerProcesses = x.ProcessModel.MaxProcesses,
                //    PingingEnabled = x.ProcessModel.PingingEnabled,
                //    PingInterval = x.ProcessModel.PingInterval,
                //    PingMaximumResponseTime = x.ProcessModel.PingResponseTime,
                //    ServiceUnavailableResponseType = x.Failure.LoadBalancerCapabilities.ToString().ToLower(),
                //    RapidFailProtectionEnabled = x.Failure.RapidFailProtection,
                //    RapidFailProtectionInterval = x.Failure.RapidFailProtectionInterval,
                //    RapidFailProtectionMaxCrashes = x.Failure.RapidFailProtectionMaxCrashes,
                //}).OrderBy(x => x.Name).ToList();
            }
        }

        public static SiteStartResult StartSite(IisSite site)
        {
            using (var manager = new ServerManager())
            {
                var iisSite = manager.Sites.SingleOrDefault(x => x.Id == site.IisId);
                if (iisSite == null)
                    throw new SiteNotFoundException("Site " + site.Name + " was not found on IIS");

                try
                {
                    iisSite.Start();
                    return SiteStartResult.Started;
                }
                catch (ServerManagerException)
                {
                    return SiteStartResult.BindingIsAlreadyInUse;
                }
                catch (FileLoadException e)
                {
                    if (e.Message.Contains("being used by another"))
                    {
                        return SiteStartResult.PortInUseByAnotherService;
                    }

                    return SiteStartResult.CannotAccessSitePath;
                }
            }
        }

        public static void StopSite(IisSite site)
        {
            using (var manager = new ServerManager())
            {
                var iisSite = manager.Sites.SingleOrDefault(x => x.Id == site.IisId);
                if (iisSite == null)
                    throw new SiteNotFoundException("Site " + site.Name + " was not found on IIS");

                iisSite.Stop();
            }
        }

        public class SiteNotFoundException : Exception
        {
            public SiteNotFoundException(string message) : base(message) { }
        }

        public static IisSite GetSiteByName(string name)
        {
            using (var manager = new ServerManager())
            {
                return ParseSite(manager.Sites.SingleOrDefault(x => x.Name == name));
            }
        }

        //public static ManageSiteResult CreateSite(IisSite site)
        //{
        //    var validationResult = Validators.ValidateSite(site, null);
        //    if (validationResult.Errors.Any())
        //    {
        //        return validationResult;
        //    }

        //    var result = new ManageSiteResult();
        //    var bindingInformations = site.Bindings.Select(x => x.ToIisBindingInformation()).ToList();

        //    // Check bindings
        //    var bindingInUse = GetBindingInUse(0, bindingInformations); // 0 never exists
        //    if (bindingInUse != null)
        //    {
        //        result.Result = SiteResult.BindingAlreadyInUse;
        //        return result;
        //    }

        //    using (var manager = new ServerManager())
        //    {
        //        if (manager.Sites.Any(x => x.Name == site.Name))
        //        {
        //            result.Result = SiteResult.NameAlreadyInUse;
        //            return result;
        //        }

        //        // Create site
        //        manager.Sites.Add(site.Name, "http", bindingInformations.First(), site.SitePath);
        //        var iisSite = manager.Sites.SingleOrDefault(x => x.Name == site.Name);

        //        // Add bindings
        //        iisSite.Bindings.Clear();
        //        foreach (var binding in bindingInformations)
        //            iisSite.Bindings.Add(binding, "http");

        //        // Set/create application pool
        //        if (string.IsNullOrWhiteSpace(site.ApplicationPool)) // Auto create application pool
        //        {
        //            var appPoolName = site.Name;
        //            var existingApplicationPoolNames = manager.ApplicationPools.Select(x => x.Name).ToList();
        //            var newNameCount = 1;

        //            while (existingApplicationPoolNames.Contains(appPoolName))
        //            {
        //                appPoolName = site.Name + "_" + newNameCount;
        //                newNameCount++;
        //            }

        //            manager.ApplicationPools.Add(appPoolName);
        //            iisSite.ApplicationDefaults.ApplicationPoolName = appPoolName;
        //        }
        //        else
        //        {
        //            iisSite.ApplicationDefaults.ApplicationPoolName = site.ApplicationPool;
        //        }

        //        //Add Virtual apps/directories
        //        foreach (var application in site.Applications)
        //        {
        //            if (!application.Path.StartsWith("/"))
        //                application.Path = "/" + application.Path;

        //            if (application.IsApplication)
        //            {
        //                if (application.Path.EndsWith("/"))
        //                {
        //                    application.Path.Remove(application.Path.Length - 1, 1);
        //                }

        //                iisSite.Applications.Add(application.Path, application.DiskPath);
        //            }
        //            else // Directory
        //            {
        //                iisSite.Applications.First().VirtualDirectories.Add(application.Path, application.DiskPath);
        //            }
        //        }

        //        manager.CommitChanges();

        //        var created = false;
        //        var sw = new Stopwatch();
        //        sw.Start();
        //        while (!created && sw.Elapsed.TotalSeconds < 3)
        //        {
        //            try
        //            {
        //                if (iisSite.State == ObjectState.Started || iisSite.State == ObjectState.Stopped)
        //                {
        //                    created = true;
        //                }
        //            }
        //            catch (COMException)
        //            {
        //                System.Threading.Thread.Sleep(100);
        //            }

        //        }
        //        sw.Stop();

        //        if (created)
        //        {
        //            result.Result = SiteResult.Success;
        //            result.IisSiteId = (int)iisSite.Id;
        //        }
        //        else
        //        {
        //            result.Result = SiteResult.Failed;
        //        }

        //        return result;
        //    }
        //}

        private static string GetBindingInUse(int iisId, List<string> bindingInformations)
        {
            // IIS only allows one of each https binding.
            var httpsBindings = new List<string>();

            using (var manager = new ServerManager())
            {
                var sites = manager.Sites.Where(x => x.Id != iisId);
                foreach (var iisSite in sites)
                    foreach (var binding in iisSite.Bindings)
                    {
                        if (binding.Protocol == "https")
                            httpsBindings.Add(binding.BindingInformation.Substring(0, binding.BindingInformation.LastIndexOf(":")));

                        if (bindingInformations.Contains(binding.BindingInformation))
                            return binding.BindingInformation;
                    }

                foreach (var binding in bindingInformations)
                {
                    var ipPortCombi = binding.Substring(0, binding.LastIndexOf(":"));
                    if (httpsBindings.Contains(ipPortCombi))
                        return binding;
                }

                return null;
            }
        }

        public static void RestartSite(int iisSiteId)
        {
            var site = GetSiteById(iisSiteId);
            StopSite(site);
            StartSite(site);
        }

        public static void DeleteSite(int iisId)
        {
            using (var manager = new ServerManager())
            {
                var siteToDelete = manager.Sites.SingleOrDefault(x => x.Id == iisId);

                if (siteToDelete == null)
                    return;

                var applicationPoolname = siteToDelete.Applications[0].ApplicationPoolName;

                var sitesWithApplicationPoolname =
                    from site in manager.Sites
                    let application = site.Applications[0]
                    where application.ApplicationPoolName == applicationPoolname
                    select site;

                siteToDelete.Delete();

                if (sitesWithApplicationPoolname.Count() == 1)
                    manager.ApplicationPools[applicationPoolname].Delete();

                manager.CommitChanges();
            }

            System.Threading.Thread.Sleep(500);
        }

        #region 应用池管理方法
        //public static void UpdateApplicationPool(string originalPoolName, ApplicationPool applicationPool)
        //{
        //    using (var manager = new ServerManager())
        //    {
        //        var pool = manager.ApplicationPools.Single(x => x.Name == originalPoolName);

        //        var apps = manager.Sites.SelectMany(x => x.Applications);
        //        foreach (var app in apps.Where(x => x.ApplicationPoolName == pool.Name))
        //        {
        //            app.ApplicationPoolName = applicationPool.Name;
        //        }

        //        pool = pool.UpdateIisApplicationPoolFromServant(applicationPool);

        //        manager.CommitChanges();
        //    }
        //}

        public static ApplicationPool GetDefaultApplicationPool()
        {
            using (var manager = new ServerManager())
            {
                return manager.ApplicationPools.FirstOrDefault();
                //return new ApplicationPool
                //{
                //    ClrVersion = manager.ApplicationPoolDefaults.ManagedRuntimeVersion,
                //    PipelineMode = manager.ApplicationPoolDefaults.ManagedPipelineMode.ToString().ToLower(),
                //    AutoStart = manager.ApplicationPoolDefaults.AutoStart,
                //    DisallowOverlappingRotation = manager.ApplicationPoolDefaults.Recycling.DisallowOverlappingRotation,
                //    DisallowRotationOnConfigChange = manager.ApplicationPoolDefaults.Recycling.DisallowRotationOnConfigChange,
                //    RecycleInterval = manager.ApplicationPoolDefaults.Recycling.PeriodicRestart.Time,
                //    RecyclePrivateMemoryLimit = manager.ApplicationPoolDefaults.Recycling.PeriodicRestart.PrivateMemory,
                //    RecycleVirtualMemoryLimit = manager.ApplicationPoolDefaults.Recycling.PeriodicRestart.Memory,
                //    RecycleRequestsLimit = manager.ApplicationPoolDefaults.Recycling.PeriodicRestart.Requests,
                //    IdleTimeout = manager.ApplicationPoolDefaults.ProcessModel.IdleTimeout,
                //    MaximumWorkerProcesses = manager.ApplicationPoolDefaults.ProcessModel.MaxProcesses,
                //    PingingEnabled = manager.ApplicationPoolDefaults.ProcessModel.PingingEnabled,
                //    PingInterval = manager.ApplicationPoolDefaults.ProcessModel.PingInterval,
                //    PingMaximumResponseTime = manager.ApplicationPoolDefaults.ProcessModel.PingResponseTime,
                //    ServiceUnavailableResponseType = manager.ApplicationPoolDefaults.Failure.LoadBalancerCapabilities.ToString().ToLower(),
                //    RapidFailProtectionEnabled = manager.ApplicationPoolDefaults.Failure.RapidFailProtection,
                //    RapidFailProtectionInterval = manager.ApplicationPoolDefaults.Failure.RapidFailProtectionInterval,
                //    RapidFailProtectionMaxCrashes = manager.ApplicationPoolDefaults.Failure.RapidFailProtectionMaxCrashes
                //};
            }
        }
        /// <summary>
        /// 根据应用程序池名称，启动一个应用程序池
        /// </summary>
        /// <param name="poolName"></param>
        public static void StartApplicationPool(string poolName)
        {
            using (var manager = new ServerManager())
            {
                var pool = manager.ApplicationPools.Single(x => x.Name == poolName);
                pool.Start();
            }
        }
        /// <summary>
        /// 根据应用程序池名称，停止一个应用程序池
        /// </summary>
        public static void StopApplicationPool(string poolName)
        {
            using (var manager = new ServerManager())
            {
                var pool = manager.ApplicationPools.Single(x => x.Name == poolName);
                pool.Stop();
            }
        }
        /// <summary>
        /// 根据应用程序池名称，回收一个应用程序池
        /// </summary>
        /// <param name="poolName"></param>
        public static void RecycleApplicationPool(string poolName)
        {
            using (var manager = new ServerManager())
            {
                var pool = manager.ApplicationPools.Single(x => x.Name == poolName);
                pool.Recycle();
            }
        }
        /// <summary>
        /// 根据应用程序池名称，删除一个应用程序池
        /// </summary>
        /// <param name="poolName"></param>
        public static void DeleteApplicationPool(string poolName)
        {
            using (var manager = new ServerManager())
            {
                var pool = manager.ApplicationPools.Single(x => x.Name == poolName);
                pool.Delete();
                manager.CommitChanges();
            }
        }
        ///// <summary>
        ///// 创建应用程序池
        ///// </summary>
        ///// <param name="applicationPool"></param>
        //public static void CreateApplicationPool(ApplicationPool applicationPool)
        //{
        //    using (var manager = new ServerManager())
        //    {
        //        Microsoft.Web.Administration.ApplicationPool newAppPool = manager.ApplicationPools.Add(applicationPool.Name);
        //        newAppPool.UpdateIisApplicationPoolFromServant(applicationPool);
        //        manager.CommitChanges();
        //    }
        //} 
        #endregion
    }


    public enum SiteStartResult
    {
        Started,
        BindingIsAlreadyInUse,
        PortInUseByAnotherService,
        CannotAccessSitePath,
        IISError
    }

    public enum SiteResult
    {
        UnknownSiteId,
        NameAlreadyInUse,
        BindingAlreadyInUse,
        Success,
        Failed,
        ValidationError,
        SiteNameNotFound
    }
}