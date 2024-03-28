Ext.define('B4.view.appealcits.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.appealCitsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.menu.ListMenu',
        'B4.form.ComboBox',
        'B4.enums.YesNo'
    ],

    store: 'AppealCits',
    itemId: 'appealCitsGrid',
    closable: false,
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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 170,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealObjAddresses',
                    text: 'Адреса домов',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressEdo',
                    text: 'Адрес из ЭДО',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountRealtyObj',
                    width: 170,
                    flex: 1,
                    text: 'Количество домов',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsEdo',
                    flex: 1,
                    text: 'Из ЭДО',
                    hidden: true,
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountSubject',
                    width: 170,
                    flex: 1,
                    text: 'Количество тематик',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 80,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    width: 170,
                    flex: 1,
                    text: 'Номер ГЖИ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Распоряжение',
                    width: 170,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubjectName',
                    text: 'Тематика',
                    width: 170,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    text: 'Корреспондент',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата обращения',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckTime',
                    text: 'Контрольный срок',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SuretyResolve',
                    text: 'Резолюция',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executant',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExecuteDate',
                    text: 'Срок исполнения',
                    format: 'd.m.Y',
                    width: 75,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tester',
                    text: 'Проверяющий',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceNames',
                    text: 'Источники',
                    sortable: false,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceNumbers',
                    text: 'Исх. № источника',
                    sortable: false,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceDates',
                    text: 'Даты поступления',
                    sortable: false,
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    if (record.data.IsProcessedStateOver) {
                        return 'back-green';
                    }
                    else if (record.data.IsNotProcessedStateOver) {
                        return 'back-red';
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Экспорт',
                            textAlign: 'left',
                            itemId: 'btnExport'
                        },
                        {
                            text: 'Фильтр по отображению обращений',
                            itemId: 'menuFiltersBtn',
                            menu: {
                                xtype: 'listmenu',
                                itemId: 'menuFilters',
                                options: [
                                    {
                                        text: 'Показать обращения, имеющие связь с СОПР',
                                        fieldParam: 'showSoprAppeals'
                                    },
                                    {
                                        text: 'Показать обращения, обработанные в СОПР в установленный срок',
                                        fieldParam: 'showProcessedAppeals'
                                    },
                                    {
                                        text: 'Показать обращения, не обработанные в СОПР в установленный срок',
                                        fieldParam: 'showNotProcessedAppeals'
                                    },
                                    {
                                        text: 'Показать обращения, находящиеся в работе в СОПР',
                                        fieldParam: 'showInWorkAppeals'
                                    },
                                    {
                                        text: 'Показать закрытые обращения',
                                        fieldParam: 'showClosedAppeals'
                                    }
                                ]
                            }
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