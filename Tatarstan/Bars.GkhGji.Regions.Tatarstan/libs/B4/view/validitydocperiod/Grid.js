Ext.define('B4.view.validitydocperiod.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.validitydocperiodgrid',
    requires: [
        'B4.enums.TypeDocumentGji',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Add',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'B4.store.ValidityDocPeriod',

    title: 'Периоды действия документов ГЖИ',
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me,
            {
                columnLines: true,
                height: 600,
                columns: [
                    {
                        xtype: 'b4editcolumn',
                        scope: me
                    },
                    {
                        xtype: 'b4enumcolumn',
                        enumName: 'B4.enums.TypeDocumentGji',
                        text: 'Тип документа',
                        flex: 1,
                        dataIndex: 'TypeDocument'
                    },
                    {
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        text: 'Дата начала действия',
                        flex: 1,
                        dataIndex: 'StartDate'
                    },
                    {
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        text: 'Дата окончания действия',
                        flex: 1,
                        dataIndex: 'EndDate'
                    },
                    {
                        xtype: 'b4deletecolumn'
                    }
                ],
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
            }
        );

        me.callParent(arguments);
    }
});