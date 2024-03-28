Ext.define('B4.view.competition.ProtocolGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.competitionprotocolgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.competition.Protocol',
        'B4.enums.TypeCompetitionProtocol',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox'
    ],

    title: 'Протоколы',
    
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.competition.Protocol');

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
                    dataIndex: 'TypeProtocol',
                    flex: 1,
                    text: 'Тип протокола',
                    renderer: function (val) { return B4.enums.TypeCompetitionProtocol.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeCompetitionProtocol.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'SignDate',
                    width: 150,
                    text: 'Дата подписания',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsCancelled',
                    width: 150,
                    text: 'Конкурс не состоялся',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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