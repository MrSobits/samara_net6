Ext.define('B4.view.disposal.ProvidedDocGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.disposalprovideddocgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Предоставляемые документы',
    store: 'disposal.ProvidedDoc',
    itemId: 'disposalProvidedDocGrid',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProvidedDocGji',
                    flex: 1,
                    text: 'Предоставляемый документ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Подробнее',
                    editor: 'textfield',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'disposalProvidedDocSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'ProvideDocumentsNum',
                            itemId: 'ProvideDocumentsNum',
                            fieldLabel: 'Предоставить документы в течение',
                            labelWidth: 170
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