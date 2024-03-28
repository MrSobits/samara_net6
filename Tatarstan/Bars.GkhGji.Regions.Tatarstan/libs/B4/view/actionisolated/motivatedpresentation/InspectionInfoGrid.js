Ext.define('B4.view.actionisolated.motivatedpresentation.InspectionInfoGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.motivatedpresentationinspectioninfogrid',
    itemId: 'motivatedPresentationInspectionInfoGrid',
    title: 'Сведения о проверках',
    
    store: 'actionisolated.motivatedpresentation.InspectionInfo',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Дата окончания обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер проверки',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TimeVisitStart',
                    text: 'Время начала',
                    format: 'H:i',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TimeVisitEnd',
                    text: 'Время окончания',
                    format: 'H:i',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Перейти',
                    tooltip: 'Переход в карточку основания проверки',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    align: 'center',
                    sortable: false,
                    flex: 0.5,
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.origScope;
                        if (!scope)
                            scope = this.up('grid');

                        scope.fireEvent('rowaction', scope, 'gotoinspection', rec);
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        var grid = this.up('motivatedpresentationinspectioninfogrid');

                                        grid.getStore().load();
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});