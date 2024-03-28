Ext.define('B4.view.import.OwnersImportPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт собственников',
    alias: 'widget.ownersimportpanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'importForm',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'AccountCreateDate',
                            //checked: false,
                            allowBlank: false,
                            //margin: '5 0 0 105',
                            labelWidth: 200,
                            
                            fieldLabel: 'Дата открытия лицевого счета',
                            itemId: 'dfAccountCreateDate'
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileImport',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            flex: 1,
                            itemId: 'fileImport',
                            possibleFileExtensions: 'xls'
                        },
                        {
                            xtype: 'button',
                            text: 'Загрузить',
                            tooltip: 'Загрузить',
                            iconCls: 'icon-accept',
                            itemId: 'loadIdIsButton'
                        }
                    ]
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
                }
            ]
        });

        me.callParent(arguments);
    }
});