Ext.define('B4.model.specialobjectcr.PerformedWorkAct', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialPerformedWorkAct'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'TypeWorkCr', defaultValue: null },
        { name: 'WorkName' },
        { name: 'Municipality' },
        { name: 'WorkFinanceSource' },
        { name: 'DocumentNum' },
        { name: 'Volume', defaultValue: null },
        { name: 'Sum', defaultValue: null },
        { name: 'DateFrom', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'Address' },
        { name: 'ObjectCrId', defaultValue: null },
        { name: 'DateFrom' },
        { name: 'DocumentNum' },
        { name: 'CostFile' },
        { name: 'DocumentFile' },
        { name: 'AdditionFile' },
        { name: 'IsWork' },
        { name: 'UsedInExport', defaultValue: 20 },
        { name: 'RepresentativeSigned', defaultValue: 20 },
        { name: 'RepresentativeSurname', defaultValue: null },
        { name: 'RepresentativeName', defaultValue: null },
        { name: 'RepresentativePatronymic', defaultValue: null },
        { name: 'ExploitationAccepted', defaultValue: 20 },
        { name: 'WarrantyStartDate', defaultValue: null },
        { name: 'WarrantyEndDate', defaultValue: null }
    ]
});