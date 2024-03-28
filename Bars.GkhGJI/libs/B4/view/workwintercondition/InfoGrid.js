Ext.define('B4.view.workwintercondition.InfoGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.Url',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    border: false,
    cls: 'x-large-head',
    alias: 'widget.workWinterConditionInfoGrid',

    store: 'workwintercondition.Information',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RowNumber',
                    text: '№ строки',
                    filter: true,
                    width: 45
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Показатели',
                    filter: true,
                    width: 450
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Measure',
                    text: 'Единица измерения',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Okei',
                    text: 'Код по ОКЕИ',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Total',
                    text: 'Всего',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PreparationTask',
                    text: 'Задание по подготовке',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PreparedForWork',
                    text: 'Подготовлено для работы в зимних условиях на отчетный период',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinishedWorks',
                    text: 'Выполнено работ по капитальному ремонту, реконструкции, замене',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    text: '% выполнения задания'
                }
            ],
            plugins: [
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