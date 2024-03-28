Ext.define('B4.view.appealcits.AppealCitsResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealCitsResolutionGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet'
    ],

    title: 'Резолюции',
    store: 'appealcits.AppealCitsResolution',
    itemId: 'appealCitsResolutionGrid',

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
                    dataIndex: 'ResolutionText',
                    flex: 1,
                    text: 'Текст резолюции'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ResolutionTerm',
                    flex: 1,
                    text: 'Срок резолюции',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResolutionAuthor',
                    flex: 1,
                    text: 'Автор резолюции'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ResolutionDate',
                    flex: 1,
                    text: 'Дата резолюции',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Executed',
                    text: 'Отчет принят',
                    flex: 0.5,
                    filter: true,
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