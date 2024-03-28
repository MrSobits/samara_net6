Ext.define('B4.model.passport.PassportStruct', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PassportStruct'
    },

    fields: [
        { name: 'Name' },
        { name: 'PassportType' },
        { name: 'ValidFromMonth' },
        { name: 'ValidFromYear' }
    ]
});