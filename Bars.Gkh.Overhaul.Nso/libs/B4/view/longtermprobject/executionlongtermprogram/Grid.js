Ext.define('B4.view.longtermprobject.executionlongtermprogram.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.form.field.ComboBox',
        'B4.store.longtermprobject.ExecutionLongTermProgram'
    ],

    title: 'Выполнение программы капитального ремонта',
    alias: 'widget.executionlongtermprogramgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.longtermprobject.ExecutionLongTermProgram');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramName',
                    flex: 1,
                    text: 'Программа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Work',
                    flex: 1,
                    text: 'Вид работы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Стоимомсть',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Perform',
                    text: 'Выполнено',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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