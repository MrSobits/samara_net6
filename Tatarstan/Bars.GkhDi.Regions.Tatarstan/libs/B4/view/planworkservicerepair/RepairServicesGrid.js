Ext.define('B4.view.planworkservicerepair.RepairServicesGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.planworkservrepairrepairservicesgrid',
    requires: [
        'B4.ux.button.Update',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.PeriodicityTemplateService',
        'B4.store.service.WorkRepairTechService'
    ],

    title: 'ТО',
    store: 'service.WorkRepairTechService',
    itemId: 'planWorkServiceRepairServicesGrid',
    closable: false,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GroupName',
                    flex: 1,
                    text: 'Наименование'
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    layout: 'fit',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '5',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumWorkTo',
                                            itemId: 'sumWorkTo',
                                            fieldLabel: 'Планова сумма (руб.)',
                                            editable: false,
                                            readOnly: true,
                                            width: 270,
                                            labelWidth: 150
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'SumFact',
                                            itemId: 'sumFact',
                                            fieldLabel: 'Фактическая сумма (руб.)',
                                            editable: false,
                                            readOnly: true,
                                            width: 270,
                                            labelWidth: 150
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStart',
                                            fieldLabel: 'Дата начала',
                                            width: 200,
                                            editable: false,
                                            readOnly: true,
                                            hideTrigger: true,
                                            labelWidth: 100
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEnd',
                                            fieldLabel: 'Дата окончания',
                                            editable: false,
                                            readOnly: true,
                                            hideTrigger: true,
                                            width: 200,
                                            labelWidth: 100
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ProgressInfo',
                                            fieldLabel: 'Сведения о выполнении',
                                            readOnly: true,
                                            editable: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RejectCause',
                                            fieldLabel: 'Причина отклонения',
                                            readOnly: true,
                                            editable: false
                                        }
                                    ]
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