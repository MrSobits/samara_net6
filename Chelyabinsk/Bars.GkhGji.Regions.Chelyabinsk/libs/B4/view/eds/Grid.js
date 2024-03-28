Ext.define('B4.view.eds.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeBase',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.esdgrid',
    store: 'eds.EDSInspection',
    closable: true,
    title: 'Реестр СЭД',
    enableColumnHide: true,
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeBase',
                    dataIndex: 'TypeBase',
                    text: 'Основание',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BaseDoc',
                    flex: 1,
                    text: 'Документ - основание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocNumber',
                    flex: 1,
                    text: 'Номер документа - основания',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentINN',
                    flex: 1,
                    text: 'ИНН',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Disposals',
                    flex: 0.5,
                    text: 'Приказы',
                    filter: { xtype: 'textfield' }
                },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'inspectors',
                    flex: 0.5,
                    text: 'Инспектор',
                    filter: { xtype: 'textfield' }
                },
              
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'InspectionDate',
                //    flex: 0.5,
                //    text: 'Дата проверки',
                //    format: 'd.m.Y',
                //    renderer: function (v) {
                //        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                //            v = undefined;
                //        }
                //        return Ext.util.Format.date(v, 'd.m.Y');
                //    }
                //},              
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    flex: 0.5,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                }

            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var isOpened = record.get('NotOpened');
                    if (isOpened) {
                        return 'x-summary';
                    }

                    return '';
                },

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