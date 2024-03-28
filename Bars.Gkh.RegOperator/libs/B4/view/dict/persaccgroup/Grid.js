Ext.define('B4.view.dict.persaccgroup.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.persaccgroupGrid',
    title: 'Группы лицевых счетов',
    store: 'dict.PersAccGroup',

    closable: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNo'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield'},
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsSystem',
                    width: 100,
                    text: 'Системная',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNo.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function(val) {
                        return B4.enums.YesNoNotSet.displayRenderer(val);
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    //убираем стандартный обработчик событий
                    handler: null,
                    renderer: function (val, btn, rec) {
                        var deleteCol = this.columns[2];
                        if (rec.get('IsSystem') == B4.enums.YesNo.Yes) {
                            deleteCol.disable();
                            deleteCol.disableAction();
                        } else {
                            deleteCol.enable();
                            deleteCol.enableAction();
                        }
                        
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function (editor, e) {
                            return e.record.get('IsSystem') !== B4.enums.YesNo.Yes;
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});