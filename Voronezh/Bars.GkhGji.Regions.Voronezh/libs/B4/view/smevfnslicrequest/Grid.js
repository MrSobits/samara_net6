﻿Ext.define('B4.view.smevfnslicrequest.Grid', {
    extend: 'B4.ux.grid.Panel',    
    alias: 'widget.smevfnslicrequestgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar',
        'B4.enums.RequestState',
        'B4.enums.FNSLicRequestType'
    ],

    title: 'Реестр запросов в ФНС',
    store: 'smev.SMEVFNSLicRequest',
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
                    dataIndex: 'IdDoc',
                    flex: 1,
                    text: 'ID документа',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FNSLicRequestType',
                    dataIndex: 'FNSLicRequestType',
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
                   xtype: 'b4enumcolumn',
                   enumName: 'B4.enums.RequestState',
                   filter: true,
                   text: 'Состояние запроса',
                   dataIndex: 'RequestState',
                   flex: 3
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