Ext.define('B4.model.objectcr.TypeWorkCrRemoval', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCrRemoval'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'WorkName'},
        { name: 'TypeWorkCr' },
        { name: 'TypeReason', DefaultValue:0 },
        { name: 'YearRepair' },
        { name: 'NewYearRepair' },
        { name: 'FileDoc' },
        { name: 'NumDoc' },
        { name: 'DateDoc' },
        { name: 'Description' },
        { name: 'StructElement' },
        { name: 'TypeWorkSt1' }
    ]
});