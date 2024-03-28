Ext.define('B4.model.actcheck.Familiarized', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckFamiliarized'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'Fio' }
    ]
});