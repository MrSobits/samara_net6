Ext.define('B4.view.administration.executionaction.actionwithparams.BindKPKRandDPKRAction', {
    extend: 'Ext.form.Panel',
    border: false,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 230
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Только основные программы',
                                    name: 'IsMain',
                                    flex: 1
                                },
                                //{
                                //    xtype: 'checkbox',
                                //    fieldLabel: 'Принудительная загрузка обновления',
                                //    name: 'ForceDownload',
                                //    flex: 1
                                //}
                            ]
                        },
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});