Ext.define('B4.view.version.EditPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.programversioneditpanel',
    title: 'Версия программы',
    requires: [
        'B4.ux.button.Save',
        'B4.view.version.RecordsGrid',
        'B4.store.version.ProgramVersion',
        'B4.form.SelectField',
        'B4.view.version.RecordsGrid',
        'B4.view.version.PublicationGrid',
        'B4.view.version.SubsidyRecordGrid',
        'B4.view.version.CorrectionGrid',
        'B4.view.version.ActualizationLogGrid',
        'B4.view.version.ActualizationFileLoggingGrid',
        'B4.view.version.OwnerDecisionGrid'
    ],

    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    bodyPadding: 5,

    items: [
        {
            xtype: 'container',
            layout: {
                type: 'hbox'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Version',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                    store: 'B4.store.version.ProgramVersion',
                    fieldLabel: 'Версия программы',
                    editable: false,
                    allowBlank: false,
                    labelAlign: 'right',
                    labelWidth: 170,
                    margin: '0 10 0 0',
                    flex:1
            },
                {
                    xtype: 'buttongroup',
                    items: [
                        {
                            text: 'Переход',
                            itemId: 'btnredirect',
                            iconCls: 'icon-arrow-out',
                            flex: 1,
                            height: 20,
                            width: 100,
                            menu: {
                                xtype: 'menu',
                                items: [
                                    {
                                        text: 'Перейти к родительской версии',
                                        itemId: 'btnredirecttoparent',
                                        action: 'redirecttoparent',
                                        iconCls: 'icon-arrow-out',
                                        parentVersionId: null
                                    }
                                ]
                            }
                        },
                    ]
                },
            ]
        },
        {
            xtype: 'checkbox',
            boxLabel: 'Утвержденная',
            name: 'IsMain',
            labelWidth: 170,
            margin: '5 0 10 60',
            boxLabelAlign: 'before'
        },  
        {
            xtype: 'fieldset',
            title: 'Параметры отображения записей в версии',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Основная версия',
                            name: 'ShowMainVersion',
                            margin: '0 0 0 50',
                            boxLabelAlign: 'before',
                            tip: 'Показать ООИ версии программы',
                            listeners: {
                                render: function (c) {
                                    Ext.create('Ext.tip.ToolTip', {
                                        target: c.getEl(),
                                        html: c.tip
                                    });
                                }
                            }
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Подпрограмма',
                            name: 'ShowSubVersion',
                            margin: '0 0 0 60',
                            boxLabelAlign: 'before',
                            tip: 'Показать ООИ подпрограммы',
                            listeners: {
                                render: function (c) {
                                    Ext.create('Ext.tip.ToolTip', {
                                        target: c.getEl(),
                                        html: c.tip
                                    });
                                }
                            }
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Показать удаленные',
                            name: 'ShowNoShowing',
                            margin: '0 0 0 60',
                            boxLabelAlign: 'before'
                        },

                    ]
                },
            ]
        },
        {
            xtype: 'tabpanel',
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'versionrecordsgrid',
                    flex: 1,
                    border: 0
                },
                {
                    xtype: 'panel',
                    title: 'Субсидирование',
                    type: 'subsidy',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                            {
                                xtype: 'versionsubsidyrecordgrid',
                                flex: 1,
                                border: 0
                            }
                    ]
                },
                {
                    xtype: 'panel',
                    title: 'Результат корректировки',
                    type: 'correction',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                            {
                                xtype: 'versioncorrectiongrid',
                                flex: 1,
                                border: 0
                            }
                    ]
                },
                {
                    xtype: 'panel',
                    title: 'Опубликованная программа',
                    type: 'publication',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                            {
                                xtype: 'versionpublicationgrid',
                                flex: 1,
                                border: 0
                            }
                    ]
                },
                {
                    xtype: 'panel',
                    title: 'Логи актуализации',
                    type: 'actualizationlog',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'actualizationloggrid',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    title: 'Файловое логирование актуализации',
                    type: 'actualizationfilelogging',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'actualizationfilelogginggrid',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    title: 'Решения собственников',
                    type: 'ownerDecision',
                    margins: -1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'versionownerdecisiongrid',
                            flex: 1
                        }
                    ]
                }
            ]
        }
    ]
});