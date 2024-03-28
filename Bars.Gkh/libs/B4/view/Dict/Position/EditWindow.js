Ext.define('B4.view.dict.position.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 400,
    bodyPadding: 5,
    itemId: 'positionEditWindow',
    title: 'Должность',
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
                labelWidth: 100,
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
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'fieldset',
                    title: 'Падежи',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'textfield',
                        maxLength: 300,
                        labelWidth: 90,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            fieldLabel: 'Родительный',
                            name: 'NameGenitive'
                        },
                        {
                            fieldLabel: 'Дательный',
                            name: 'NameDative'
                        },
                        {
                            fieldLabel: 'Винительный',
                            name: 'NameAccusative'
                        },
                        {
                            fieldLabel: 'Творительный',
                            name: 'NameAblative'
                        },
                        {
                            fieldLabel: 'Предложный',
                            name: 'NamePrepositional'
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
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});