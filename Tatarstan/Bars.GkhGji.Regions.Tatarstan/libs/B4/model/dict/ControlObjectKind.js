Ext.define('B4.model.dict.ControlObjectKind', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlObjectKind'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ControlObjectType'},
        { name: 'ControlType'},
        { name: 'ErvkId' }
    ]
});