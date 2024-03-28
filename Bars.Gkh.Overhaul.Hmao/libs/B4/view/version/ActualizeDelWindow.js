Ext.define('B4.view.version.ActualizeDelWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.versionactualizedelwindow',
    
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.version.ActualizeDelGrid'
    ],
    
    title: 'Удаление лишних записей',
    modal: true,
    width: 800,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,
    closeAction: 'destroy',
    bodyPadding: 5,
    
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            items: [
                {
                    xtype: 'versionactualizedelgrid',
                    flex: 1,
                    border: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});
