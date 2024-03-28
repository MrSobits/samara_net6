Ext.define('B4.model.DisclosureInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'PeriodDi', defaultValue: null },
        { name: 'State', defaultValue: null },

        //количество персонала
        { name: 'AdminPersonnel', defaultValue: null },
        { name: 'Engineer', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'DismissedAdminPersonnel', defaultValue: null },
        { name: 'DismissedEngineer', defaultValue: null },
        { name: 'DismissedWork', defaultValue: null },
        { name: 'UnhappyEventCount', defaultValue: null },

        //поля для отображения
        { name: 'ContragentName' },
        { name: 'PeriodDiName' },
        { name: 'ContragentId' },
        { name: 'ManagingOrgId' },
        { name: 'TypeManagement' },
        { name: 'FioDirector' },
        { name: 'FiasJurAddressName' },
        { name: 'Ogrn' },
        { name: 'OgrnRegistration' },
        { name: 'ActivityDateStart' },
        { name: 'FiasMailAddressName' },
        { name: 'FiasFactAddressName' },
        { name: 'Phone' },
        { name: 'Fax' },
        { name: 'Email' },
        { name: 'OfficialWebsite' },
        { name: 'FioMemberAudit' },
        { name: 'FioMemberManagement' },
        { name: 'IsDispatchCrrespondedFact' },
        { name: 'DispatchPhone' },
        { name: 'DispatchAddress' },
        { name: 'DispatchFile' },
        { name: 'NumberEmployees' },
        
        //единичные поля над гридами разделов
        { name: 'TerminateContract', defaultValue: 30 },
        { name: 'MembershipUnions', defaultValue: 30 },
        { name: 'FundsInfo', defaultValue: 30 },
        { name: 'DocumentWithoutFunds', defaultValue: null },
        { name: 'AdminResponsibility', defaultValue: 30 },
        { name: 'SizePayments', defaultValue: null },
        { name: 'ContractsAvailability', defaultValue: 30 },
        { name: 'NumberContracts', defaultValue: null },
        { name: 'HasLicense' },
        { name: 'ShareMo', defaultValue: null },
        { name: 'ShareSf', defaultValue: null }
    ]
});