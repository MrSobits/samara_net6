Ext.define('B4.model.appealcits.AppealOrderFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealOrderFile'
    },
    fields: [
        { name: 'AppealOrder', defaultValue: null },
        { name: 'FileInfo', defaultValue: null },
        { name: 'ObjectCreateDate' },
        { name: 'Description' }
    ]
});