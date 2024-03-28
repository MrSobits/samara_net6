Ext.define('B4.view.dict.inspectionbasetype.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.InspectionKind'
    ],

    title: 'Основание проверки',
    alias: 'widget.inspectionbasetypegrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.InspectionBaseType');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 20
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionKindId',
                    renderer: function (val) {
                        if(val == 0)
                            val = 1;
                        return B4.enums.InspectionKind.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        editable: false,
                        items: B4.enums.InspectionKind.getItems(),
                        displayField: 'Display',
                        valueField: 'Value'
                    },
                    flex: 1,
                    text: 'Вид проверки'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SendErp',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    },
                    editor: {
                        xtype: 'b4combobox',
                        editable: false,
                        items: [[false, 'Нет'], [true, 'Да']],
                        displayField: 'Display',
                        valueField: 'Value'
                    },
                    flex: 1,
                    text: 'Значение передается в ЕРП'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TorId',
                    flex: 1,
                    text: 'Идентификатор в ТОР КНД',
                    editor: {
                        xtype: 'textfield',
                        regex: new RegExp('^[0-9A-Fa-f]{8}-([0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}$'),
                        regexText: 'Введен некорректный GUID'
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
                            xtype: 'checkbox',
                            name: 'OpenValuesForErknm',
                            boxLabel: 'Открыть значения для ЕРКНМ'
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

        me.initialConfig.columns = me.columns;
        me.callParent(arguments);
    }
});