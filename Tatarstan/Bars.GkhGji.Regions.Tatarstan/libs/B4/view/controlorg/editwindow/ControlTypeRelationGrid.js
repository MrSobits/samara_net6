Ext.define('B4.view.controlorg.editwindow.ControlTypeRelationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.controltyperelationgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.ControlType'
    ],

    title: 'Виды контроля',
    store: 'controlorg.ControlOrganizationControlTypeRelation',

    initComponent: function () {
        var me = this,
            controlTypeStore = Ext.create('B4.store.dict.ControlType');

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlType',
                    text: 'Наименование вида контроля',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.Name) {
                            return val.Name;
                        }

                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: controlTypeStore,
                        modalWindow: true,
                        textProperty: 'Name',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Наименование вида контроля',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                    filter: { xtype: 'textfield' }
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
                }
            ]
        });

        me.callParent(arguments);
    }
});