Ext.define('B4.view.prescription.CancelGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.YesNoNotSet'
    ],

    alias: 'widget.prescriptionCancelGrid',
    title: 'Решения',
    store: 'prescription.Cancel',
    itemId: 'prescriptionCancelGrid',

    /*
    Перекрывается в модуле GkhGji.Regions.Smolensk
    */
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCancel',
                    text: 'Дата отмены',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsCourt',
                    text: 'Отменено судом',
                    format: 'd.m.Y',
                    width: 100,
                    renderer: function (val) { return B4.enums.YesNoNotSet.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssuedCancel',
                    flex: 1,
                    text: 'ДЛ, вынесшее решение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Причина'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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