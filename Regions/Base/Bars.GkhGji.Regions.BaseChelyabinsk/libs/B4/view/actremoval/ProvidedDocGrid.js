Ext.define('B4.view.actremoval.ProvidedDocGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actRemovalProvidedDocGrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Предоставленные документы',
    store: 'actremoval.ProvidedDoc',
    itemId: 'actRemovalProvidedDocGrid',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProvidedDocGji',
                    flex: 1,
                    text: 'Предоставленный документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateProvided',
                    text: 'Дата предоставления',
                    format: 'd.m.Y',
                    width: 150,
                    editor: 'datefield'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
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
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'actProvidedDocGridAddButton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'actProvidedDocGridSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'actProvidedDocGridUpdateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
                                }
                            ]
                        }
                    ]
                        },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});