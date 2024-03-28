Ext.define('B4.view.suggestion.citizensuggestion.CommentGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.citsugcommentgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.suggestion.Comment',
        'Ext.ux.CheckColumn'
    ],

    title: 'Вопросы',
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.suggestion.Comment');

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
                    dataIndex: 'CreationDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executor',
                    flex: 1,
                    sortable: false,
                    text: 'Исполнитель'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Question',
                    flex: 1,
                    text: 'Описание проблемы',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    flex: 1,
                    text: 'Ответ',
                    sortable: false
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'HasFiles',
                    sortable: false,
                    text: 'Приложенные файлы',
                    listeners: {
                        'beforecheckchange': function() {
                            return false;
                        }
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