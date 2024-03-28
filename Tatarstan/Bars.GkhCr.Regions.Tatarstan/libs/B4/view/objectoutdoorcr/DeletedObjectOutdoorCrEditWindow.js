Ext.define('B4.view.objectoutdoorcr.DeletedObjectOutdoorCrEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.deletedobjectoutdoorcreditwindow',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.HistoryGrid'
    ],

    modal: true,
    width: 840,
    minWidth: 840,
    height: 400,
    minHeight: 400,
    title: 'Удаленный объект программ благоустройства дворов',

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
                                    name: 'RealityObjectOutdoor',
                                    fieldLabel: 'Объект',
                                    textProperty: 'Name',
                                    readOnly: true,
                                    flex: 1,
                                    labelWidth: 50
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'BeforeDeleteRealityObjectOutdoorProgram',
                                    fieldLabel: 'Программа благоустройства дворов',
                                    textProperty: 'Name',
                                    readOnly: true,
                                    flex: 2,
                                    labelWidth: 230
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'typeworkrealityobjectoutdoorhistorygrid',
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