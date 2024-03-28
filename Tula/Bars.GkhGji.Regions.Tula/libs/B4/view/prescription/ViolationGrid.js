Ext.define('B4.view.prescription.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.prescriptionViolationGrid',
    store: 'prescription.Violation',
    itemId: 'prescriptionViolationGrid',
    title: 'Нарушения',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormDocNum',
                    text: 'Пункт нормативного дкоумента',
                    width: 100,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    flex: 1.3,
                    text: 'Текст нарушения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                    /*, В САХЕ нередактируется этот грид вся работа идет через грид Описания
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                    */
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    width: 130,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
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
                                /* В САХЕ у грида нарешний недолжно быт ькнопки Созхранить потомучто все данные сохраняются не через инайн грид а через другой грил Описания
                                {
                                    xtype: 'button',
                                    itemId: 'prescriptionViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },*/
                                {
                                    xtype: 'button',
                                    itemId: 'updateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'pagingtoolbar',
                    store: 'prescription.Violation',
                    dock: 'bottom',
                    displayInfo: true,
                    displayMsg: 'Всего записей {2}',
                    getPagingItems: function () {
                        return [];
                    },
                    onLoad: function () {
                        Ext.suspendLayouts();
                        this.updateInfo();
                        Ext.resumeLayouts(true);
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});