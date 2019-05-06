﻿// MIT License, see COPYING.TXT
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace BitAddict.Aras.Data
{
    /// <summary>
    /// Defines the data required to sync with Aras databases
    ///
    /// Represents an entire arasdb(-local).json file.
    /// </summary>
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public class ArasConfManifest
    {
        /// <summary>
        /// Aras Instance used for development/unit testing
        /// </summary>
        public string DevelopmentInstance { get; set; }
        /// <summary>
        /// A list of Aras DB instances
        /// </summary>
        public List<ArasDb> Instances { get; set; }
        /// <summary>
        /// File copying information
        /// </summary>
        public DeployDllInfo DeployDll { get; set; }
    }

    /// <summary>
    /// Defines an Aras database instance
    /// </summary>
    [UsedImplicitly]
    public class ArasDb
    {
        /// <summary>
        /// Instance id. Used when invoking arassync only. Not relevant for Aras.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// HTTP URL to web server
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Database name as in web login dialog
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// Web server 'binaries', i.e. C:\Program Files (x86)\Aras\TheInstance\
        /// </summary>
        public string BinFolder { get; set; }
    }

    /// <summary>
    /// Configures DeployDLL command with extensions and exclusions
    /// </summary>    
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public class DeployDllInfo
    {
        /// <summary>
        /// Copies these extensions only
        /// </summary>
        public List<string> Extensions { get; set; } = new List<string>
        {
            ".dll", ".pdb"
        };
        /// <summary>
        /// Does not copy files with these fragments in the name
        /// </summary>
        public List<string> Excludes { get; set; } = new List<string>
        {
            "IOM", "InnovatorCore", "Test", "UnitTestFramework"
        };
    }
}
