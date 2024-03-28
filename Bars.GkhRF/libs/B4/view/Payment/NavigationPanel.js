Ext.define('B4.view.payment.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.Control.MenuTreePanel',
        'B4.store.RealityObject',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],
    closable: true,
    storeName: 'payment.NavigationMenu',
    title: 'Оплата КР',
    itemId: 'paymentNavigationPanel',
    layout: {
        type: 'border'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'paymentInfoLabel'
                },
                {
                    id: 'paymentMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: this.storeName,
                    margins: '0 2 0 0'
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    id: 'paymentMainTab',
                    enableTabScroll: true
                }
            ]
        });

        me.callParent(arguments);
    }
});