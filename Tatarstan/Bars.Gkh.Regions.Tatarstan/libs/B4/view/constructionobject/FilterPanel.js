Ext.define('B4.view.constructionobject.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.constructionobjfilterpanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'constructionObjFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfResettlementProgram',
                    itemId: 'tfResettlementProgram',
                    fieldLabel: 'Программа переселения',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfMunicipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные районы',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }
});