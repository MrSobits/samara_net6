Ext.define('B4.model.PersonalAccountParam', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountParam'
        
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ValPrm' }
    ]
});