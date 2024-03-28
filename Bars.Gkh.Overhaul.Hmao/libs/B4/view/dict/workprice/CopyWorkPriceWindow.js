Ext.define('B4.view.dict.workprice.CopyWorkPriceWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.copyworkpricewindow',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.ComboBox',
        'B4.view.dict.municipality.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.municipality.ListByParamAndOperator'
    ],

    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    bodyPadding: 5,
    title: 'Копирование расценок',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5',
                flex: 1
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Скопировать из:',
                    name: 'CopyFrom',
                    store: 'B4.store.dict.municipality.ListByParamAndOperator',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Муниципальный район', dataIndex: 'ParentMo', flex: 2,
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
                        {
                            text: 'Муниципальное образование', dataIndex: 'Name', flex: 3,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListSettlementWithoutPaging'
                            }
                        }
                    ]
                    
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'CopyToMunicipalities',
                    itemId: 'copyToMunicipalitiesTrigerField',
                    allowBlank: false,
                    fieldLabel: 'Cкопировать в:'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'municipalityWorkPrices',
                    itemId: 'municipalityWorkPricesTrigerField',
                    fieldLabel: 'Расценки по работам',
                    disabled: true
                }
            ],
            tbar: {
                items: [
                    { xtype: 'b4savebutton' },
                    { xtype: 'tbfill' },
                    { xtype: 'b4closebutton' }
                ]
            }
        });
        me.callParent(arguments);
    }
});