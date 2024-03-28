Ext.define('B4.view.normconsumption.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 200,
    maxHeight: 200,
    bodyPadding: 5,
    alias: 'widget.normconsumptionAddWindow',
    title: 'Нормативы потребления',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.enums.NormConsumptionType',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    store: Ext.create('B4.store.dict.Municipality'),
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    name: 'Period',
                    operand: CondExpr.operands.eq,
                    width: 300,
                    storeAutoLoad: false,
                    fieldLabel: 'Период',
                    editable: false,
                    allowBlank: false,
                    valueField: 'Id',
                    emptyItem: { Name: '-' },
                    store: Ext.create('B4.store.dict.PeriodNormConsumption')
                },
                {
                    xtype: 'b4combobox',
                    name: 'Type',
                    fieldLabel: 'Вид норматива потребления',
                    width: 300,
                    storeAutoLoad: false,
                    editable: false,
                    allowBlank: false,
                    valueField: 'Value',
                    displayField: 'Display',
                    emptyItem: { Name: '-' },
                    store: B4.enums.NormConsumptionType.getStore()
                }
            ],
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});