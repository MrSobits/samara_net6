Ext.define('B4.model.dict.OrganMvd', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OrganMvd'
    },
    fields: [
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Municipality' }
    ]
});