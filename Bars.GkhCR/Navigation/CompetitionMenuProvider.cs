namespace Bars.GkhCr.Navigation
{
    using B4;

    /// <summary>
    /// 
    /// </summary>
    public class CompetitionMenuProvider : INavigationProvider
    {
        /// <summary>
        /// Ключ меню
        /// </summary>
        public string Key
        {
            get
            {
                return "Competition";
            }
        }

        /// <summary>
        /// Описание меню
        /// </summary>
        public string Description
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Метод инициализации
        /// </summary>
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "competitionedit/{0}/edit").AddRequiredPermission("GkhCr.Competition.View");
            root.Add("Лоты", "competitionedit/{0}/lot").AddRequiredPermission("GkhCr.Competition.Lot.View");
            root.Add("Документы", "competitionedit/{0}/doc").AddRequiredPermission("GkhCr.Competition.Document.View");
            root.Add("Протоколы", "competitionedit/{0}/protocol").AddRequiredPermission("GkhCr.Competition.Protocol.View");
        }
    }
}