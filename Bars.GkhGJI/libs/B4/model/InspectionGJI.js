Ext.define('B4.model.InspectionGji', {
    extend: 'B4.base.Model',
    
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeBase', defaultValue: 10 },
        { name: 'DisposalId' },
        { name: 'Contragent', defaultValue: null },
        { name: 'InspectionBaseType', defaultValue: null },
        { name: 'TypeJurPerson', defaultValue: 10 },
        { name: 'InspectionYear' },
        { name: 'InspectionNum' },
        { name: 'InspectionNumber' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'PersonInspection', defaultValue: 20 },
        { name: 'RegistrationNumber' },
        { name: 'RegistrationNumberDate' },
        { name: 'RiskCategory' },
        { name: 'RiskCategoryStartDate' },
        { name: 'CheckDayCount' },
        { name: 'ControlType' },
        { name: 'CheckDate' }, 
        { name: 'ReasonErpChecking', defaultValue: 0 }
    ]
});