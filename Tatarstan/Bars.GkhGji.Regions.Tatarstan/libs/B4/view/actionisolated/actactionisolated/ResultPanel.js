Ext.define('B4.view.actionisolated.actactionisolated.ResultPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.actactionisolatedresultpanel',
    
    layout: 'fit',
    itemId: 'actActionIsolatedResultPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.actcheck.RealityObjectGrid',
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'actCheckRealityObjectGrid',
                    itemId: 'actActionIsolatedRealityObjectGrid',
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
                                            xtype: 'b4addbutton'
                                        },
                                        {
                                            xtype: 'b4updatebutton'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'actcheck.RealityObject',
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});