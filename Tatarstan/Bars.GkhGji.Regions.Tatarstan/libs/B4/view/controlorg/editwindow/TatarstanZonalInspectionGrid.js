Ext.define('B4.view.controlorg.editwindow.TatarstanZonalInspectionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.tatarstanzonalinspectiongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.SelectFieldEditor',
        'B4.store.controlorg.TatarstanZonalInspection'
    ],

    title: 'Зональные инспекции',
    store: 'controlorg.TatarstanZonalInspection',

    initComponent: function () {
        var me = this,
            inspectionStore = Ext.create('B4.store.controlorg.TatarstanZonalInspection');
        inspectionStore.on('beforeload', me.onBeforeLoad, me);

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ZoneName',
                    text: 'Наименование зональной инспекции',
                    flex: 1,
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: inspectionStore,
                        modalWindow: true,
                        textProperty: 'ZoneName',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Наименование зональной инспекции',
                                dataIndex: 'ZoneName',
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
    },

    onBeforeLoad: function (store, operation) {
        //параметр необходим для получения инспекций, которые не привязаны к КНО.
        operation.params.isKnoWindow = true;
    }
});