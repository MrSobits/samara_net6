Ext.define('B4.view.administration.ExportableTypesWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.exportabletypeswindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    height: 480,
    bodyPadding: 5,
    maximizable: true,
    modal: true,
    closeAction: 'destroy',
    title: 'Интеграция с ЖКХ.Комплекс',

    requires: [
        'B4.store.dict.PersAccGroup',
        'B4.view.administration.ExportableTypesGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'exportabletypesgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
    
});