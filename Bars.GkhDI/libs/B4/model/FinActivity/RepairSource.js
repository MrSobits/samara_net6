Ext.define('B4.model.finactivity.RepairSource', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityRepairSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'TypeSourceFundsDi', defaultValue: 10 },
        { name: 'Sum', defaultValue: null },
        { name: 'IsInvalid', defaultValue: false }
    ]
});
