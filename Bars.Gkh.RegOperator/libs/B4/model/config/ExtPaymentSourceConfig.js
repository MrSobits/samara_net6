Ext.define('B4.model.config.ExtPaymentSourceConfig', {
    extend: 'B4.model.config.PaymentSourceConfig',
    idProperty: 'Path',
    proxy: {
        type: 'memory'
    },
    fields: [
        { name: 'iconCls', type: 'string', defaultValue: 'treenode-no-icon' },
        { name: 'expanded', defaultValue: true },
        { name: 'сhildren' },
        { name: 'checked', defaultValue: false },

        { name: 'Code' },
        { name: 'Name' },
        { name: 'Description' }
    ]
});