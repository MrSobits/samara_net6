Ext.define('B4.view.gisgmp.GISGMPPaymentsGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.gisgmppaymentsgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.SMEVFileType',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.column.Enum'
    ],

    store: 'smev.PaymentsListByGisGmpId',


    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [                 
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
                    xtype: 'datecolumn',
                    dataIndex: 'PaymentDate',
                    flex: 0.5,
                    text: 'Дата платежа',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    flex: 1,
                    text: 'Основание',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Amount',
                    flex: 1,
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentId',
                    flex: 1,
                    text: 'ИД платежа',
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
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл ответа',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                //{
                //    xtype: 'actioncolumn',
                //    text: 'Операция',
                //    action: 'reconsile',
                //    width: 150,
                //    items: [{
                //        tooltip: 'Сквитировать с принятым начислением',
                //        iconCls: 'icon-fill-button',
                //        icon: B4.Url.content('content/img/BtnReconcile.png')
                //    }]
                //}
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Квитировать все',
                                    tooltip: 'Квитировать все сопоставленные платежи',
                                    iconCls: 'icon-accept',
                                    width: 130,
                                    itemId: 'btnReconcileAll'
                                }
                            ]
                        },

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