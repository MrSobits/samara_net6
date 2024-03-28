Ext.define('B4.view.regop.owner.RoomAreaShareAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.roomareashareaddwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.view.regop.owner.RoomAreaShareGrid'
    ],

    title: 'Номер квартиры/помещения',

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    width: 500,
    height: 550,
    autoScroll: true,
    closeAction: 'destroy',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    defaults: {
                        margin: '5 5 5 5'
                    },
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    flex: 1,
                    items: [
                        {
                            xtype: 'roomareasharegrid',
                            flex: 1
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Выбрать',
                                    itemId: 'selectBtn'
                                }
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