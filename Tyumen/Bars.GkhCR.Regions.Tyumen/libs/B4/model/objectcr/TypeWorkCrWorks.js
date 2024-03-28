Ext.define('B4.model.objectcr.TypeWorkCrWorks', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCrWorks'
    },
    fields: [
        { name: 'Id'},
        { name: 'Name' },
        { name: 'Year' },
        { name: 'Cost' }
    ]
});