Ext.define('B4.view.dict.statsubsubjectgji.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Подтематики',
    store: 'dict.StatSubsubjectGji',
    alias: 'widget.statSubsubjectGjiGrid',
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
                    dataIndex: 'Code',
                    width: 100,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'SSTUNameSub',
                     flex: 1,
                     text: 'Наименование в ССТУ'
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SSTUCodeSub',
                    flex: 1,
                    text: 'Код в ССТУ'
                 },
                {
                    xtype: 'booleancolumn',
                    falseText: '',
                    trueText: 'Да',
                    dataIndex: 'ISSOPR',
                    flex: 0.5,
                    text: 'СОПР',
                    filter:
                    {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'booleancolumn',
                    falseText: 'Нет',
                    trueText: 'Да',
                    dataIndex: 'TrackAppealCits',
                    flex: 0.5,
                    text: 'Отслеживать обращение',
                    filter:
                    {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
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