Ext.define('B4.view.objectcr.PersonalAccountEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.objectcrpersonalaccounteditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    maxHeight: 150,
    minHeight: 150,
    minWidth: 400,
    maxWidth: 600,
    title: 'Лицевой счет',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.TypeFinanceGroup'
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
                    name: 'Account',
                    fieldLabel: 'Лицевой счет',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Группа финансирования',
                    store: B4.enums.TypeFinanceGroup.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'FinanceGroup',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'checkbox',
                    name: 'Closed',
                    fieldLabel: 'Счет закрыт'
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