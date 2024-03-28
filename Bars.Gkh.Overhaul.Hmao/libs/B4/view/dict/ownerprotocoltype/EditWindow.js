Ext.define('B4.view.dict.ownerprotocoltype.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.ownerProtocolTypeEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.ownerprotocoltype.OwnerProtocolTypeDecisionGrid'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 600,
    height: 400,
    bodyPadding: 5,
    itemId: 'ownerProtocolTypeEditWindow',
    title: 'Вид протокола',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                { xtype: 'container',
                    layout: 'vbox',
                    align: 'stretch',
                    items: [
                        {
                            xtype: 'container',
                            width: '100%',
                            layout: 'anchor',
                            items: [
                                      {
                                          xtype: 'textfield',
                                          name: 'Code',
                                          fieldLabel: 'Код',
                                          labelAlign: 'right',
                                          allowBlank: false,
                                          maxLength: 300
                                      },
                                     {
                                         xtype: 'textfield',
                                         name: 'Name',
                                         labelAlign: 'right',
                                         fieldLabel: 'Наименование',
                                         anchor: '100%',
                                         maxLength: 2000
                                     },
                                    {
                                        xtype: 'textfield',
                                        name: 'Description',
                                        labelAlign: 'right',
                                        fieldLabel: 'Описание',
                                        anchor: '100%',
                                        maxLength: 2000
                                    }
                                    ]
                                },
                                    {
                                        xtype: 'ownerProtocolTypeDecisionGrid',
                                        flex: 1,
                                        width: '100%'
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