Ext.define('B4.view.taskcalendar.DisposalGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.KindKNDGJI',
        'B4.enums.TypeDisposalGji',
        'B4.enums.TypeAgreementProsecutor',
        'B4.DisposalTextValues'
    ],

    title: 'Распоряжения',
    store: 'taskcalendar.ListDisposals',
    itemId: 'taskcalendarDisposalGrid',
    alias: 'widget.taskcalendardisposalgrid',
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeDisposalGji',
                    filter: true,
                    header: 'Тип распоряжения',
                    dataIndex: 'TypeDisposal',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.KindKNDGJI',
                    filter: true,
                    header: 'Вид контроля',
                    dataIndex: 'KindKNDGJI',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorNames',
                    flex: 2,
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 1,
                    text: 'Вид проверки',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        url: '/KindCheckGji/List',
                        editable: false,
                        storeAutoLoad: false,
                        emptyItem: { Name: '-' },
                        valueField: 'Name'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
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
                    dataIndex: 'ContragentName',
                    flex: 2,
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectCount',
                    width: 65,
                    text: 'Количество домов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 65,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 70,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ERPID',
                    width: 65,
                    text: 'Номер в ЕРП',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Начало обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    text: 'Окончание обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
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
                            columns: 1,
                            items: [
                                {
                                    itemId: 'updatebutton',
                                    xtype: 'b4updatebutton'
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