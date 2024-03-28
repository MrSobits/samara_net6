Ext.define('B4.view.appealcits.BaseStatementGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.baseStatementAppCitsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeBase'
    ],

    title: 'Процессы по обращению',
    store: 'appealcits.BaseStatement',
    itemId: 'baseStatementAppCitsGrid',

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
                    dataIndex: 'TypeBase',
                    text: 'Тип процесса',
                    renderer: function (val) {
                        return B4.enums.TypeBase.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.TypeBase.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    width: 80,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Номер распоряжения'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата распоряжения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealtyObject',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Объект процесса по обращению'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: false
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