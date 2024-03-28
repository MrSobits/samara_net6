Ext.define('B4.view.regop.bankstatement.DetailGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.store.DistributionDetail'
    ],

    alias: 'widget.rbankstatementdetailgrid',

    entityId: null,
    source: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.DistributionDetail');

        store.on('beforeload', function(s, oper) {
            oper.params['entityId'] = me.entityId;
            oper.params['source'] = me.source;
        });

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
            columns: [
                {
                    text: 'Объект',
                    dataIndex: 'Object',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: '№ Р/с',
                    dataIndex: 'PaymentAccount',
                    flex: .5,
                    filter: {
                        xtype: 'textfield'
                    },
                    hidden: true
                },
                {
                    text: 'Сумма',
                    flex: .5,
                    dataIndex: 'Sum',
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: 'true',
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Назначение',
                    dataIndex: 'Destination',
                    flex: .5,
                    filter: {
                        xtype: 'textfield'
                    },
                    hidden: true
                },
                {
                    dataIndex: 'UserLogin',
                    text: 'Пользователь',
                    flex: 0.5,
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });
        me.callParent(arguments);

    }
});