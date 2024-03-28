namespace Bars.Gkh.Regions.Tyumen.Permissions
{
    using B4;

    public class GkhPermissionMap : PermissionMap
    {
        public GkhPermissionMap()
        {
            this.Namespace("Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment", "Вопросы");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment.Edit", "Редактирвоание");
            this.Namespace("Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment.Field", "Поля");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment.Field.ExecutorType",
                "Тип контрагента - редактирование");

            this.Namespace("Gkh.Dictionaries.TypeLiftShaft", "Типы шахт лифта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeLiftShaft");

            this.Namespace("Gkh.Dictionaries.TypeLiftDriveDoors", "Типы привода дверей кабины лифта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeLiftDriveDoors");

            this.Namespace("Gkh.Dictionaries.TypeLiftMashineRoom", "Расположение машинного помещения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeLiftMashineRoom");

            this.Namespace("Gkh.Dictionaries.TypeLift", "Типы лифта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeLift");

            this.Namespace("Gkh.Dictionaries.ModelLift", "Модель лифта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ModelLift");

            this.Namespace("Gkh.Dictionaries.CabinLift", "Лифт (кабина)");
            this.CRUDandViewPermissions("Gkh.Dictionaries.CabinLift");

            this.Namespace("Gkh.RealityObject.Register.Lift", "Лифты");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Lift");
        }
    }
}