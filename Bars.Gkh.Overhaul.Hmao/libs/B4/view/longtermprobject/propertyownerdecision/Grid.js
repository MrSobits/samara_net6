Ext.define('B4.view.longtermprobject.propertyownerdecision.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MethodFormFundCr'
    ],

    title: 'Повестка ОСС',
    store: 'BasePropertyOwnerDecision',
    itemId: 'propertyownerdecisionGrid',
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
                    dataIndex: 'Decision',
                    flex: 1,
                    text: 'Повестка',
                    renderer: function (val) {
                        return B4.enums.PropertyOwnerDecisionType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.PropertyOwnerDecisionType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MethodFormFund',
                    flex: 1,
                    text: 'Способ формирования фонда',
                    renderer: function (val) {
                        return B4.enums.MethodFormFundCr.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.MethodFormFundCr.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolNumber',
                    flex: 1,
                    text: 'Номер протокола собственников',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProtocolDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата принятия протокола',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
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
                            columns: 3,
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