namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using B4.Utils.Annotations;

    public class Lookuper
    {
        private readonly string _path;

        public Lookuper(string path)
        {
            ArgumentChecker.NotNullOrEmpty(path, "path");

            _path = path;
        }

        public string Lookup()
        {
            return _path;
        } 
    }
}