Ext.define('B4.view.import.DecisionProtocolImportPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт протоколов решений',
    alias: 'widget.decisionprotocolimportpanel',
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
            defaults: {
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: xlsx.</span>'
                },
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
                        labelWidth: 45
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'FileImport',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            flex: 1,
                            itemId: 'fileImport',
                            possibleFileExtensions: 'xlsx'
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
                    boxLabel: 'Перевод действующего на доме протокола решения в начальный статус',
                    name: 'TransferToDefaultState',
                    style: 'margin-left: 10px; margin-top: 20px;',
                    listeners: {
                        render: {
                            fn: function () {
                                var me = this;

                                Ext.create('Ext.tip.ToolTip', {
                                    target: me.getEl().getAttribute("id"),
                                    trackMouse: true,
                                    width: 300,
                                    html: 'При выставлении данного параметра и выполнении импорта актуального протокола, у существующего на доме протокола решения статус изменится на начальный'
                                });
                            }
                        }
                    }
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