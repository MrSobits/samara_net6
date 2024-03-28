Ext.define('B4.view.politicauth.ReceptionPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    autoScroll: true,
    title: 'Время приема',
    itemId: 'politicAuthReceptionPanel',
    layout: 'anchor',
    frame: true,
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.button.Save',
        'B4.view.Control.GkhWorkModeGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Прием граждан',
                    store: 'politicauth.ReceptionCitizens',
                    itemId: 'politicAuthRecepCitsGrid'
                },
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Прием юр.лиц',
                    store: 'politicauth.ReceptionJurPerson',
                    itemId: 'politicAuthRecepJurGrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});
