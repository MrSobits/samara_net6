Ext.define('B4.model.EstimateRegister', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EstimateCalculation'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'RealityObjName' },
        { name: 'TypeWorkCr', defaultValue: null },
        { name: 'TypeWorkCrCount' },
        { name: 'EstimateCalculationsCount' },
        { name: 'Municipality'},
        { name: 'ProgramCrName'},
        { name: 'ObjectCrId' }
    ]
});