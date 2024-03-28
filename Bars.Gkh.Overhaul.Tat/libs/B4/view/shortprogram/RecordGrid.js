Ext.define('B4.view.shortprogram.RecordGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.shortprogramrecordgrid',

    requires: [
        'B4.grid.feature.Summary',
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.shortprogram.Record',
        'B4.enums.TypeWork',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Работы',
    closable: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.shortprogram.Record');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWork',
                    text: 'Вид работы',
                    flex: 1,
                    renderer: function(val) {
                        return B4.enums.TypeWork.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeWork.getItemsWithEmpty([null, '-']),
                        valueField: 'Value',
                        displayField: 'Display',
                        editable: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkName',
                    text: 'Наименование',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    text: 'Объем',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    text: 'Сумма (руб.)',
                    flex: 1,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
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
            features: [{
                ftype: 'b4_summary'
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});