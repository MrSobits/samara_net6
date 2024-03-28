Ext.define('B4.view.dict.knmtypes.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.KnmTypes',
        'B4.grid.SelectFieldEditor',
        'B4.view.Control.GkhTriggerField'
    ],

    title: 'Виды КНМ',
    alias: 'widget.knmtypesgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.KnmTypes');
        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 1,
                    text: 'Наименование',
                    sortable: false,
                    renderer: function(val) {
                        return me.getRawValue(val);
                    },
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'tfKindCheck'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    flex: 1,
                    text: 'Идентификатор в ЕРВК',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36,
                        allowBlank: false
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
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function (editor, e) {
                            if (e.field == 'KindCheck') {
                                var trigField = editor.grid.down('gridcolumn[dataIndex=KindCheck]').getEditor();
                                trigField.updateDisplayedText(editor.grid.getRawValue(e.value));
                            }
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    layout: {
                        type: 'vbox'
                    },
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
                        },
                        {
                            xtype: 'textfield',
                            labelWidth: 200,
                            name: 'KnmTypeId',
                            fieldLabel: 'Идентификатор справочника в ЕРВК',
                            padding: '5 0 5 0',
                            readOnly: true,
                            width: 500
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
    getRawValue: function(val) {
        if(typeof val != "string") {
            return val.map(function(item) {
                return item['Name'];
            }).join(', ');
        }
    }
});