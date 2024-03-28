Ext.define('B4.model.objectcr.TypeWorkCrHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCrHistory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'WorkName'},
        { name: 'TypeWorkCr' },
        { name: 'TypeReason', DefaultValue:0 },
        { name: 'TypeAction' },
        { name: 'FinanceSourceName' },
        { name: 'Volume' },
        { name: 'Sum' },
        { name: 'YearRepair' },
        { name: 'NewYearRepair' },
        { name: 'UserName' },
        { name: 'ObjectCreateDate' },
        { name: 'NameDoc' },
        { name: 'DateDoc' },
        { name: 'FileDoc' },
        { name: 'NumDoc' },
        { name: 'Description' },
        { name: 'StructElement' }
    ]
});