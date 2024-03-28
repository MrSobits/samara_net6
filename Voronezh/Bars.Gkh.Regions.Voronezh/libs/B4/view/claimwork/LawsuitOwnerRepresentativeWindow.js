Ext.define('B4.view.claimwork.LawsuitOwnerRepresentativeWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.lawsuitownerrepwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 500,
    bodyPadding: 5,
    title: 'Добавление законного представителя',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.RepresentativeType',
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'RepresentativeType',
                    fieldLabel: 'Законный представитель',
                    enumName: 'B4.enums.RepresentativeType',
                    includeEmpty: false,
                    allowBlank: false,
                    hideTrigger: false
                },
                {
                    xtype: 'textfield',
                    name: 'Surname',
                    fieldLabel: 'Фамилия'
                },
                {
                    xtype: 'textfield',
                    name: 'FirstName',
                    fieldLabel: 'Имя'
                },
                {
                    xtype: 'textfield',
                    name: 'Patronymic',
                    fieldLabel: 'Отчество',
                    allowBlank: true
                },
                {
                    xtype: 'datefield',
                    name: 'BirthDate',
                    fieldLabel: 'Дата рождения',
                    format: 'd.m.Y',
                    allowBlank: true
                },
                {
                    xtype: 'textfield',
                    name: 'BirthPlace',
                    fieldLabel: 'Место рождения',
                    allowBlank: true
                },
                {
                    xtype: 'textfield',
                    name: 'LivePlace',
                    fieldLabel: 'Место жительства',
                    allowBlank: true
                },
                {
                    xtype: 'textfield',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    allowBlank: true
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },

                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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