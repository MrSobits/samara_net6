namespace Bars.Gkh.Report
{
    /// <summary>
    /// Класс для вывода описания о габлонах отчета, т.к. их может быть много
    /// </summary>
    public class TemplateInfo
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public byte[] Template { get; set; }
    }
}