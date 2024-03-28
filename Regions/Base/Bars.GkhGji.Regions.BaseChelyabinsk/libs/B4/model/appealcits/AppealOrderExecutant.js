Ext.define('B4.model.appealcits.AppealOrderExecutant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsExecutant',
        listAction: 'ListAppealOrderExecutant'
    },
    fields: [
        { name: 'Fio'},
        { name: 'Position'},
        { name: 'Phone' },
        { name: 'Email' }
    ]
});