Ext.define('B4.model.manorglicense.LicensePrescription', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicensePrescription'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'ArticleLawGji' },
        { name: 'ActualDate' },
        { name: 'FileInfo' },
        { name: 'YesNoNotSet' },
        { name: 'SanctionGji' },
        { name: 'Penalty' },
        { name: 'MorgContractRO' }
    ]
});