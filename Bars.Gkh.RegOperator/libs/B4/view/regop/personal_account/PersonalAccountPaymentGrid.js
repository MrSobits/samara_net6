Ext.define('B4.view.regop.personal_account.PersonalAccountPaymentGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.papaymentgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.column.Number',
        'Ext.grid.column.Date',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.store.regop.personal_account.PaymentsInfo',
        'B4.enums.TypeTransferSource',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Оплаты',

    margin: '10 0 0 0',
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PaymentsInfo');

        Ext.apply(me, {
            store: store,
            columns: [
                    {
                        xtype: 'b4editcolumn'
                    },
                    {
                        dataIndex: 'Period',
                        text: 'Период',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        text: 'Дата оплаты',
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        dataIndex: 'PaymentDate',
                        flex: 1,
                        filter: { xtype: 'datefield' }
                    },
                    {
                        dataIndex: 'Reason',
                        text: 'Тип операций',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'Amount',
                        text: 'Сумма',
                        format: '0.00',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'b4enumcolumn',
                        enumName: 'B4.enums.TypeTransferSource',
                        text: 'Источник поступления',
                        dataIndex: 'Source',
                        filter: true,
                        flex: 1
                    },
                    {
                        dataIndex: 'DocumentNum',
                        text: 'Номер документа/реестра',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'DocumentDate',
                        text: 'Дата документа/реестра',
                        format: 'd.m.Y',
                        flex: 1,
                        filter: { xtype: 'datefield' },
                        renderer: function (value) {
                            if (!value || value.getDate || value.indexOf('0001-01-01T') > -1) {
                                return '';
                            }

                            return Ext.util.Format.date(value, 'd.m.Y');
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'OperationDate',
                        text: 'Дата операции',
                        format: 'd.m.Y',
                        flex: 1,
                        filter: { xtype: 'datefield' },
                        renderer: function (value) {
                            if (!value || value.getDate || value.indexOf('0001-01-01T') > -1) {
                                return '';
                            }

                            return Ext.util.Format.date(value, 'd.m.Y');
                        }
                    },
                    {
                        dataIndex: 'PaymentAgentName',
                        text: 'Наименование платежного агента',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        dataIndex: 'PaymentNumberUs',
                        text: 'Номер платежа в Системе ПА',
                        flex: 1,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        text: 'Идент. документа',
                        dataIndex: 'DocumentId',
                        hidden: true,
                        renderer: function (val) { return val ? val : ''; },
                        filter: {
                            xtype: 'numberfield',
                            allowDecimals: false,
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        dataIndex: 'UserLogin',
                        text: 'Пользователь',
                        flex: 1,
                        hideable: false,
                        filter: {
                            xtype: 'textfield'
                        }
                    },
                    {
                        xtype: 'actioncolumn',
                        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                        tooltip: 'Переход к документу',
                        text: 'Документ',
                        dataIndex: 'Redirect',
                        width: 100,
                        handler: function (view, rowIndex, colIndex, item, e, record) {
                            me.fireEvent('rowaction', me, 'redirect', record);
                        },
                        renderer: function () {
                            return 'Перейти ';
                        }
                    }
            ],
            dockedItems: [
                 {
                     xtype: 'toolbar',
                     dock: 'top',
                     items: [
                         {
                             xtype: 'b4updatebutton'
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
                loadMask: true,
                markDirty: false
            }
        });

        me.callParent(arguments);
    }
});