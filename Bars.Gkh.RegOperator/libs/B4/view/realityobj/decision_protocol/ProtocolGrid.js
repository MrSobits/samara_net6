Ext.define('B4.view.realityobj.decision_protocol.ProtocolGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.protocolgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.realityobj.decision_protocol.Protocol',
        'B4.enums.CoreDecisionType',
        'B4.enums.CrFundFormationType',
        'B4.enums.AccountOwnerDecisionType'
    ],

    title: 'Протоколы и решения',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Id' },
                    { name: 'DocumentNum' },
                    { name: 'ProtocolDate' },
                    { name: 'ProtocolType' },
                    { name: 'State' },
                    { name: 'CrFundFormationType' },
                    { name: 'AccountOwnerDecisionType' },
                    { name: 'ManOrgName' },
                    { name: 'DateStart' },
                    { name: 'LetterNumber' },
                    { name: 'LetterDate' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'RealityObjectBothProtocol',
                    listAction: 'List'
                }
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер протокола'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProtocolType',
                    flex: 1,
                    text: 'Тип протокола',
                    renderer: function (val) {
                        return B4.enums.CoreDecisionType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.CoreDecisionType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProtocolDate',
                    flex: 1,
                    text: 'Дата принятия протокола',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 175,
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
                    dataIndex: 'CrFundFormationType',
                    width: 250,
                    text: 'Способ формирования фонда КР',
                    renderer: function (val) {
                        return B4.enums.CrFundFormationType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.CrFundFormationType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountOwnerDecisionType',
                    width: 250,
                    text: 'Владелец специального счета',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.AccountOwnerDecisionType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.AccountOwnerDecisionType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrgName',
                    flex: 1,
                    text: 'Управление домом',
                    hidden: true
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Дата вступления в силу протокола',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LetterNumber',
                    flex: 1,
                    text: 'Номер входящего письма',
                    hidden: true
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LetterDate',
                    flex: 1,
                    text: 'Дата входящего письма',
                    format: 'd.m.Y',
                    hidden: true
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
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