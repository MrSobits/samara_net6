namespace SMEV3Library.Entities
{
    public class FileAttachment
    {
        public string FileName;
        public byte[] FileData;
        public string SignerInfo;

        //не в guid формате!!!
        public string FileGuid;

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
    public class FsAttachmentProxy
    {
        public string uuid;
        public string UserName;
        public string Password;
        public string FileName;
    }

}
