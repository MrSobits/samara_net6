Ext.define('B4.view.giserp.Grid', {
    extend: 'B4.ux.grid.Panel',    
    alias: 'widget.giserpgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.KindKND',
        'Ext.ux.grid.FilterBar',
        'B4.enums.RequestState',
        'B4.enums.GisErpRequestType'
    ],

    title: 'Обмен данными с ГИС ЕРП',
    store: 'smev.GISERP',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'ERPID',
                    flex: 0.5,
                    text: 'Учетный номер проверки',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.GisErpRequestType',
                    dataIndex: 'GisErpRequestType',
                    text: 'Тип запроса ЕРП',
                    flex: 1, 
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.KindKND',
                    dataIndex: 'KindKND',
                    text: 'Вид контроля',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RequestDate',
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
                    dataIndex: 'Disposal',
                    flex: 1,
                    text: 'Приказ',
                    filter: {
                        xtype: 'textfield',
                    },
                },               
               {
                   xtype: 'b4enumcolumn',
                   enumName: 'B4.enums.RequestState',
                   filter: true,
                   text: 'Состояние запроса',
                   dataIndex: 'RequestState',
                   flex: 1
               },             
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    flex: 1,
                    text: 'Ответ СМЭВ',
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
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 60,
                                    fieldLabel: 'Период с',
                                    labelAlign: 'right',
                                    width: 160,
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    labelAlign: 'right',
                                    fieldLabel: 'по',
                                    width: 130,
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
                                },
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