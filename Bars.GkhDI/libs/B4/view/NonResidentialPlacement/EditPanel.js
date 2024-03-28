Ext.define('B4.view.nonresidentialplacement.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения об использовании нежилых помещений',
    itemId: 'nonResidentialPlacementEditPanel',
    layout: 'fit',

    requires: [
        'B4.view.nonresidentialplacement.GridPanel',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'nonresidplacegridpanel'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'addNonResidentialPlacementButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
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
