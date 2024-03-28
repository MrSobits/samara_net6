Ext.define('B4.view.objectcr.TypeWorkChangeYearEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.typeworkchangeyeareditwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 900,
    height: 500,
    bodyPadding: 5,
    title: 'Перенос конструктивного элемента',
    closeAction: 'destroy',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.TypeWorkSt1Grid'
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
                    xtype: 'textfield',
                    name: 'Work',
                    fieldLabel: 'Вид работы',                  
                    readOnly:true
                },
                {
                    xtype: 'label',
                    text: 'Установите новый год ремонта конструктивного элемента и сохраните изменения.',
                    margin: '10 0 10 10',
                    style: {
                        fontWeight:'bold'
                    }

                },
                {
                    xtype: 'objectcrtypeworkst1grid',
                    flex: 1
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
                                            btn.up('typeworkchangeyeareditwin').close();
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