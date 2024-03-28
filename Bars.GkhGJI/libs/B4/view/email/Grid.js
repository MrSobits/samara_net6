Ext.define('B4.view.email.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.column.Delete',
        'B4.form.ComboBox',
        'B4.enums.EmailGjiType',
        'B4.enums.EmailDenailReason',
        'B4.enums.EmailGjiSource',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    title: 'Электронные письма',
    store: 'email.EmailGji',
    itemId: 'emailGjiGrid',
    alias: 'widget.emailgjigrid',
    closable: true,

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'From',
                    flex: 1,
                    text: 'От кого',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SenderInfo',
                    flex: 1,
                    text: 'Отправитель',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EmailDate',
                    text: 'Дата письма',
                    flex: 0.5,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Theme',
                    flex: 1,
                    text: 'Тема',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GjiNumber',
                    flex: 1,
                    text: 'Номер ГЖИ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.EmailGjiType',
                    dataIndex: 'EmailType',
                    flex: 1,
                    text: 'Тип письма',
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.EmailGjiSource',
                    dataIndex: 'EmailGjiSource',
                    flex: 0.5,
                    text: 'Источник письма',
                    filter: true
                },
                {
                    xtype: 'booleancolumn',
                    falseText: 'Нет',
                    trueText: 'Да',
                    dataIndex: 'Registred',
                    flex: 1,
                    text: 'Обработано',
                    filter:
                    {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },          
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EmailPdf',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.EmailDenailReason',
                    dataIndex: 'EmailDenailReason',
                    flex: 1,
                    text: 'Причина отклонения',
                    filter: true
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