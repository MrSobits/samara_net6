Ext.define('B4.model.confirmcontribution.Record', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConfirmContributionDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConfirmContribution', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Address' },
        { name: 'DocumentNum', defaultValue: null },
        { name: 'DateFrom', defaultValue: null },
        { name: 'TransferDate', defaultValue: null },
        { name: 'Scan', defaultValue: null },
        { name: 'Amount', defaultValue: 0.0}
    ]
});