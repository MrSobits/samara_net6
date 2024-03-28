Ext.define('B4.view.GisGmpPatternDictGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.grid.plugin.CellEditing',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.GisGmpPatternDict'
    ],

    alias: 'widget.gisgmppatterndictgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GisGmpPatternDict');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PatternName',
                    flex: 1,
                    text: 'Наименование шаблона',
                    filter: {
                        xtype: 'textfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PatternCode',
                    flex: 1,
                    text: 'Код шаблона',
                    filter: {
                        xtype: 'numberfield'
                    },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 255,
                        maskRe: /[0-9]/
                    }
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Relevance',
                    width: 200,
                    text: 'Актуальность'
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
                            columns: 3,
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