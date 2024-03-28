Ext.define('B4.model.dict.ContentRepairMkdWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContentRepairMkdWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Work' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Description' }
    ]
});
