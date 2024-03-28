namespace Bars.Gkh.DocIoGenerator
{
    using System.IO;

    public class PictureParam
    {
        public bool ChangeSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Stream Value { get; set; }
    }
}