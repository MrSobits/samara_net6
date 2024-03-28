Ext.define('B4.view.dict.paymentsizecr.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.paymentsizecrpanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.form.ComboBox',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeIndicator'
    ],

    title: 'Размер взноса на КР',
    store: 'dict.PaymentSizeCr',
    enableColumnHide: true,
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
                    dataIndex: 'TypeIndicator',
                    flex: 1,
                    text: 'Показатель',
                    renderer: function(val) {
                         return B4.enums.TypeIndicator.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeIndicator.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
                    flex: 1,
                    text: 'Муниципальное образование',
                    renderer: function(val) {
                        return val;
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentSize',
                    width: 130,
                    text: 'Размер взноса (руб.)',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStartPeriod',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Действует с',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEndPeriod',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Действует по',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityCount',
                    hidden: true,
                    flex: 1,
                    text: 'Количество муниципальных образований',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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