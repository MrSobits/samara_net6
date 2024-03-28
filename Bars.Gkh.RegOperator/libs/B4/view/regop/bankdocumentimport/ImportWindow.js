Ext.define('B4.view.regop.bankdocumentimport.ImportWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'importWindow',
    title: 'Импорт',
    trackResetOnLoad: true,
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.base.Store',
        'B4.base.Proxy',
        'B4.enums.YesNo'
    ],

    initComponent: function() {
        var me = this,
            providerStore = Ext.create('B4.base.Store', {
                fields: ['Key', 'Name', 'Serializer'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'Import',
                    listAction: 'GetImportProviders'
                }
            }),
            fsStore = Ext.create('B4.base.Store', {
                fields: ['Code', 'Name'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'FsGorodImportInfo',
                    listAction: 'List'
                }
            }),

            filesStore = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'FileName' }
                ],
                proxy: {
                    type: 'memory'
                }
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
            {
                xtype: 'b4combobox',
                editable: false,
                name: 'providerCode',
                valueField: 'Key',
                store: providerStore,
                allowBlank: false,
                fieldLabel: 'Тип импорта',
                listeners: {
                    storebeforeload: function(cmp, st, opts) {
                        opts.params = {
                            typeName: 'PersonalAccountPaymentInfoIn,PaymentInProxy'
                        };
                    },
                    select: function(cmb, recs) {
                        var me = this,
                            win = cmb.up(),
                            fsCombo = win.down('[name="fsGorodCode"]'),
                            record = recs[0];

                        if (record.get('Serializer') === 'fs_gorod') {
                            fsCombo.enable();
                        } else {
                            fsCombo.disable();
                        }
                    }
                }
            },
            {
                xtype: 'b4combobox',
                editable: false,
                name: 'fsGorodCode',
                valueField: 'Code',
                store: fsStore,
                allowBlank: false,
                disabled: true,
                fieldLabel: 'Тип универсального импорта'
            },
            {
                xtype: 'b4combobox',
                editable: false,
                name: 'DistributePenalty',
                fieldLabel: 'Распределение суммы на задолженность по пени',
                store: B4.enums.YesNo.getStore(),
                listeners: {
                    change: function(cmp, newValue) {
                        me.down('[name=DistributePenaltyInfo]').setVisible(newValue === 10);
                    }
                },
                displayField: 'Display',
                valueField: 'Value',
                value: 20
            },
            {
                xtype: 'container',
                name: 'DistributePenaltyInfo',
                hidden: true,
                style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; padding: 5px 10px; line-height: 16px;',
                html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell;">'
                    + '     При выборе значения "Да" в поле "Распределение суммы на задолженность по пени.'
                    + ' сумма в импорте будет распределяться также на задолженность по пени.'
                    + ' Сначала погашается основной долг, остаток суммы идет на погашение задолженности по пени.'
                    + ' Если после этого остается сумма, то она идет на погашение основного долга.'
                    + '</span>'
            },
            {
                xtype: 'b4combobox',
                editable: false,
                name: 'selectOverwrite',
                fieldLabel: 'Перезаписать неподтвержденные оплаты по данному документу',
                store: B4.enums.YesNo.getStore(),
                displayField: 'Display',
                valueField: 'Value',
                value: 20
            },
            {
                xtype: 'displayfield',
                itemId: 'log'
            },
            {
                xtype: 'b4grid',
                name: 'bankdocumentimportfilesgrid',
                title: 'Файлы',
                height: 300,
                store: filesStore,
                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'FileName',
                            flex: 1,
                            text: 'Файл',
                            sortable: false
                        },
                        {
                            xtype: 'b4deletecolumn',
                            scope: me
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'filefield',
                                    name: 'FileImport',
                                    hidden: true,
                                    listeners: {
                                        afterrender: function(field) {
                                            field.fileInputEl.set({
                                                multiple: 'multiple'
                                            });
                                        }
                                    },
                                    itemId: 'fileImport'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Добавить',
                                    textAlign: 'left',
                                    iconCls: 'icon-add',
                                    action: 'AddFiles'
                                },
                                {
                                    xtype: 'container',
                                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; padding: 2px; line-height: 12px;',
                                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. При загрузке файла формата: zip/rar/7z максимальный размер файла 30Мб</span>'
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});