Ext.define('B4.model.actremoval.Witness', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemovalWitness'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActRemoval', defaultValue: null },
        { name: 'Position' },
        { name: 'IsFamiliar' },
        { name: 'Fio' }
    ]
});