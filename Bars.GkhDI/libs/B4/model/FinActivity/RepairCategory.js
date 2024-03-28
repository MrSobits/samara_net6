Ext.define('B4.model.finactivity.RepairCategory', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityRepairCategory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'TypeCategoryHouseDi', defaultValue: 10 },
        { name: 'WorkByRepair', defaultValue: null },
        { name: 'WorkByBeautification', defaultValue: null },
        { name: 'PeriodService', defaultValue: null },
        { name: 'IsInvalid', defaultValue: false }
    ]
});
