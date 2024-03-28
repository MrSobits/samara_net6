namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class ChesImportMenuProvider : INavigationProvider
    {
        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Несопоставленные данные", "chesimport_detail/{0}/comparing");
            root.Add("Сопоставленные данные", "chesimport_detail/{0}/compared");
            root.Add("Сводные суммы", "chesimport_detail/{0}/sums");
            root.Add("Сверка сальдо", "chesimport_detail/{0}/saldocheck");
            root.Add("Разбор файла", "chesimport_detail/{0}/analysis");
        }

        /// <inheritdoc />
        public string Key => "ChesImport";

        /// <inheritdoc />
        public string Description => "Меню импорта сведений из биллинга";
    }
}