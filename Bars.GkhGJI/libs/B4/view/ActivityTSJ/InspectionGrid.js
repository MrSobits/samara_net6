Ext.define('B4.view.activitytsj.InspectionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    order: false,   
    title: 'Документы',
    store: 'BaseActivityTsj',
    itemId: 'activityTsjInspectionGrid',
    closable: true,
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                  xtype: 'b4editcolumn',
                  scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DisposalDocumentDate',
                    flex: 1,
                    text: 'Дата документа',
                    format:'d.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisposalDocumentNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisposalTypeCheck',
                    flex: 1,
                    text: 'Вид проверки'
                }//, Колонка закомментирована специально. Удаление в карточке документа кнопка удалить. НЕ РАСКОММЕНТИРОВАТЬ!!!
//                {
//                    xtype: 'b4deletecolumn',
//                    scope: me
//                }
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