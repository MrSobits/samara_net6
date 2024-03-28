Ext.define('B4.view.fssp.courtordergku.LitigationPanel', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'Ext.ux.RowExpander',
        'B4.ux.grid.filter.YesNo'
    ],

    alias: 'widget.litigationpanel',
    title: 'Реестр делопроизводств',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.fssp.courtordergku.Litigation');

        Ext.applyIf(me,
            {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'JurInstitution',
                        text: 'Подразделение ОСП'
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'State',
                        text: 'Статус'
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'IndEntrRegistrationNumber',
                        text: 'Регистрационный номер ИП'
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'EntrepreneurCreateDate',
                        text: 'Дата создания ИП',
                        format: 'd.m.Y',
                        flex: 1,
                        filter: {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Сумма задолженности по ИП',
                        dataIndex: 'EntrepreneurDebtSum',
                        format: '0.00',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'Debtor',
                        text: 'Должник'
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'DebtorAddress',
                        text: 'Адрес должника'
                    },
                    {
                        xtype: 'gridcolumn',
                        hideable: false,
                        flex: 0.5,
                        align: 'center',
                        text: 'Адрес сопоставлен',
                        dataIndex: 'IsMatchAddress',
                        sortable: false,
                        filter: {
                            xtype: 'b4dgridfilteryesno',
                            operator: 'eq'
                        },
                        renderer: function (val, el, record) {
                            if (this.isAllowed) {
                                el.tdCls = 'x-action-col-cell';
                            }

                            if (val) {
                                return '<img src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" ' +
                                    'width="16" class="x-action-col-icon x-action-col-0 icon-accept" data-qtip="Изменить сопоставление с адресом">';
                            }

                            return '<img src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw== " ' +
                                'width="16" class="x-action-col-icon x-action-col-0 icon-decline" data-qtip="Сопоставить с адресом">';
                        },
                        processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                            if (type == 'click' && e.target.localName == 'img') {
                                var grid = view.ownerCt;

                                if (grid.isAllowed) {
                                    var record = view.getStore().getAt(recordIndex);
                                    grid.fireEvent('rowaction', grid, 'addressmatch', record);
                                }
                            }
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
                                xtype: 'checkbox',
                                name: 'ShowAll',
                                boxLabel: 'Показать все записи',
                                fieldStyle: 'vertical-align: middle;',
                                margin: '0 0 0 10',
                                checked: false
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