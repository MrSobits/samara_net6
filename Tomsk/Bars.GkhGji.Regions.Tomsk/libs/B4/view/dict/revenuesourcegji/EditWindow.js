Ext.define('B4.view.dict.revenuesourcegji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'revenueSourceGjiEditWindow',
    title: 'Источник поступлений',
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
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    anchor: '100%',
                    maxLength: 300
                },
                {
                    xtype: 'fieldset',
                    title: 'Падежи',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'textfield'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NameGenitive',
                            fieldLabel: 'Родительный'
                        },
                        {
                            xtype: 'textfield',
                            name: 'NameDative',
                            fieldLabel: 'Дательный'
                        },
                        {
                            xtype: 'textfield',
                            name: 'NameAccusative',
                            fieldLabel: 'Винительный'
                        },
                        {
                            xtype: 'textfield',
                            name: 'NameAblative',
                            fieldLabel: 'Творительный'
                        },
                        {
                            xtype: 'textfield',
                            name: 'NamePrepositional',
                            fieldLabel: 'Предложный'
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