Ext.define('B4.view.appealcits.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appealCitsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.appealcits.GridTopToolBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'Ext.ux.grid.FilterBar',
    ],

    store: 'AppealCits',
    itemId: 'appealCitsGrid',
    closable: false,
    cls: 'x-grid-header',
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
                    width: 160,
                    text: 'Муниципальный район',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'MoSettlement',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'PlaceName',
                //    width: 160,
                //    text: 'Населенный пункт',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RealObjAddresses',
                //    text: 'Адрес',
                //    flex: 1,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityAddresses',
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
                    dataIndex: 'Correspondent',
                    text: 'Корреспондент',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrespondentAddress',
                    text: 'Адрес корреспондента',
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
                    xtype: 'datecolumn',
                    dataIndex: 'ExtensTime',
                    text: 'Продленный контрольный срок',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindStatement',
                    text: 'Вид обращения',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SuretyResolve',
                    text: 'Виза',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingSourcesName',
                    text: 'Источник',
                    sortable: false,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingSources',
                    text: 'Исх. № источника',
                    sortable: false,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RevenueSourceNumbers',
                //    text: 'Исх. № источника',
                //    sortable: false,
                //    flex: 1,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSourceDates',
                    text: 'Дата поступления',
                    width: 100,
                    flex: 1,
                    filter: { xtype: 'textfield'}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executants',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Controllers',
                    text: 'Проверяющий (инспектор)',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false,
                    enableOperators: false
                }],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var state = record.get('State');

                    if (state && state.FinalState) {
                        return 'back-coralgreen';
                    }

                    if (!record.get('HasExecutant')) {
                        return 'back-yellow';
                    }

                    if (record.get('ExtensTime')) {
                        if (new Date(record.get('CheckTime')) <= new Date()) {
                            return 'back-coralred';
                        }
                    }

                    if (!record.get('ExtensTime') && record.get('CheckTime')) {
                        if (new Date(record.get('CheckTime')) <= new Date()) {
                            return 'back-coralred';
                        }
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'appealcitsgridtoptoolbar',
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});