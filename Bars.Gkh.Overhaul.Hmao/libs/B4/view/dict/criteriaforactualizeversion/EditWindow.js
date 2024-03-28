Ext.define('B4.view.dict.criteriaforactualizeversion.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.criteriaforactualizeversionwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.enums.CriteriaType'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    hight: 600,
    bodyPadding: 5,
    title: 'Редактирование',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'CriteriaType',
                    fieldLabel: 'Тип',
                    displayField: 'Display',
                    store: B4.enums.CriteriaType.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    itemId: 'cbCriteriaType',
                    maxWidth: 553
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Нижнее пороговое значение',
                    name: 'ValueFrom',
                    allowDecimals: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Верхнее пороговое значение',
                    name: 'ValueTo',
                    allowDecimals: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Количество баллов',
                    name: 'Points',
                    allowDecimals: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Весовой коэффициент',
                    name: 'Weight',
                    allowDecimals: true,
                    decimalPrecision: 2,
                    decimalSeparator: ',',
                    allowBlank: false
                },
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