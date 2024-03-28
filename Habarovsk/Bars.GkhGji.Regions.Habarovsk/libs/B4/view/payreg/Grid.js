Ext.define('B4.view.payreg.Grid', {
    extend: 'B4.ux.grid.Panel',    
    alias: 'widget.payreggrid',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.RequestState',
        'B4.enums.YesNoNotSet',
    ],

    title: 'Реестр платежей',
    store: 'smev.PayReg',
    closable: true,
    enableColumnHide: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'IsGisGmpConnected',
                    text: 'Есть начисление',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisGmpUIN',
                    flex: 1,
                    text: 'УИН</br>сопоставленного</b>начисления',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentId',
                    flex: 1,
                    text: 'Идентификатор</br>платежа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    flex: 1,
                    text: 'Назначение</br>платежа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Amount',
                    flex: 1,
                    text: 'Сумма платежа',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PaymentDate',
                    flex: 1,
                    text: 'Дата платежа',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SupplierBillID',
                    flex: 1,
                    text: 'УИН',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kbk',
                    flex: 1,
                    text: 'КБК',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OKTMO',
                    flex: 1,
                    text: 'ОКТМО',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayerName',
                    flex: 1,
                    text: 'Наименование</br>плательщика',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AccDocDate',
                    flex: 1,
                    text: 'Дата</br>платёжного</br>документа ',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccDocNo',
                    flex: 1,
                    text: 'Номер</br>платёжного</br>документа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentOrg',
                    flex: 1,
                    text: 'Оплата через',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentOrgDescr',
                    flex: 1,
                    text: 'БИК /</br>код ТОФК /</br>УРН',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayerId',
                    flex: 1,
                    text: 'Идентификатор</br>плательщика',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BdiStatus',
                    flex: 1,
                    text: 'Статус</br>плательщика</br>(поле 101)',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BdiPaytReason',
                    flex: 1,
                    text: 'Основание</br>платежа</br>(поле 106)',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BdiTaxPeriod',
                    flex: 1,
                    text: 'Период</br>платежа</br>(поле 107)',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BdiTaxDocNumber',
                    flex: 1,
                    text: 'Номер</br>документа</br>(поле 108)',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BdiTaxDocDate',
                    flex: 1,
                    text: 'Дата</br>документа</br>(поле 109)',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Status',
                    flex: 1,
                    text: 'Статус</br>платежа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
               
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Reconcile',
                    text: 'Сквитирована',
                    flex: 0.5,
                    filter: true,
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: true,
                    showClearAllButton: true,
                    pluginId: 'headerFilter'
                },
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Запросить оплаты',
                                    tooltip: 'Задать параметры запроса',
                                    iconCls: 'icon-accept',
                                    width: 150,
                                    itemId: 'btnGetPayments'
                                },
                                {
                                    xtype: 'button',
                                    text: 'История запросов',
                                    tooltip: 'Посмотреть историю запросов',
                                    iconCls: 'icon-accept',
                                    width: 150,
                                    itemId: 'btnGetPaymentsHistory'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});