Ext.define('B4.view.manorg.MembershipEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 650,
    minWidth: 500,
    bodyPadding: 5,
    itemId: 'manorgMembershipEditWindow',
    title: 'Членство в объединении',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 250
                },
                {
                    xtype: 'textfield',
                    name: 'Address',
                    fieldLabel: 'Адрес',
                    maxLength: 1000
                },
                {
                    xtype: 'textfield',
                    name: 'OfficialSite',
                    fieldLabel: 'Сайт',
                    maxLength: 250
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNum',
                    fieldLabel: 'Номер свидетельства о членстве',
                    maxLength: 50
                },
                {
                    xtype: 'container',
                    margin: '-2 0 0 0',
                    layout: {
                        padding: '0 0 5 0',
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        width: 290,
                        labelWidth: 200,
                        flex: 1,
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания'
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