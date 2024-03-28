/*
* Грид обращений граждан. При наличия модуля Интеграция с ЭДО перекрывается этим модулем
*/
Ext.define('B4.view.appealcits.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appealCitsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo'
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
                    width: 100,
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
                    width: 150,
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
                    text: 'Адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountRealtyObj',
                    width: 140,
                    flex: 1,
                    hidden: true,
                    text: 'Количество домов',
                    filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 80,
                    text: 'Номер',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    width: 170,
                    flex: 1,
                    text: 'Номер обращения',
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
                    xtype: 'booleancolumn',
                    dataIndex: 'SpecialControl',
                    flex: 1,
                    text: 'Особый контроль',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno', operator: CondExpr.operands.eq }
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
                    dataIndex: 'ExecutantNames',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SuretyDate',
                    text: 'Дата ответа',
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
                    dataIndex: 'DescriptionLocationProblem',
                    text: 'Описание',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceNames',
                    text: 'Источники',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceNumbers',
                    text: 'Исх. № источника',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceDates',
                    text: 'Даты поступления',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [{ ptype: 'filterbar', renderHidden: false, showShowHideButton: false, showClearAllButton: false }],
            viewConfig: {
                loadMask: true
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
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseAppeals',
                            boxLabel: 'Показать закрытые обращения',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
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