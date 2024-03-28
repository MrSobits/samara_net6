Ext.define('B4.view.appealcits.appealcitsanswerregistration.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        'B4.enums.TypeAppealAnswer',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    title: 'Ответы на регистрацию',
    store: 'appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration',
    alias: 'widget.ansreggrid',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Ответ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата ответа',
                    flex: 0.5,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер ответа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealState',
                    flex: 1,
                    text: 'Статус обращения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealNumber',
                    flex: 1,
                    text: 'Номер обращения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    flex: 1,
                    text: 'Заявитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignerFio',
                    flex: 1,
                    text: 'Подписант',
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.TypeAppealAnswer',
                //    dataIndex: 'TypeAppealAnswer',
                //    flex: 1,
                //    text: 'Тип ответа',
                //    filter: true
                //},
                {
                    text: 'Отправлено',
                    dataIndex: 'Sended',
                    flex: 1,
                    filter: { xtype: 'b4dgridfilteryesno' },
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    text: 'Зарегестрировано',
                    dataIndex: 'Registred',
                    flex: 1,
                    filter: { xtype: 'b4dgridfilteryesno' },
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },        
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedFile',
                    width: 100,
                    text: 'Подписанный файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обработать входящие',
                                    tooltip: 'Обработать входящую электронную почту',
                                    iconCls: 'icon-pencil',
                                    width: 150,
                                    itemId: 'btnProcess'
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