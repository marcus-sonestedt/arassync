﻿// MIT License, see COPYING.TXT
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BitAddict.Aras.Data;
using Newtonsoft.Json;

namespace BitAddict.Aras.ArasSync.Ops
{
    internal static class Config
    {
        private static DirectoryInfo _solutionDir;

        internal static DirectoryInfo SolutionDir
        {
            get
            {
                if (_solutionDir != null)
                    return _solutionDir;

                var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir, "arasdb.json" )))
                        break;

                    dir = Path.GetDirectoryName(dir);
                }

                if (dir == null)
                    throw new Exception("Failed to find arasdb.json");

                return _solutionDir = new DirectoryInfo(dir);
            }
        }

        public static ArasDb FindDb(string id)
        {
            var arasDbs = (new[] { "arasdb-local.json", "arasdb.json" }
                .Select(mfFile => Path.Combine(SolutionDir.FullName, mfFile))
                .Where(File.Exists)
                .Select(File.ReadAllText)
                .Select(JsonConvert.DeserializeObject<ArasConfManifest>))
                .SelectMany(mf => mf.Instances)
                .ToList();

            var arasDb = arasDbs.FirstOrDefault(
                db => string.Equals(db.Id, id, StringComparison.CurrentCultureIgnoreCase));

            if (arasDb == null)
                throw new UserMessageException($"Couldn't find Aras instance '{id}'. " +
                                               $"Available DBs: {string.Join(", ", arasDbs.Select(db => db.Id))}");

            return arasDb;
        }

        internal static DeployDllInfo GetDeployDllInfo()
        {
            var arasConf = (new[] {"arasdb-local.json", "arasdb.json"}
                .Select(mfFile => Path.Combine(SolutionDir.FullName, mfFile))
                .Where(File.Exists)
                .Select(File.ReadAllText)
                .Select(JsonConvert.DeserializeObject<ArasConfManifest>))
                .FirstOrDefault();

            if (arasConf == null)
                throw new UserMessageException($"No arasdb(-local).json found at {SolutionDir.FullName}.");

            if (arasConf.DeployDll == null)
                throw new UserMessageException("No DeployDll section found in arasdb(-local).json.");

            return arasConf.DeployDll;
        }
    }
}
