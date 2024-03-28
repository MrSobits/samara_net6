Ext.define('B4.model.RisContragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RisContragent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FullName' },
        { name: 'Ogrn' },
        { name: 'JuridicalAddress' }
    ]
});
