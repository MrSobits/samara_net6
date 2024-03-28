Ext.define('B4.view.heatseason.InspectionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.DisposalTextValues'
    ],

    order: false,   
    title: 'Обследование дома',
    store: 'BaseHeatSeason',
    itemId: 'heatSeasonInspectionGrid',
    alias: 'widget.heatingseasoninspectiongrid',
    
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
                     xtype: 'gridcolumn',
                     dataIndex: 'DisposalDocumentNumber',
                     flex: 1,
                     text: B4.DisposalTextValues.getSubjectiveCase(),
                     renderer: function(val) {
                         return val ? '№ ' + val : '';
                     }
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
                    dataIndex: 'CountViol',
                    flex: 1,
                    text: 'Количество нарушений'
                }
                //, Колонка закомментирована специально. Удаление в карточке документа кнопка удалить. НЕ РАСКОММЕНТИРОВАТЬ!!!
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