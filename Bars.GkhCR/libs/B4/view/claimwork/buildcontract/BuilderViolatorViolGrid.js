Ext.define('B4.view.claimwork.buildcontract.BuilderViolatorViolGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.claimwork.BuilderViolatorViol'
    ],

    title: 'Нарушения условий договора',
    cls: 'x-large-head',
    alias: 'widget.builderviolatorviolgrid',

    closable: false,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.claimwork.BuilderViolatorViol');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violation',
                    flex: 1,
                    text: 'Нарушение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Note',
                    flex: 1,
                    text: 'Примечание',
                    filter: { xtype: 'textfield' },
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'builderViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'builderViolatorViolAddButton'
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
            }

        });

        me.callParent(arguments);
    }
});

