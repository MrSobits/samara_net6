Ext.define('B4.view.entitylog.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
       
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeEntityLogging',
        'B4.enums.AppealOperationType'
    ],

    title: 'Лог операций по работе с электронными документами',
    store: 'entitylog.EntityChangeLogRecord',
    alias: 'widget.entitychangelogrecordgrid',
    closable: true,
    enableColumnHide: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [      
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'AuditDate',
                     flex: 0.5,
                     text: 'Дата операции',
                     filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                     format: 'd.m.Y H:i'
                 },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.AppealOperationType',
                    dataIndex: 'OperationType',
                    text: 'Тип операции',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeEntityLogging',
                    dataIndex: 'TypeEntityLogging',
                    text: 'Тип сущности',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentValue',
                    flex: 1,
                    text: 'Значение основного параметра',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PropertyType',
                    flex: 0.5,
                    text: 'Код свойства',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PropertyName',
                    flex: 1,
                    text: 'Наименование свойства',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldValue',
                    flex: 1,
                    text: 'Старое значение',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewValue',
                    flex: 1,
                    text: 'Новое значение',
                    filter: {
                        xtype: 'textfield'
                    }
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OperatorLogin',
                    flex: 0.5,
                    text: 'Логин',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OperatorName',
                    flex: 1,
                    text: 'Имя пользователя',
                    filter: {
                        xtype: 'textfield'
                    }
                }  
               
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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