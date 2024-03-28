namespace Bars.Gkh.Diagnostic.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using Microsoft.VisualBasic.ApplicationServices;

    [Serializable]
    public class SerializableAssemblyInfo
    {
        protected bool Equals(SerializableAssemblyInfo other)
        {
            return string.Equals(this.AssemblyName, other.AssemblyName) && string.Equals(this.AssemblyVersion, other.AssemblyVersion);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.AssemblyName != null ? this.AssemblyName.GetHashCode() : 0) * 397) ^ (this.AssemblyVersion != null ? this.AssemblyVersion.GetHashCode() : 0);
            }
        }

        public SerializableAssemblyInfo(Assembly assembly)
        {
            this.AssemblyName = assembly.GetName().Name;

            this.AssemblyVersion = assembly.GetName().Version.ToString();
        }

        public string AssemblyName { get; private set; }

        public string AssemblyVersion { get; private set; }

        public override bool Equals(System.Object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((SerializableAssemblyInfo)obj);
        }
    }
}
