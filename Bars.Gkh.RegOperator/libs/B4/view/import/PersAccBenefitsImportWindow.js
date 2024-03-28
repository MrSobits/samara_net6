Ext.define('B4.view.import.PersAccBenefitsImportWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField'
    ],

    title: 'Импорт начисленных льгот',
    alias: 'widget.persaccbenefitsimportwin',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,
    width: 400,

    initComponent: function () {
        var me = this,
            periodStore = Ext.create('B4.store.regop.ChargePeriod');
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                allowBlank: false,
                flex:1
            },
            items: [
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите период и импортируемые данные. Допустимые типы файлов: dbf.</span>'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Period',
                    fieldLabel: 'Период',
                    labelWidth: 60,
                    columns: [
                        {
                            dataIndex: 'Name',
                            text: 'Наименование',
                            flex: 1
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'StartDate',
                            text: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'EndDate',
                            text: 'Дата окончания',
                            format: 'd.m.Y'
                        }
                    ],
                    store: periodStore
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
                        labelWidth: 60
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'FileImport',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            flex: 1,
                            itemId: 'fileImport',
                            possibleFileExtensions: 'dbf',
                            disabled: true
                        }
                    ]
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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