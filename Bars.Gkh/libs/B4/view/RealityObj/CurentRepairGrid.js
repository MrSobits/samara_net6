Ext.define('B4.view.realityobj.CurentRepairGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjcurentrepairgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Текущий ремонт',
    store: 'realityobj.CurentRepair',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkKindName',
                    flex: 3,
                    text: 'Вид работы'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PlanDate',
                    text: 'Плановая дата',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanSum',
                    flex: 1,
                    text: 'План на сумму', 
                    editor:{
                        xtype: 'numberfield',
                        allowNegative: false,
                        hideTrigger: true,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanWork',
                    flex: 1,
                    text: 'План объем работ',
                    editor:{
                        xtype: 'numberfield',
                        allowNegative: false,
                        hideTrigger: true,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        minValue: 0
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FactDate',
                    text: 'Факт дата',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactSum',
                    flex: 1,
                    text: 'Факт на сумму',
                    editor:{
                        xtype: 'numberfield',
                        allowNegative: false,
                        hideTrigger: true,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactWork',
                    flex: 1,
                    text: 'Факт объем работ',
                    allowDecimals: true,
                    editor:{
                        xtype: 'numberfield',
                        allowNegative: false,
                        hideTrigger: true,
                        allowDecimals: true,
                        decimalSeparator: ',',
                        minValue: 0
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед. измерения',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                { xtype: 'b4savebutton' }
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