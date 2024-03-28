Ext.define('B4.model.preventivevisit.Witness', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitWitness'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisit', defaultValue: null },
        { name: 'Position' },
        { name: 'IsFamiliar' },
        { name: 'Fio' }
    ]
});