Ext.define('B4.view.service.repair.WorkRepairDetailGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.workrepdetailgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.store.dict.UnitMeasureNoPaging',
        'B4.view.Control.GkhDecimalField'
    ],
    
    store: 'service.WorkRepairDetail',
    itemId: 'workRepairDetailGrid',
    title: 'Детализация',
    closable: false,

    initComponent: function() {
        var me = this,
            storeUnitMeasure = Ext.create('B4.store.dict.UnitMeasureNoPaging', {
                remoteFilter: false
            });

        storeUnitMeasure.load();

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlannedVolume',
                    flex: 1,
                    text: 'Запл. объем',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactVolume',
                    flex: 1,
                    text: 'Факт. объем',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    },
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Единица измерения',
                    editor: {
                        xtype: 'b4combobox',
                        displayField: 'Name',
                        valueField: 'Id',
                        store: storeUnitMeasure,
                        editable: false
                    },
                    renderer: function (val) {
                        if (val) {
                            var um = storeUnitMeasure.findRecord('Id', val, 0, false, false, true);
                            if (um) {
                                return um.get('Name');
                            }
                        }
                        return '';
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
                                    xtype: 'b4addbutton',
                                    itemId: 'workRepairDetailAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnSave',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
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