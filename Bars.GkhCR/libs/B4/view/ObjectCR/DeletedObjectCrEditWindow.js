Ext.define('B4.view.objectcr.DeletedObjectCrEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.deletedobjectwin',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod',
        'B4.view.objectcr.TypeWorkCrHistoryGridForDeleted'
    ],

    modal: true,
    closable: false,
    width: 840,
    minWidth: 840,
    height: 400,
    minHeight: 400,
    title: 'Удаленный объект КР',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '10 0 10 0',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'RealityObject',
                                    fieldLabel: 'Объект недвижимости',
                                    textProperty: 'Address',
                                    readOnly: true,
                                    labelWidth: 150,
                                    width: 500,
                                    anchor: '100%'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'BeforeDeleteProgramCr',
                                    fieldLabel: 'Программа КР',
                                    textProperty: 'Name',
                                    readOnly: true,
                                    width: 320,
                                    anchor: '100%'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'typeworkhistorygridfordeleted',
                    flex: 1
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
                                    btn.up('window').destroy();
                                }
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});