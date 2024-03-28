Ext.define('B4.view.dict.privilegedcategory.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 450,
    bodyPadding: 5,
    itemId: 'privilegedcategoryEditWindow',
    title: 'Группа льготных категорий граждан',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
            {
                xtype: 'textfield',
                name: 'Code',
                fieldLabel: 'Код',
                allowBlank: false,
                maxLength: 300
            },
            {
                xtype: 'textfield',
                name: 'Name',
                fieldLabel: 'Наименование',
                allowBlank: false,
                maxLength: 300
            },
            {
                xtype: 'numberfield',
                name: 'Percent',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                fieldLabel: 'Процент льготы',
                allowBlank: false,
                minValue: 0,
                maxValue: 100
            },
            {
                xtype: 'container',
                margin: '0 0 5 0',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [
                    {
                        xtype: 'checkbox',
                        name: 'HasLimitArea',
                        padding: '0 5 0 0',
                        fieldLabel: 'Предельное значение площади',
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    {
                        xtype: 'numberfield',
                        name: 'LimitArea',
                        flex: 1,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        disabled: true,
                        decimalSeparator: ',',
                        minValue: 0,
                        fieldLabel: 'Предельное значение площади',
                        hideLabel: true
                    }
                ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateFrom',
                    fieldLabel: 'Действует с',
                    allowBlank: false,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'DateTo',
                    fieldLabel: 'Действует по',
                    format: 'd.m.Y'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});