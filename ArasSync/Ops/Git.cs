﻿// MIT License, see COPYING.TXT
using System;

namespace BitAddict.Aras.ArasSync.Ops
{
    internal static class Git
    {
        public static string GetRepoStatusString()
        {
            var output = "";

            if (Common.RunProcess("git.exe", true, ref output,
                "describe", "--long", "--dirty", "--abbrev=10", "--tags", "--always")
                != 0)
            {
                throw new Exception("Git failed!");
            }

            return output;
        }
    }
}