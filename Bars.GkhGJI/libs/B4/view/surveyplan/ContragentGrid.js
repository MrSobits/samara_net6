Ext.define('B4.view.surveyplan.ContragentGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.Month',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox'
    ],

    store: 'surveyplan.Contragent',
    alias: 'widget.surveyPlanContragentGrid',
    enableColumnHide: true,

    initComponent: function() {
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
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальный район',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsExcluded',
                    text: 'Статус',
                    renderer: function(val) {
                        return val ? 'Исключен' : 'Актуальный';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Актуальный'], [true, 'Исключен']],
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'JuridicalAddress',
                    flex: 1,
                    text: 'Юридический адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    width: 100,
                    text: 'Телефон',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    width: 100,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kpp',
                    width: 100,
                    text: 'КПП',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'PlanMonth',
                    text: 'Месяц проверки',
                    enumName: 'B4.enums.Month',
                    width: 100,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    text: 'Год проверки',
                    width: 100,
                    filter: { xtype: 'textfield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AuditPurpose',
                    flex: 1,
                    text: 'Цель проверки',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Основание включения в план',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectEditDate',
                    width: 100,
                    text: 'Изменения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', format: 'd.m.Y', operand: CondExpr.operands.eq }
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
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'b4updatebutton'
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