Ext.define('B4.view.infoaboutusecommonfacilities.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения об использовании мест общего пользования',
    itemId: 'infoAboutUseCommonFacilitiesEditPanel',
    layout: 'fit',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.infoaboutusecommonfacilities.GridPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'infusecommonfacgridpanel'
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
                                    itemId: 'addInfoAboutUseCommonFacilitiesButton'
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
