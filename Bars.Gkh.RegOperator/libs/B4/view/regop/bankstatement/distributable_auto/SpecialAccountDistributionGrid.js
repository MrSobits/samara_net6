Ext.define('B4.view.regop.bankstatement.distributable_auto.SpecialAccountDistributionGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.specialaccountdistributiongrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store',
            {
                autoLoad: false,
                fields: [
                    { name: 'RealtyAccountId' },
                    { name: 'AccountNumber' },
                    { name: 'RealityObject' },
                    { name: 'Sum' },
                    { name: 'Municipality' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'Distribution',
                    listAction: 'ListAutoDistributionObjs'
                }
            });

        store.on('beforeload', function (st, operation) {
            var windows = me.getView().up('window');
            operation.params = operation.params || {};
            if (windows.getDistributionParams) {
                Ext.apply(operation.params, windows.getDistributionParams());
            }
        });
            
        Ext.apply(me, {
            store: store,
            columns: [
                {
                    text: 'Муниципальное образование',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Жилой дом',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Расчетный счет',
                    dataIndex: 'AccountNumber',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Распределеяемая сумма',
                    dataIndex: 'Sum',
					width: 140,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: 'true',
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);

    }
});