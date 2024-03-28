Ext.define('B4.model.actcheck.Witness', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckWitness'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'Position' },
        { name: 'IsFamiliar' },
        { name: 'Fio' }
    ]
});