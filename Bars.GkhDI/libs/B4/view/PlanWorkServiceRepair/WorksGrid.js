Ext.define('B4.view.planworkservicerepair.WorksGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.planworkservrepairworksgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.PeriodicityTemplateService',
        'B4.view.Control.GkhDecimalField'
    ],

    title: 'Работы по содержанию и ремонту',
    store: 'planworkservicerepair.Works',
    itemId: 'planWorkServiceRepairWorksGrid',
    closable: false,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkRepairListName',
                    flex: 2,
                    text: 'Наименование работы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodicityTemplateService',
                    flex: 2,
                    text: 'Периодичность'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateComplete',
                    format: 'd.m.Y',
                    flex: 2,
                    text: 'Срок выполнения'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    flex: 2,
                    text: 'Плановая стоимость (руб.)',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    flex: 2,
                    text: 'Дата начала'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    flex: 2,
                    text: 'Дата окончания'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactCost',
                    flex: 2,
                    text: 'Фактическая стоимость (руб.)',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DataComplete',
                    flex: 3,
                    text: 'Сведения о выполнении'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReasonRejection',
                    flex: 3,
                    text: 'Причина отклонения'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Актуализировать из сведений об услугах',
                                    tooltip: 'Актуализировать из сведений об услугах',
                                    iconCls: 'icon-accept',
                                    itemId: 'planWorkServiceRepairWorksReloadButton'
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