Ext.define('B4.model.EmergencyObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.ConditionHouse'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmergencyObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActualInfoDate' },
        { name: 'CadastralNumber' },
        { name: 'AddressName' },
        { name: 'FurtherUseName' },
        { name: 'DemolitionDateShort' },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'FileInfo', defaultValue: null },
        { name: 'Description' },
        { name: 'LandArea' },
        { name: 'ResettlementFlatArea' },
        { name: 'TotalArea' },
        { name: 'InhabitantNumber' },
        { name: 'IsRepairExpedient' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'FurtherUse', defaultValue: null },
        { name: 'ReasonInexpedient', defaultValue: null },
        { name: 'ConditionHouse', defaultValue: 10 },
        { name: 'ResettlementProgram', defaultValue: null },
        { name: 'ResettlementProgramName' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'ResettlementFlatAmount' },
        { name: 'ExemptionsBasis' },
        { name: 'EmergencyDocumentName' },
        { name: 'EmergencyDocumentNumber' },
        { name: 'EmergencyDocumentDate', defaultValue: null },
        { name: 'EmergencyFileInfo', defaultValue: null },
        { name: 'DemolitionDate', defaultValue: null },
        { name: 'ResettlementDate', defaultValue: null },
        { name: 'FactDemolitionDate', defaultValue: null },
        { name: 'FactResettlementDate', defaultValue: null }
    ]
});