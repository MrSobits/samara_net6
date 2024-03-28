Ext.define('B4.view.realityobj.HousekeeperEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'realityobjHousekeeperEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    bodyPadding: 5,
    title: 'Старший по дому',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'FIO',
                    fieldLabel: 'ФИО',
                    maxLength: 150
                },
                {
                    xtype: 'textfield',
                    name: 'PhoneNumber',
                    fieldLabel: 'Номер телефона',
                    maskRe: /[0-9]/i,
                    maxLength: 11
                },
                {
                    xtype: 'combobox',
                    name: 'IsActive',
                    fieldLabel: 'Активен',
                    displayField: 'Display',
                    store: B4.enums.YesNoNotSet.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'Login',
                    fieldLabel: 'Логин',
                    allowBlank: false,
                    maxLength: 30
                },               
                {
                    xtype: 'textfield',
                    name: 'NewPassword',
                    fieldLabel: 'Новый пароль',
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'NewConfirmPassword',
                    fieldLabel: 'Подтверждение пароля',
                    maxLength: 300
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