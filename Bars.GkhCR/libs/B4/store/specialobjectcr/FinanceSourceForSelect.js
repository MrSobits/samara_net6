Ext.define('B4.store.specialobjectcr.FinanceSourceForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.FinanceSource'],
    autoLoad: false,
    model: 'B4.model.dict.FinanceSource',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialTypeWorkCr',
        listAction: 'ListFinanceSources'
    }
});