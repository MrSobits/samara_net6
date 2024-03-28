using System;

namespace SMEV3Library.Entity
{
    public class FileAttachment
    {
        public string FileName;
        public byte[] FileData;

        public override string ToString()
        {
            return FileName;
        }

        public string GetExtension()
        {
            int pointpoint = FileName.LastIndexOf('.');
            if (pointpoint == -1)
                return "";

            return FileName.Substring(pointpoint);
        }
    }
}
