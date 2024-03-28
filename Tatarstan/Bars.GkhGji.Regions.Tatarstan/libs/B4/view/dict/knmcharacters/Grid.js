Ext.define('B4.view.dict.knmcharacters.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.KnmCharacters',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Характеры КНМ',
    alias: 'widget.knmcharactersgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.KnmCharacters');

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
                    renderer: function (val) {
                        return me.getRawValue(val);
                    },
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'trigfKindChecks'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErknmCode',
                    flex: 1,
                    text: 'Код в ЕРКНМ',
                    editor: {
                        xtype: 'numberfield'
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
                        },
                        afteredit: function (editor, e) {
                            var fieldName = 'KindCheck';
                            if (e.field == fieldName) {
                                var rows = editor.grid.getStore().getRange(),
                                    code = editor.context.record.getData().ErknmCode,
                                    row = rows.find(x => x.data.ErknmCode == code);
                                row.set(fieldName, e.originalValue);
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
    },

    getRawValue: function (val) {
        if (val && typeof val != "string") {
            return val.map(function (item) {
                return item['Name'];
            }).join(', ');
        }
    }
});