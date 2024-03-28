Ext.define('B4.view.appealcits.AppealCitsResolutionExecutorEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    minWidth: 520,
    //minHeight: 310,
    height: 300,
    bodyPadding: 5,
    itemId: 'appealCitsResolutionExecutorEditWindow',
    title: 'Форма редактирования исполнителя',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Surname',
                    fieldLabel: 'Фамилия',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Имя',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'Patronymic',
                    fieldLabel: 'Отчество',
                    maxLength: 50
                },
                {
                    xtype: 'datefield',
                    name: 'PersonalTerm',
                    fieldLabel: 'Персональный срок',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    height: 50,
                    anchor: '100%',
                    name: 'Comment',
                    fieldLabel: 'Комментарий',
                    labelAlign: 'right',
                    maxLength: 300
                },
                {
                    xtype: 'combobox',
                    name: 'IsResponsible',
                    labelWidth: 130,
                    labelAlign: 'right',
                    fieldLabel: 'Ответственный',
                    displayField: 'Display',
                    store: B4.enums.YesNo.getStore(),
                    valueField: 'Value'
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