Ext.define('B4.view.service.repair.WorkRepairListGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.workreplistgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.YesNoNotSet'
    ],
    store: 'service.WorkRepairList',
    itemId: 'workRepairListGrid',
    title: 'Список работ',
    closable: false,

    initComponent: function() {
        var me = this;

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
                    dataIndex: 'PlannedCost',
                    flex: 1,
                    text: 'Запл. стоимость (руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactCost',
                    flex: 1,
                    text: 'Факт. стоимость (руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Дата начала',
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    text: 'Дата окончания',
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InfoAboutExec',
                    flex: 1,
                    text: 'Cведения о выполнении',
                    editor: 'textfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReasonRejection',
                    flex: 1,
                    text: 'Причина отклонения',
                    editor: 'textfield'
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
                })],
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'workRepairListSaveButton'
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    fieldLabel: 'Наличие планово-предупредительных работ',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    value: B4.enums.YesNoNotSet.getStore().last().get('Value'),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'ScheduledPreventiveMaintanance',
                                    labelWidth: 143
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