Ext.define('B4.view.contragent.ContactCasesPanel', {
    extend: 'Ext.container.Container',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    alias: 'widget.contragentContactCasesPanel',
    title: 'Падежи',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'container',
                padding: '5 0 5 0',
                layout: { type: 'hbox' },
                defaults: {
                    flex: 1,
                    padding: '0 3 0 3'
                }
            },
            items: [
                {
                    items: [
                        {
                            xtype: 'component'
                        },
                        {
                            xtype: 'label',
                            text: 'Фамилия'
                        },
                        {
                            xtype: 'label',
                            text: 'Имя'
                        },
                        {
                            xtype: 'label',
                            text: 'Отчество'
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'label',
                            text: 'Родительный'
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'SurnameGenitive',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'NameGenitive',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'PatronymicGenitive',
                            maxLength: 100
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'label',
                            text: 'Дательный'
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'SurnameDative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'NameDative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'PatronymicDative',
                            maxLength: 100
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'label',
                            text: 'Винительный'
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'SurnameAccusative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'NameAccusative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'PatronymicAccusative',
                            maxLength: 100
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'label',
                            text: 'Творительный'
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'SurnameAblative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'NameAblative',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'PatronymicAblative',
                            maxLength: 100
                        }
                    ]
                },
                {
                    items: [
                        {
                            xtype: 'label',
                            text: 'Предложный'
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'SurnamePrepositional',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'NamePrepositional',
                            maxLength: 100
                        },
                        {
                            xtype: 'textfield',
                            hideLabel: true,
                            name: 'PatronymicPrepositional',
                            maxLength: 100
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});