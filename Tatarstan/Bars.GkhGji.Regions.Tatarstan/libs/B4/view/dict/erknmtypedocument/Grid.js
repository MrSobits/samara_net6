Ext.define('B4.view.dict.erknmtypedocument.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.ErknmTypeDocument'
    ],

    title: 'Тип документов ЕРКМН',
    alias: 'widget.erknmtypedocumentgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ErknmTypeDocument');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentType',
                    flex: 2,
                    text: 'Тип документа',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 2,
                    text: 'Код в ЕРКНМ',
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false,
                        maxLength: 36
                    },
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsBasisKnm',
                    flex: 2,
                    text: 'Используется в "Основании проведения КНМ"',
                    trueText: 'Да',
                    falseText: 'Нет',
                    editor: {
                        xtype: 'checkbox',
                        allowBlank: false
                    },
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 3,
                    text: 'Используется для видов проверок',
                    renderer: (val) => this.getRawValue(val),
                    editor: {
                        xtype: 'gkhtriggerfield',
                        editable: false,
                        itemId: 'trigfKindCheck',
                        allowBlank: false
                    },
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
                    dock: 'bottom',
                    displayInfo: true,
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    },

    getRawValue: function (val) {
        if (val && typeof val != "string") {
            return val.map(function (item) {
                if (item) {
                    if (item['Name']) {
                        return item['Name'];
                    }
                    else {
                        return item.KindCheck.map(x => x.Name).join(', ');
                    }
                }
                else {
                    return null;
                }
            }).join(', ');
        }
    }
});