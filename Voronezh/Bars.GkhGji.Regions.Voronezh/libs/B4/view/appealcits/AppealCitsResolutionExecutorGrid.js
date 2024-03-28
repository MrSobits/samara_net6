Ext.define('B4.view.appealcits.AppealCitsResolutionExecutorGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealCitsResolutionExecutorGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Исполнители',
    store: 'appealcits.AppealCitsResolutionExecutor',
    itemId: 'appealCitsResolutionExecutorGrid',

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
                    dataIndex: 'Surname',
                    flex: 1,
                    text: 'Фамилия'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Имя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Patronymic',
                    flex: 1,
                    text: 'Отчество'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PersonalTerm',
                    flex: 1,
                    text: 'Персональный срок',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Comment',
                    flex: 1,
                    text: 'Комментарий'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    dataIndex: 'IsResponsible',
                    flex: 1,
                    text: 'Ответственный'
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