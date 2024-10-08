﻿Ext.define('B4.view.dict.contragentrole.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.store.dict.ContragentRole'
    ],

    title: 'Роли контрагента',
    alias: 'widget.contragentrolegrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.ContragentRole');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 0.5,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Полное наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 400
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ShortName',
                    flex: 1,
                    text: 'Краткое наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 400
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
                    clicksToEdit: 1,
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }, 
                                {
                                    xtype: 'b4updatebutton'
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