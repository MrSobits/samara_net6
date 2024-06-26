﻿Ext.define('B4.view.tasks.TaskEntryGrid', {
    extend: 'B4.view.TaskEntryGrid',
    alias: 'widget.taskentrygrid',

    requires: [
        'B4.store.tasks.TaskEntry',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.TaskStatus',
        'Ext.ux.grid.FilterBar'
    ],

    storeName: 'B4.store.tasks.TaskEntry',

    columns: [
        {
            xtype: 'actioncolumn',
            width: 20,
            tooltip: 'Переход к результатам',
            icon: B4.Url.content('content/img/icons/arrow_right.png'),
            handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                var scope = this.origScope;
                if (!scope)
                    scope = this.up('grid');

                scope.fireEvent('rowaction', scope, 'gotoresult', rec);
            },
            sortable: false
        },
        {
            header: 'Id',
            dataIndex: 'Id',
            width: 70,
            filter: {
                xtype: 'numberfield',
                hideTrigger: true,
                operand: CondExpr.operands.eq
            },
            sortable: false
        },
        {
            header: 'Родитель',
            dataIndex: 'ParentId',
            width: 70,
            filter: {
                xtype: 'numberfield',
                hideTrigger: true,
                operand: CondExpr.operands.eq
            },
            sortable: false
        },
        {
            header: 'Дата запуска',
            xtype: 'datecolumn',
            format: 'd.m.Y H:i:s',
            dataIndex: 'CreateDate',
            width: 200,
            filter: {
                xtype: 'datefield',
                operand: CondExpr.operands.eq,
                format: 'd.m.Y H:i:s'
            },
            sortable: false
        },
        {
            header: 'Логин',
            dataIndex: 'UserLogin',
            flex: 3,
            filter: { xtype: 'textfield' },
            sortable: false
        },
        {
            header: 'Наименование',
            dataIndex: 'Name',
            flex: 3,
            filter: { xtype: 'textfield' },
            sortable: false
        },
        {
            xtype: 'b4enumcolumn',
            enumName: 'B4.enums.TaskStatus',
            filter: true,
            header: 'Статус',
            dataIndex: 'Status',
            flex: 3,
            sortable: false
        },
        {
            header: 'Описание',
            dataIndex: 'Description',
            flex: 6,
            filter: { xtype: 'textfield' },
            sortable: false
        },
        {
            header: 'Процент выполнения',
            dataIndex: 'Percentage',
            flex: 3,
            filter: {
                xtype: 'numberfield',
                operand: CondExpr.operands.eq,
                minValue: 0,
                maxValue: 100
            },
            sortable: false
        },
        {
            header: 'Ход выполнения',
            dataIndex: 'Progress',
            flex: 6,
            filter: { xtype: 'textfield' }
        },
        {
            header: 'Затраченное время',
            dataIndex: 'Duration',
            flex: 3,
            sortable: false
        }
    ]
});