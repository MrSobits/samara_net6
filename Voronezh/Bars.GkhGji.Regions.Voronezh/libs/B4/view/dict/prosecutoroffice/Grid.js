Ext.define('B4.view.dict.prosecutoroffice.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.store.dict.Municipality',
        'B4.form.SelectField',
        'B4.grid.SelectFieldEditor',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Отделы прокуратуры',
    store: 'dict.ProsecutorOffice',
    alias: 'widget.prosecutorofficegrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FederalCentersCode',
                    flex: 1,
                    text: 'Код региона',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    renderer: function (val) {
                        if (val && val.Name) {
                            return val.Name;
                        }
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.Municipality',
                        name: 'Municipality',
                        modalWindow: true,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Наименование',
                                flex: 1,
                                dataIndex: 'Name',
                                filter: { xtype: 'textfield' }
                            }
                           
                        ]
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Municipality.Name'
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
                                    xtype: 'button',
                                    text: 'Запросить из СМЭВ',
                                    tooltip: 'Запросить список отделов прокуратуры по коду региона',
                                    iconCls: 'icon-accept',
                                    width: 150,
                                    itemId: 'btnGetProsecutorOffice'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});