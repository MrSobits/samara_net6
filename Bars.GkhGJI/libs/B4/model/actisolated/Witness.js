Ext.define('B4.model.actisolated.Witness', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedWitness'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'Position' },
        { name: 'IsFamiliar' },
        { name: 'Fio' }
    ]
});