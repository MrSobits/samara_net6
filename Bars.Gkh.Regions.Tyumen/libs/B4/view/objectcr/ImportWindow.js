Ext.define('B4.view.objectcr.ImportWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 400,
    bodyPadding: 5,
    itemId: 'importObjectCrWindow',
    title: 'Импорт',
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.store.dict.ProgramCrObj',
        'B4.store.dict.FinanceSource'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 80,
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'ProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа',
                    store: 'B4.store.dict.ProgramCrObj',
                    columns: [
                        { text: 'Программа КР', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'container',
                    name: 'ImportInfo',
                    hidden: true,
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: xls, xlsx.</span>'
                },
                {
                    xtype: 'b4filefield', editable: false,
                    name: 'FileImport',
                    fieldLabel: 'Файл',
                    itemId: 'fileImport'
                },
                {
                    xtype: 'container',
                    itemId: 'containerInfo',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Внимание! В результате импорта будут заменены текущие источники финансирования работ объектов КР в выбранной программе</span>'
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'FinanceCut',
                    textProperty: 'Name',
                    fieldLabel: 'Разрез фин.',
                    store: 'B4.store.dict.FinanceSource',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'checkbox',
                    name: 'HasLinkOverhaul',
                    boxLabel: 'Создать связь с ДПКР',
                    margin: '0 0 0 85'
                },
                {
                    xtype: 'checkbox',
                    name: 'ReplaceObjectCr',
                    boxLabel: 'Заменить объекты кап.ремонта',
                    margin: '0 0 0 85'
                },
                {
                    xtype: 'checkbox',
                    name: 'ReplaceTypeWork',
                    boxLabel: 'Заменить данные по видам работ',
                    margin: '0 0 0 85'
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
