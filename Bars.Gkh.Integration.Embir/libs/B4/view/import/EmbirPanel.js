Ext.define('B4.view.import.EmbirPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField',
        'B4.view.import.EmbirLogGrid'
    ],

    title: 'Импорт из ЕМБИР',
    alias: 'widget.importembirpanel',
    layout: { type: 'vbox', align: 'stretch'},
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this,
            importStore = Ext.create('Ext.data.Store', {
                autoLoad: false,
                fields: ['Key', 'Name', 'PossibleFileExtensions'],
                proxy: {
                    autoLoad: false,
                    type: 'ajax',
                    url: B4.Url.action('GetImportList', 'GkhImport', { codeImport: "ImportEmbir" }),
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                }
            });
        
        Ext.applyIf(me, {
            items: [
            {
                xtype: 'form',
                border: false,
                bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 100,
                    bodyPadding: 5
                },
                items: [
                    {
                        xtype: 'fieldset',
                        layout:  'hbox',
                        padding: '0 5 0 0',
                        flex: 1,
                        title: 'Синхронизация и импорт данных',
                        items: [
                            {
                                xtype: 'combobox',
                                name: 'ImportName',
                                fieldLabel: 'Импорт',
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 100,
                                store: importStore,
                                valueField: 'Key',
                                displayField: 'Name'
                            },
                            {
                                xtype: 'component',
                                width: 10
                            },
                            {
                                xtype: 'button',
                                text: 'Загрузить',
                                action: 'import',
                                readOnly: true,
                                tooltip: 'Загрузить',
                                iconCls: 'icon-accept'
                            }]
                    },
                    {
                        xtype: 'component',
                        width: 10
                    },
                    {
                        xtype: 'fieldset',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        flex: 1,
                        title: 'Дата последней синхронизации',
                        items: [
                            {
                                xtype: 'container',
                                flex: 1,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    xtype: 'datefield',
                                    labelWidth: 100,
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    readOnly: true
                                },
                                items: [
                                    {                                              
                                        name: 'ImportRealityObjectEmbir',
                                        fieldLabel: 'Импорт домов'
                                    },
                                    {
                                        name: 'ImportPersonalAccountEmbir',
                                        fieldLabel: 'Импорт лиц. счетов'
                                    },
                                    {
                                        name: 'ImportContragentEmbir',
                                        fieldLabel: 'Импорт контрагентов'
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                flex: 1,
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    xtype: 'datefield',
                                    labelWidth: 100,
                                    format: 'd.m.Y',
                                    labelAlign: 'right',
                                    readOnly: true
                                },
                                items: [
                                    {
                                        name: 'ImportWallMaterialEmbir',
                                        fieldLabel: 'Материал стен'
                                    },
                                    {
                                        name: 'ImportOrgFormEmbir',
                                        fieldLabel: 'Тип юр. лица'
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            {
                xtype: 'embirimportloggrid',
                flex: 1
            }
            ]
        });

        me.callParent(arguments);
    }
});
