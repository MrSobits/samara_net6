Ext.define('B4.view.dict.meteringdevice.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 400,
    bodyPadding: 5,
    itemId: 'meteringDeviceEditWindow',
    title: 'Прибор учета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeAccounting',
        'B4.enums.MeteringDeviceGroup'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Группа',
                    store: B4.enums.MeteringDeviceGroup.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'Group'
                },
                {
                    xtype: 'textfield',
                    name: 'AccuracyClass',
                    fieldLabel: 'Класс точности',
                    allowBlank: false,
                    maxLength: 30
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Тип учета',
                    store: B4.enums.TypeAccounting.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeAccounting'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'ReplacementCost',
                    decimalSeparator: ',',
                    fieldLabel: 'Стоимость замены (руб.)'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'LifeTime',
                    allowDecimals: false,
                    fieldLabel: 'Срок эксплуатации'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    flex: 1,
                    maxLength: 1000
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