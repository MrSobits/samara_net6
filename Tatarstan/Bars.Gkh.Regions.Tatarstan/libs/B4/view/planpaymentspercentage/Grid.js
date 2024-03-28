Ext.define('B4.view.planpaymentspercentage.Grid',
{
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.planpaymentspercentagegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.Combobox'
    ],

    title: 'Справочник процентов оплат',
    store: 'PlanPaymentsPercentage',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Ресурсоснабжающая организации',
                    dataIndex: 'PublicServiceOrg',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Услуга',
                    dataIndex: 'Service',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Ресурс',
                    dataIndex: 'Resource',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Процент оплаты',
                    dataIndex: 'Percentage',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        decimalSeparator: ','
                    },
                    renderer: function(val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата начала действия',
                    dataIndex: 'DateStart',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата окончания действия',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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