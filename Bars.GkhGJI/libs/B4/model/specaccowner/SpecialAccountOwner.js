Ext.define('B4.model.specaccowner.SpecialAccountOwner', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountOwner'
    },
    fields: [
        { name: 'Id'},
        { name: 'ActivityDateEnd' },
        { name: 'ActivityGroundsTermination' },
        { name: 'Contragent' },  
        { name: 'Inn' },  
        { name: 'OrgStateRole' },
        { name: 'Description' }
        
    ]
});