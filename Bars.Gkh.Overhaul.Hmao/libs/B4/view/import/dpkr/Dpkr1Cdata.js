Ext.define('B4.view.import.dpkr.Dpkr1Cdata', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт ДПКР из 1С',
    alias: 'widget.dpkr1cdataimport',
    layout: 'form',
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
                        type: 'vbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'container',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимый формат: csv. Вручную скорректировать привязку населенных пукнтов к МО возможно в реестре Администрирование / ОКТМО / Привязка населенных пунктов <br> </span>'
                        },
                        {
                            defaults: {
                                labelAlign: 'left',
                                labelWidth: 40
                            },
                            margin: '15 15 15 15',
                            layout: {
                                type: 'hbox'
                            },
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileImport',
                                    fieldLabel: 'Файл',
                                    allowBlank: false,
                                    flex: 1,
                                    itemId: 'fileImport',
                                   // possibleFileExtensions: 'xslx',
                                    width: 400
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить',
                                    tooltip: 'Загрузить',
                                    iconCls: 'icon-accept',
                                    itemId: 'loadIdIsButton'
                                }
                            ]
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
