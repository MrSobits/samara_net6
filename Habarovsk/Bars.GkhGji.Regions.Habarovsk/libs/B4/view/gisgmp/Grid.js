Ext.define('B4.view.gisgmp.Grid', {
    extend: 'B4.ux.grid.Panel',    
    alias: 'widget.gisgmpgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar',
        'B4.enums.RequestState',
        'B4.enums.GisGmpChargeType'
    ],

    title: 'Обмен данными с ГИС ГМП',
    store: 'smev.GisGmp',
    closable: true,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',                    
                    text: 'Номер запроса',
                    flex: 0.5,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MessageId',
                    flex: 1,
                    text: 'Номер в СМЭВ',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.GisGmpChargeType',
                    dataIndex: 'GisGmpChargeType',
                    text: 'Тип запроса',
                    flex: 1, 
                    filter: true,
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalcDate',
                    flex: 0.5,
                    text: 'Дата запроса',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'textfield',
                    },
                },
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'Inspector',
                   flex: 1,
                   text: 'Инспектор',
                   filter: {
                       xtype: 'textfield',
                   },
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BillFor',
                    flex: 1,
                    text: 'Назначение платежа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalAmount',
                    flex: 1,
                    text: 'Сумма начисления',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentsAmount',
                    flex: 1,
                    text: 'Оплачено',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                },
               {
                   xtype: 'b4enumcolumn',
                   enumName: 'B4.enums.RequestState',
                   filter: true,
                   text: 'Состояние запроса',
                   dataIndex: 'RequestState',
                   flex: 3
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AltPayerIdentifier',
                    flex: 1,
                    text: 'Идентификатор плательщика',
                    filter: {
                        xtype: 'textfield',
                    },
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UIN',
                    flex: 1,
                    text: 'УИД',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                }
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, me) {
                    amm = record.get('TotalAmount');
                    pay = record.get('PaymentsAmount');
                    
                    if (amm - pay<=0)
                    {
                        return 'back-coralgreen';
                    }
                    return '';
                }
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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