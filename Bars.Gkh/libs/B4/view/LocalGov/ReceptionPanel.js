Ext.define('B4.view.localgov.ReceptionPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    autoScroll: true,
    title: 'Время приема',
    itemId: 'localGovReceptionPanel',
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
                    store: 'localgov.ReceptionCitizens',
                    itemId: 'receptionCitizensGrid'
                },
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Прием юр.лиц',
                    store: 'localgov.ReceptionJurPerson',
                    itemId: 'receptionJurGrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});
