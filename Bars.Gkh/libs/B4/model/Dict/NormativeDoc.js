Ext.define('B4.model.dict.NormativeDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormativeDoc'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'FullName' },
        { name: 'Code' },
        { name: 'Category' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'TorId'}
    ]
});