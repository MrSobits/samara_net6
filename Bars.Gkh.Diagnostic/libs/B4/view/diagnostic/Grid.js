Ext.define('B4.view.diagnostic.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.diagnostic.CollectedDiagnosticResult',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNo',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.enums.CollectedDiagnosticResultState'
    ],

    title: 'Диагностика',
    alias: 'widget.diagnosticgrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.diagnostic.CollectedDiagnosticResult');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    text: 'Дата диагностики',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    flex: 1,
                    text: 'Состояние',
                    renderer: function(val) {
                        return B4.enums.CollectedDiagnosticResultState.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.CollectedDiagnosticResultState.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'

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
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: store,
                        dock: 'bottom'
                    },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Действия',
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function () {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Диагностика',
                                    action: 'runDiagnostic',
                                    iconCls: 'icon-accept'
                                    
                                }
                            ]
                        }
                    ]
                }
                ]
            })
    ;

        me.callParent(arguments);
    }
});