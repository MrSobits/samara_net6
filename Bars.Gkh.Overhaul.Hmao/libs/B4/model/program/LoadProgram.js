Ext.define('B4.model.program.LoadProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LoadProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IndexNumber' },
        { name: 'Locality' },
        { name: 'Street' },
        { name: 'House' },
        { name: 'Housing' },
        { name: 'Address' },
        { name: 'CommissioningYear' },
        { name: 'CommonEstateobject' },
        { name: 'Wear' },
        { name: 'LastOverhaulYear' },
        { name: 'PlanOverhaulYear' }
    ]
});