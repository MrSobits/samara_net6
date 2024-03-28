Ext.define('B4.view.prescription.CancelEditViolCancelTab', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],
    alias: 'widget.canceleditviolcanceltab',
    title: 'Отмененные нарушения',
    itemId: 'canceleditviolcanceltab',
    viewConfig: {
        autoFill: true
    },

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.prescription.PrescriptionCancelViol');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiPin',
                    text: 'Пункт НПД',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    text: 'Наименование нарушения',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    name: 'NewDatePlanRemoval',
                    dataIndex: 'NewDatePlanRemoval',
                    text: 'Новый срок устранения',
                    flex: 1,
                    format: 'd.m.Y',
                    editor: 'datefield'
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
                                    itemId: 'prescriptionCancelViolRefSaveButton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});