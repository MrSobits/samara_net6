Ext.define('B4.view.objectcr.FinanceSourceResEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.financesourcereseditwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minHeight: 205,
    maxHeight: 300,
    minWidth: 400,
    maxWidth: 700,
    bodyPadding: 5,
    itemId: 'financeSourceResEditWindow',
    title: 'Средства источников финансирования',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.store.objectcr.TypeWorkCr',
        'B4.store.dict.FinanceSource',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this,
            yearsStore = Ext.create('Ext.data.Store', {
                fields: [{ name: 'Year' }]
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'FinanceSource',
                    fieldLabel: 'Разрез финансирования',                  
                    store: 'B4.store.dict.FinanceSource',
                    isGetOnlyIdProperty: false,
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'TypeWorkCr',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.objectcr.TypeWorkCr',
                    textProperty: 'WorkName',
                    columns: [
                        { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Year',
                    fieldLabel: 'Год',
                    store: yearsStore,
                    idProperty: 'Year',
                    textProperty: 'Year',
                    selectionMode: 'MULTI',
                    columns: [
                        { text: 'Год', dataIndex: 'Year', flex: 1 }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'BudgetMu',
                    fieldLabel: 'Бюджет МО'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'BudgetSubject',
                    fieldLabel: 'Бюджет субъекта'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'OwnerResource',
                    fieldLabel: 'Средства собственника'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'FundResource',
                    fieldLabel: 'Средства фонда'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'OtherResource',
                    fieldLabel: 'Иные источники',
                    readOnly: true
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});