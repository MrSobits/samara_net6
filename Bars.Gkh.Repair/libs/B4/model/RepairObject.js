Ext.define('B4.model.RepairObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'RepairProgram' },
        { name: 'RealityObjName' },
        { name: 'RepairProgramName' },
        { name: 'Municipality' },
        { name: 'State', defaultValue: null },
        { name: 'Builder' },
        { name: 'ReasonDocument' },
        { name: 'Comment' }
    ]
});