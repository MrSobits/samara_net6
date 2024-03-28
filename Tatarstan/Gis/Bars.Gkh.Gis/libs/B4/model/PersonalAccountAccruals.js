Ext.define('B4.model.PersonalAccountAccruals', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountAccruals'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'Supplier' },
        { name: 'BalanceIn' },
        { name: 'TariffAmount' },
        { name: 'BackorderAmount' },
        { name: 'RecalcAmount' },
        { name: 'ErcAmount' },
        { name: 'SupplierAmount' },
        { name: 'BalanceOut' }
    ]
});