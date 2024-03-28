Ext.define('B4.view.actionisolated.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.actionisolated.TaskAction',
        'B4.enums.KindAction',
        'B4.enums.TypeBaseAction',
        'B4.enums.TypeObjectAction',
        'B4.ux.grid.filter.YesNo',
    ],

    alias: 'widget.actionisolatedgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.actionisolated.TaskAction');

        me.relayEvents(store, ['beforeload'], 'actionisolatedstore.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 150,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gji_document_task_actionisolated';
                            },
                            storeloaded: {
                                fn: function(me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function(type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 100,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'KindAction',
                    text: 'Вид мероприятия',
                    width: 160,
                    enumName: 'B4.enums.KindAction',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 100,
                    text: 'Дата',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeBase',
                    width: 160,
                    text: 'Основание мероприятия',
                    enumName: 'B4.enums.TypeBaseAction',
                    filter: true
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsPlanDone',
                    width: 160,
                    text: 'Проведено по плану',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeObject',
                    width: 160,
                    text: 'Объект мероприятия',
                    enumName: 'B4.enums.TypeObjectAction',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonName',
                    flex: 1,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
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
                    dataIndex: 'Inspectors',
                    flex: 1,
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    margin: '0 0 0 10',
                                    xtype: 'checkbox',
                                    name: 'IsClosed',
                                    boxLabel: 'Показать закрытые мероприятия'
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