Ext.define('B4.view.system.VersionInfoPanel', {
    extend: 'Ext.panel.Panel',
    
    title: 'Версия системы',
    alias: 'widget.versioninfopanel',
    layout: {
        type: 'form'
    },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    width: 420,
                    items: [
                        {
                            xtype: 'form',
                            bodyPadding: '7 5 2 5',
                            bodyStyle: 'background: none repeat scroll 0 0 #eaf2fb',
                            defaults: {
                                xtype: 'textfield',
                                readOnly: true,
                                labelWidth: 120,
                                width: 400,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    fieldLabel: 'Версия приложения',
                                    name: 'AppVersion'
                                },
                                {
                                    fieldLabel: 'Схема',
                                    name: 'Branch'
                                },
                                {
                                    fieldLabel: 'Регион',
                                    name: 'Region'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата сборки',
                                    name: 'BuildDate',
                                    format: 'd.m.Y H:i:s'
                                },
                                {
                                    fieldLabel: 'Номер сборки',
                                    name: 'BuildNumber'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
