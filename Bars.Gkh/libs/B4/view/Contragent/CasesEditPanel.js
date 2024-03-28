Ext.define('B4.view.contragent.CasesEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    border: true,
    frame: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.contragentCasesEditPanel',
    title: 'Падежи',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%',
                        maxLength: 300
                    },
                    title: 'Падежи',
                    items: [
                        {
                            name: 'NameGenitive',
                            fieldLabel: 'Родительный'
                        },
                        {
                            name: 'NameDative',
                            fieldLabel: 'Дательный'
                        },
                        {
                            name: 'NameAccusative',
                            fieldLabel: 'Винительный'
                        },
                        {
                            name: 'NameAblative',
                            fieldLabel: 'Творительный'
                        },
                        {
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});