Ext.define('B4.view.gisGkh.ROGrid', {
    //extend: 'Ext.tree.Panel',
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisgkhrogrid',

    requires: [
        //'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'gisGkh.ROForGisGkhExportStore',
    title: 'Дома',
    closable: false,
    enableColumnHide: true,
    

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberApartments',
                    flex: 1,
                    text: 'Жилых помещений',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGisGkhMatchedApartments',
                    flex: 1,
                    text: 'Жилых сопоставлено',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberNonResidentialPremises',
                    flex: 1,
                    text: 'Нежилых помещений',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGisGkhMatchedNonResidental',
                    flex: 1,
                    text: 'Нежилых сопоставлено',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    dataIndex: 'MatchedRooms',
                    text: 'Все помещения сопоставлены',
                    flex: 1,
                    filter: true,
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: true,
                //    showClearAllButton: true,
                //    pluginId: 'headerFilter'
                //},
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                //{
                //    xtype: 'toolbar',
                //    dock: 'top',
                //    items: [
                //        {
                //            xtype: 'buttongroup',
                //            columns: 4,
                //            items: [
                //                //{
                //                //    xtype: 'b4addbutton'
                //                //},
                //                {
                //                    xtype: 'b4updatebutton'
                //                },
                //                //{
                //                //    xtype: 'button',
                //                //    text: 'Получить справочники',
                //                //    tooltip: 'Получить справочники',
                //                //    iconCls: 'icon-accept',
                //                //    width: 150,
                //                //    itemId: 'btnGetDictionaries'
                //                //},
                //                //{
                //                //    xtype: 'button',
                //                //    text: 'Получить ответы',
                //                //    tooltip: 'Получить ответы',
                //                //    iconCls: 'icon-accept',
                //                //    width: 150,
                //                //    itemId: 'btnCheckAnswers'
                //                //}
                //                //{
                //                //    xtype: 'button',
                //                //    text: 'История запросов',
                //                //    tooltip: 'Посмотреть историю запросов',
                //                //    iconCls: 'icon-accept',
                //                //    width: 150,
                //                //    itemId: 'btnGetPaymentsHistory'
                //                //}
                //            ]
                //        }
                //    ]
                //},
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
