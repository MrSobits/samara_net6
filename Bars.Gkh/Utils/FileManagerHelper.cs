namespace Bars.Gkh.Utils
{
    using System;
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    public class FileManagerHelper : IDisposable
    {
        private bool commited = false;
        private readonly SafeFileHandle tx = null;

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            readonly int nLength;
            readonly IntPtr lpSecurityDescriptor;
            readonly int bInheritHandle;
        }

        [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateTransaction(SECURITY_ATTRIBUTES securityAttributes, IntPtr guid, int options, int isolationLevel, int isolationFlags, int milliSeconds, string description);

        [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CommitTransaction(SafeFileHandle transaction);

        [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool RollbackTransaction(SafeFileHandle transaction);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeleteFileTransacted(string filename, SafeFileHandle transaction);

        public FileManagerHelper()
        {
            this.tx = FileManagerHelper.CreateTransaction(new FileManagerHelper.SECURITY_ATTRIBUTES(), IntPtr.Zero, 0, 0, 0, 0, null);
        }

        public bool DeleteFile(string filename)
        {
            return FileManagerHelper.DeleteFileTransacted(filename, this.tx);
        }

        public void Commit()
        {
            if (FileManagerHelper.CommitTransaction(this.tx))
                this.commited = true;
        }

        private void Rollback()
        {
            FileManagerHelper.RollbackTransaction(this.tx);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.commited)
                {
                    this.Rollback();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}