Ext.define('B4.view.dpkrdocument.VersionGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dpkrdocument.ProgramVersion'
    ],

    alias: 'widget.dpkrdocumentversiongrid',

    initComponent: function () {
        let me = this,
            store = Ext.create('B4.store.dpkrdocument.ProgramVersion');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'VersionDate',
                    flex: 0.25,
                    text: 'Дата',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                        const form = gridView.up('dpkrdocumenteditwindow');
                        
                        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                            if (result === 'yes') {
                                rec.destroy()
                                    .next(function() {
                                        me.getStore().load();
                                        
                                        B4.Ajax.request({
                                            url: B4.Url.action('AddRealityObjects', 'DpkrDocumentRealityObject'),
                                            params: {
                                                dpkrDocumentId: form.getRecord().getId()
                                            },
                                            timeout: 9999999
                                        }).next(function (response) {
                                            let includedRoGrid = form.down('#includedRealityObjectGrid'),
                                                excludedRoGrid = form.down('#excludedRealityObjectGrid'),
                                                gridArray = [includedRoGrid, excludedRoGrid];

                                            gridArray.forEach(function (grid) {
                                                grid.getStore().load();
                                            });
                                        });
                                    });
                            }
                        });
                    }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать перечень домов',
                                    textAlign: 'left',
                                    iconCls: 'icon-page',
                                    itemId: 'btnAddRealityObjects'
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