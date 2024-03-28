Ext.define('B4.model.dict.programcr.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCr',
        listAction: 'RealityObjectList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address' }
    ]
});