Ext.define('B4.view.dict.multipurpose.GlossaryEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.mixins.window.ModalMask'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    width: 400,
    height: 150,
    resizable: true,
    itemId: 'multipurposeGlossaryEdit',
    title: 'Универсальный справочник',
    closeAction: 'hide',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    border: false,
                    padding: 5,
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Код',
                            allowBlank: false,
                            name: 'Code'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            name: 'Name'
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