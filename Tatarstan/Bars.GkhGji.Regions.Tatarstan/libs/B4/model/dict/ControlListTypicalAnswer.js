Ext.define('B4.model.dict.ControlListTypicalAnswer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlListTypicalAnswer'
    },
    fields: [
        { name: 'TorId' },
        { name: 'Answer' }
    ]
});