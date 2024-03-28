Ext.define('B4.view.heatinputperiod.HeatInputInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    border: false,
    alias: 'widget.heatInputInfoGrid',
    

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.heatinputperiod.Information');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeHeatInputObject',
                    filter: true,
                    dataIndex: 'TypeHeatInputObject',
                    text: 'Объект',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Count',
                    flex: 1,
                    text: 'Всего',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CentralHeating',
                    width: 200,
                    text: 'Отапливаемые с центр. отоплением',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndividualHeating',
                    width: 250,
                    text: 'Отапливаемые с индивидуальным отоплением',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10,
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    flex: 1,
                    text: '%'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NoHeating',
                    flex: 1,
                    text: 'Без тепла'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function (e, editor) {
                            if (editor.rowIdx == 0)
                                return false;
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});