Ext.define('B4.view.dict.work.TypeFinSourceGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.worktypefinsourcegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Источники финансирования',
    store: 'dict.WorkTypeFinSource',
    itemId: 'worktypefinsourcegrid',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeFinSource',
                    flex: 1,
                    text: 'Наименование',
                    renderer: function(val) {
                        return B4.enums.TypeFinSource.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'combobox',
                        editable: false,
                        displayField: 'Display',
                        valueField: 'Value',
                        store: B4.enums.TypeFinSource.getStore()
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                //{
                                //    xtype: 'button',
                                //    itemId: 'workTypeFinSourceBtnSave',
                                //    iconCls: 'icon-accept',
                                //    text: 'Сохранить'
                                //},
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