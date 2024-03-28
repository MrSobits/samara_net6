Ext.define('B4.view.dict.knmaction.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.KnmAction',
        'B4.view.Control.GkhTriggerField',
        'B4.enums.ActCheckActionType',
        'B4.enums.KindAction'
    ],

    title: 'Действия в рамках КНМ',
    alias: 'widget.knmactiongrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.KnmAction');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActCheckActionType',
                    enumName: 'B4.enums.ActCheckActionType',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'b4combobox',
                        items: B4.enums.ActCheckActionType.getItems(),
                        displayField: 'Display',
                        valueField: 'Value',
                        allowBlank: false,
                        editable: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlType',
                    flex: 1,
                    text: 'Вид контроля',
                    sortable: false,
                    renderer: (val) => this.getRawValue(val),
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'trigfControlType'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KnmType',
                    flex: 1,
                    text: 'Вид КНМ',
                    sortable: false,
                    renderer: (val) => this.getRawValue(val),
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'trigfKnmType'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    flex: 1,
                    text: 'Идентификатор в ЕРВК',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindAction',
                    flex: 1,
                    text: 'Вид мероприятия',
                    sortable: false,
                    renderer: (val) => val ? val.map(x => B4.enums.KindAction.displayRenderer(x)).join(', ') : '',
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'trigfKindAction'
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
        else {
            return val;
        }
    }
});