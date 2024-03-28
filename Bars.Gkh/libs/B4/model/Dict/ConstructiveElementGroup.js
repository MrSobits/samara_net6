Ext.define('B4.model.dict.ConstructiveElementGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructiveElementGroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Necessarily', defaultValue: false }
    ]
});