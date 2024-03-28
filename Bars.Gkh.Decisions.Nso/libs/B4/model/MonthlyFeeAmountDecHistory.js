Ext.define('B4.model.MonthlyFeeAmountDecHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MonthlyFeeAmountDecHistory'
    },
    fields: [
        { name: 'Id' },
        { name: 'Protocol' },
        { name: 'From' },
        { name: 'To' },
        { name: 'Value' },
        { name: 'UserName' },
        { name: 'ParameterName' },
        { name: 'ObjectCreateDate' },
        { name: 'ProtocolDate' }
    ]
});