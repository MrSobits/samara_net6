Ext.define('B4.view.MkdChangeNotificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.mkdchangenotificationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.MkdChangeNotification',
        'B4.form.GridStateColumn'
    ],

    title: 'Реестр уведомлений о смене способа управления МКД',
    closable: true,
    enableColumnHide: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.MkdChangeNotification');

        Ext.applyIf(me, {
            store: store,
            cls:'x-large-head',
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 150,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, st, options) {
                                options.params.typeId = 'gji_mkd_change_notification';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
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
                    dataIndex: 'RegistrationNumber',
                    text: 'Регистрационный номер дела',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    },
                    width: 110
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InboundNumber',
                    text: 'Номер',
                    filter: { xtype: 'textfield' },
                    width: 50
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'RegistrationDate',
                    text: 'Дата поступления',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    width: 80
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
                    dataIndex: 'Settlement',
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotificationCause',
                    text: 'Причина уведомления',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldMkdManagementMethod',
                    text: 'Наименование предыдущего способа управления',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldManagingOrganization',
                    text: 'Наименование юридического лица',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldInn',
                    text: 'ИНН',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldOgrn',
                    text: 'ОГРН',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewMkdManagementMethod',
                    text: 'Наименование нового способа управления',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewManagingOrganization',
                    text: 'Наименование нового юридического лица',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewInn',
                    text: 'ИНН',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewOgrn',
                    text: 'ОГРН',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewJuridicalAddress',
                    text: 'Юридический адрес',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewManager',
                    text: 'ФИО председателя ТСЖ / руководителя управляющей организации',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewPhone',
                    text: 'Телефон',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewEmail',
                    text: 'E-mail',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewOfficialSite',
                    text: 'Официальный сайт',
                    filter: { xtype: 'textfield' },
                    flex: 0.8
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'NewActCopyDate',
                    text: 'Дата предоставления копии акта приема-передачи технической документации',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        return val ? Ext.Date.format(new Date(val), 'd.m.Y') : "";
                    },
                    width: 95
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
                                { xtype: 'b4addbutton' },
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});