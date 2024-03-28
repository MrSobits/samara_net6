Ext.define('B4.view.specialaccount.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 650,
    bodyPadding: 5,
    itemId: 'specialAccountAddWindow',
    title: 'Отчет по спецсчетам',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YearEnums',
        'B4.enums.AmmountMeasurement',
        'B4.enums.MonthEnums',
        'B4.store.CreditOrg',
        'B4.store.Contragent',
        'B4.store.dict.Municipality',

    ],

    initComponent: function () {
        var me = this;    

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
               {
                   xtype: 'container',
                   layout: 'hbox',
                   defaults: {
                       labelWidth: 100,
                       labelAlign: 'right',
                       flex: 1
                   },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'MonthEnums',
                            fieldLabel: 'Период',
                            displayField: 'Display',
                            store: B4.enums.MonthEnums.getStore(),
                            valueField: 'Value',
                            itemId: 'monthEnums',
                            editable: false,
                            //value: 30,
                            width: 30

                        },
                        {
                            xtype: 'combobox',
                            name: 'YearEnums',
                            fieldLabel: 'Год',
                            displayField: 'Display',
                            store: B4.enums.YearEnums.getStore(),
                            valueField: 'Value',
                            itemId: 'yearEnums',
                            editable: false,
                            // value: 30,
                            width: 100
                        }

                   ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margin: '10 0 10 0',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Tariff',
                            itemId: 'dfTariff',
                            fieldLabel: 'Тариф',
                            decimalSeparator: ',',
                            minValue: 0,
                            allowBlank: false,
                            flex: 0.5,
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            textProperty: 'ShortName',
                            labelWidth: 130,
                            name: 'Contragent',
                            fieldLabel: 'Владелец спецсчета',
                            itemId: 'sfContragent',
                            disabled: false,
                            editable: false,
                            allowBlank: true,
                            flex: 1,
                            columns: [
                                {
                                    header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        }
                    ]
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