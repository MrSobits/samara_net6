Ext.define('B4.view.suspenseaccount.NewGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.newsuspenseaccountgrid',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.form.ComboBox',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Import',

        'B4.store.regop.SuspenseAccount',
        'B4.enums.SuspenseAccountTypePayment',
        'B4.enums.SuspenseAccountStatus',
        'B4.enums.MoneyDirection',
        'B4.enums.DistributionState'
    ],

    title: 'Счета невыясненных сумм',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.SuspenseAccount');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateReceipt',
                    format: 'd.m.Y',
                    maxWidth: 110,
                    text: 'Дата поступления',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'SuspenseAccountTypePayment',
                    width: 120,
                    text: 'Тип платежа',
                    renderer: function (val) {
                        return B4.enums.SuspenseAccountTypePayment.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.SuspenseAccountTypePayment.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    text: 'Расчетный счет получателя',
                    dataIndex: 'AccountBeneficiary',
                    flex: 0.5,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'MoneyDirection',
                    enumName: 'B4.enums.MoneyDirection', // перечисление
                    filter: true, // создать фильтр
                    width: 100,
                    text: 'Приход/расход'
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    maxWidth: 110,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Назначение платежа',
                    dataIndex: 'DetailsOfPayment',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'DistributeState',
                    enumName: 'B4.enums.DistributionState', // перечисление
                    filter: true, // создать фильтр
                    width: 130,
                    text: 'Статус'
                },
                {
                    text: 'Причина',
                    dataIndex: 'Reason',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            items: [
                                {
                                    margin: 2,
                            xtype: 'checkbox',
                            name: 'ShowDistributed',
                                    boxLabel: 'Показать распределенные',
                                    fieldStyle: 'vertical-align: middle;'
                                }
                            ]
                        }
                    ]
                },
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