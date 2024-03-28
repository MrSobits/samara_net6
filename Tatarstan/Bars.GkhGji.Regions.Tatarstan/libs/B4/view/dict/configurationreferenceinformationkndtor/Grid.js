Ext.define('B4.view.dict.ConfigurationReferenceInformationKndTor.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.enums.DictTypes'
    ],

    title: 'Конфигурация справочной информации ТОР КНД',
    alias: 'widget.configurationreferenceinformationkndtorgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ConfigurationReferenceInformationKndTor');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [                
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Type',
                    enumName: 'B4.enums.DictTypes',
                    flex: 1,
                    filter: true,
                    text: 'Тип справочника',
                    editor: {
                        xtype: 'b4combobox',
                        items: B4.enums.DictTypes.getItems(),
                        displayField: 'Display',
                        valueField: 'Value',
                        allowBlank: false,
                        editable: false
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Значение',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TorId',
                    text: 'Идентификатор в ТОР КНД',
                    flex: 1,
                    filter: { xtype: 'textfield' },
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
    }
});