Ext.define('B4.model.dict.ControlType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ExternalId'},
        { name: 'Name' },
        { name: 'Level' },
        { name: 'ErvkId' }
    ]
});