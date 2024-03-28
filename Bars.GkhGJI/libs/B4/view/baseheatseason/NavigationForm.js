Ext.define('B4.view.baseheatseason.NavigationForm', {
    extend: 'B4.form.Window',
    alias: 'widget.baseheatseasonnavigationform',
    requires: [
        'B4.view.baseheatseason.NavigationPanel'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    maximized: true,
    itemId: 'baseHeatSeasonNavigationForm',
    title: 'Подготовка к отопительному сезону',
    closeAction: 'hide',
    trackResetOnLoad: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'baseheatseasonnavigationpanel',
                    closable: false
                }
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