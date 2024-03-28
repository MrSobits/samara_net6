Ext.define('B4.model.PreliminaryCheck', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreliminaryCheck'
    },
    fields: [
        { name: 'Id' },
        { name: 'AppealCits'},
        { name: 'Inspector'},
        { name: 'CheckDate'},
        { name: 'PreliminaryCheckNumber' },
        { name: 'Result' },
        { name: 'PreliminaryCheckResult', defaultValue: 0 },
        { name: 'Contragent' }, 
        { name: 'FileInfo' }
    ]
});