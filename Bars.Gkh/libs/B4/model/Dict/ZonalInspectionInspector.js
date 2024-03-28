Ext.define('B4.model.dict.ZonalInspectionInspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ZonalInspectionInspector'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Fio' },
        { name: 'Code' }
    ]
});