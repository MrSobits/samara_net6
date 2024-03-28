Ext.define('B4.model.dict.GroupWorkPpr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GroupWorkPpr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Service', defaultValue: null }
    ]
});