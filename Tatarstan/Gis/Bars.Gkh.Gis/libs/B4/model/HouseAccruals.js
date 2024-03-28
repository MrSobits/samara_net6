Ext.define('B4.model.HouseAccruals', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseAccruals'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'BalanceIn' },
        { name: 'TariffAmount' },
        { name: 'BackorderAmount' },
        { name: 'RecalcAmount' },
        { name: 'ErcAmount' },
        { name: 'SupplierAmount' },
        { name: 'BalanceOut' }
    ]
});