Ext.define('B4.view.baseactivitytsj.NavigationForm', {
    extend: 'B4.form.Window',

    requires: [
        'B4.view.baseactivitytsj.NavigationPanel'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    maximized: true,
    itemId: 'baseActivityTsjNavigationForm',
    title: 'Деятельность ТСЖ и ЖСК',
    closeAction: 'hide',
    trackResetOnLoad: true,
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                { xtype: 'baseActivityTsjNavigationPanel', closable: false }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {   xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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