Ext.define('B4.view.businessactivity.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.ux.grid.filter.YesNo',
        
        'B4.enums.TypeKindActivity'
    ],

    title: 'Уведомления о начале предпринимательской деятельности',
    store: 'BusinessActivity',
    alias: 'widget.businessActivityGrid',
    closable: true,

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
                    width: 140,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_business_activity';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    flex: 1,
                    text: 'Мун. образование',
                    tooltip: 'Муниципальное образование',
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
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Наименование юр.лица',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrgFormName',
                    flex: 1,
                    text: 'Орг.-правовая форма',
                    tooltip: 'Организационно-правовая форма',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentMailingAddress',
                    flex: 1,
                    text: 'Почтовый адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentOgrn',
                    width: 90,
                    text: 'ОГРН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentInn',
                    width: 75,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeKindActivity',
                    flex: 1,
                    text: 'Вид деятельности',
                    renderer: function (val) { return B4.enums.TypeKindActivity.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeKindActivity.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ServiceCount',
                    width: 70,
                    text: 'Кол-во услуг',
                    tooltip: 'Количество услуг',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRegistration',
                    width: 80,
                    format: 'd.m.Y',
                    text: 'Дата регистрации',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegNum',
                    flex: 1,
                    text: 'Регистрационный номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegNumDateYear',
                    width: 100,
                    sortable: false,
                    tooltip: 'Дата поступления уведомления и его регистрационный номер',
                    text: 'Дата поступления и номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingNotificationNum',
                    width: 60,
                    text: 'Входящий номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateNotification',
                    width: 75,
                    format: 'd.m.Y',
                    text: 'Дата уведомления',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasFile',
                    width: 60,
                    format: 'd.m.Y',
                    text: 'Документ',
                    tooltip: 'Загружен документ',
                    sortable: false,
                    renderer: function (val) { return val ? 'Да' : 'Нет'; },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsOriginal',
                    width: 55,
                    format: 'd.m.Y',
                    text: 'Оригинал',
                    renderer: function (val) { return val ? 'Да' : 'Нет'; },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index, rowParams) {
                    if (!record.get('Registered')) {
                        if (record.get('DateNotification')) {
                            if (Ext.Date.parse(record.get('DateNotification'), "Y-m-dTH:i:s") < Ext.Date.add(new Date(), Ext.Date.DAY, -10)) {
                                return 'back-coralyellow';
                            }
                        }
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
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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