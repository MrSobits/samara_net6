Ext.define('B4.model.integrations.infrastructure.Rki', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Infrastructure',
        listAction: 'GetRkiList'
    },
    fields: [
        { name: 'Name' },
        { name: 'Address' },
        { name: 'TypeGroupName' },
        { name: 'TypeName' }
    ]
});
