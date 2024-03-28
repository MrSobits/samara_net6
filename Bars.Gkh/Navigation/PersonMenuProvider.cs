namespace Bars.Gkh.Navigation
{
    using B4;

    public class PersonMenuProvider : INavigationProvider
    {
        public static string Key = "Person";

        string INavigationProvider.Key
        {
            get
            {
                return "Person";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки должностных лиц";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "personedit/{0}/edit").AddRequiredPermission("Gkh.Person.View").WithIcon("icon-outline");

            root.Add("Сведения о дисквалификации", "personedit/{0}/disqualification").AddRequiredPermission("Gkh.Person.PersonDisqualificationInfo.View");
            root.Add("Место работы", "personedit/{0}/placework").AddRequiredPermission("Gkh.Person.PersonPlaceWork.View");
        }
    }
}