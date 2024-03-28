Ext.define('B4.model.objectcr.TypeWorkCrPotentialWorks', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCrWorks',
        listAction: 'ListPotential'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Year' }
    ]
});