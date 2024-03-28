Ext.define('B4.model.regoperator.ServicedHouse', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address', defaultValue: null }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegopCalcAccountRealityObject'
    }
});