Ext.define('B4.view.administration.TransferIntegrationSessionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Интеграция с ЖКХ.Комплекс',
    alias: 'widget.transferintegrationsessiongrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.DataTransferIntegrationSession');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Guid',
                    width: 260,
                    text: 'GUID',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 150,
                    text: 'Время старта',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    width: 150,
                    text: 'Время окончания',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeIntegration',
                    text: 'Вид интеграции',
                    enumName: 'B4.enums.DataTransferOperationType',
                    width: 150,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TransferingState',
                    text: 'Состояние интеграции',
                    enumName: 'B4.enums.TransferingState',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Success',
                    text: 'Успешность ',
                    enumName: 'B4.enums.YesNo',
                    width: 80,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErrorCode',
                    width: 120,
                    text: 'Код ошибки',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErrorMessage',
                    flex: 1,
                    text: 'Сообщение об ошибке',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Запустить импорт',
                                    action: 'RunIntegration',
                                    iconCls: ''
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
            ]
        });

        me.callParent(arguments);
    }
});