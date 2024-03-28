Ext.define('B4.store.MethodFormFundForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.MethodFormFund'],
    model: 'B4.model.MethodFormFund',
    data: B4.enums.MethodFormFund.getItemsMeta()
});