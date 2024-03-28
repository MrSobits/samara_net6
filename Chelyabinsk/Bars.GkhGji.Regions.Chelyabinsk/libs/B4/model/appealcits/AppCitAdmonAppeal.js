Ext.define('B4.model.appealcits.AppCitAdmonAppeal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppCitAdmonAppeal'
    },
    
    fields: [
        { name: 'Id' },
        { name: 'AppealCits' },
        { name: 'AppealCitsAdmonition' },

        { name: 'AppealNumber' },
        { name: 'AppealDate' },
        { name: 'Correspondent' },
        { name: 'Address' }
        ]
});