Ext.define('B4.view.objectcr.TypeWorkSendWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Выберите программу КПР',
    closeAction: 'destroy',

    alias: 'widget.typeworkcrsendwindow',

    requires: [      
        'B4.form.SelectField',
        'B4.store.dict.ProgramCrObj',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCrObj',
                    itemId: 'sfProgramCrObj',
                    fieldLabel: 'Программа КПР',
                    store: 'B4.store.dict.ProgramCrObj',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    itemId: 'taDescription',
                    fieldLabel: '',
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
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('typeworkcrsendwindow').close();
                                        }
                                    }
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