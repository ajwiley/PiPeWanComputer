using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    internal class ProjectSourcePath {
        private const string myRelativePath = nameof(ProjectSourcePath) + ".cs";
        private static string? lazyValue;
        public static string Value => lazyValue ??= calculatePath();
        public static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";
        private static string calculatePath() {
            string pathName = GetSourceFilePathName();
            Debug.Assert(pathName.EndsWith(myRelativePath, StringComparison.Ordinal));
            return pathName.Substring(0, pathName.Length - myRelativePath.Length);
        }
    }
}
