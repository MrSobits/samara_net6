Ext.define('B4.model.finactivity.ManagRealityObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityManagRealityObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectId', defaultValue: null },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AddressName' },
        { name: 'AreaMkd' },
        { name: 'PresentedToRepay', defaultValue: null },
        { name: 'ReceivedProvidedService', defaultValue: null },
        { name: 'SumDebt', defaultValue: null },
        { name: 'SumFactExpense', defaultValue: null },
        { name: 'SumIncomeManage', defaultValue: null }
    ]
});
