namespace Bars.Gkh.RegOperator.Permissions
{
    using Bars.Gkh.DomainService;

    internal class RegOperatorFieldRequirementMap : FieldRequirementMap
    {
        public RegOperatorFieldRequirementMap()
        {
            this.Namespace("GkhRegOp", "Модуль РегОператор");
            this.Namespace("GkhRegOp.PersonalAccountOwner", "Абоненты");
            this.Namespace("GkhRegOp.PersonalAccountOwner.Field", "Поля");

            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.BirthDate_Rqrd", "Дата рождения");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.RegistrationAddress_Rqrd", "Адрес прописки");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.IdentityType_Rqrd", "Тип документа");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.IdentitySerial_Rqrd", "Серия документа");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.IdentityNumber_Rqrd", "Номер документа");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Field.Patronymic_Rqrd", "Отчество");

            this.Namespace("GkhRegOp.PersonalAccountOwner.Account", "Сведения о помещениях");
            this.Namespace("GkhRegOp.PersonalAccountOwner.Account.Field", "Поля");

            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.CreateDate_Rqrd", "Дата открытия ЛС");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.ContractNumber_Rqrd", "Номер договора");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.ContractDate_Rqrd", "Дата заключения договора");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.ContractDocument_Rqrd", "Файл");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.DateDocumentIssuance_Rqrd", "Дата выдачи документа");
            this.Requirement("GkhRegOp.PersonalAccountOwner.Account.Field.Gender_Rqrd", "Пол");


            this.Namespace("GkhRegOp.Accounts", "Счета");
            this.Namespace("GkhRegOp.Accounts.BankOperations", "Банковские операции");
            this.Namespace("GkhRegOp.Accounts.BankOperations.Field", "Поля");
            this.Requirement("GkhRegOp.Accounts.BankOperations.Field.RecipientAccountNum_Rqrd", "Р/С получателя");


            this.Namespace("Gkh.RealityObject.Protocols", "Протоколы решений");
            this.Namespace("Gkh.RealityObject.Protocols.GovDecision", "Протокол ОГВ");
            this.Namespace("Gkh.RealityObject.Protocols.GovDecision.Fields", "Поля");
            this.Requirement("Gkh.RealityObject.Protocols.GovDecision.Fields.MinFundPaymentSize_Rqrd", "Минимальный размер взноса на КР");
        }
    }
}
