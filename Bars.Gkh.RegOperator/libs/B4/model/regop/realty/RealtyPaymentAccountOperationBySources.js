Ext.define('B4.model.regop.realty.RealtyPaymentAccountOperationBySources', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'SrcFinanceType' },
        { name: 'Sum' }
    ],
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectAccount',
        listAction: 'GetPaymentAccountBySources'
    }
});