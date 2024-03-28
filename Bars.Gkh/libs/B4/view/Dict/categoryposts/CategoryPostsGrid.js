﻿Ext.define('B4.view.dict.categoryposts.CategoryPostsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.categorypostsgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.store.dict.CategoryPosts'
    ],

    title: 'Категории сообщений',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.CategoryPosts');

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1,
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    flex: 1,
                    width: 100,
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 2,
                    pluginId: 'cellEditing'
                })
            ],
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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