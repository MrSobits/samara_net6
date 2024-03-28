Ext.define('B4.view.import.realityobj.CommonRoImportPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт жилых домов (универсальный)',
    alias: 'widget.commonroimportpanel',
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
                    xtype: 'checkbox',
                    name: 'CreateObjects',
                    checked: false,
                    margin: '5 0 0 105',
                    boxLabel: 'Создавать новые дома при импорте',
                    itemId: 'chbCreateObjects'
                },
                {
                    xtype: 'checkbox',
                    name: 'AllowEmptyStreets',
                    checked: false,
                    margin: '5 0 0 105',
                    boxLabel: 'Разрешить создавать дома с пустыми улицами',
                    itemId: 'chbAllowEmptyStreets'
                },
                {
                    xtype: 'checkbox',
                    name: 'OverrideExistRecords',
                    checked: false,
                    margin: '5 0 0 105',
                    boxLabel: 'Заменять существующие сведения',
                    itemId: 'chbOverExRecs'
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