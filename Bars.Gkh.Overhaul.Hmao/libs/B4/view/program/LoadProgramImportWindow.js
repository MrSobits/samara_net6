Ext.define('B4.view.program.LoadProgramImportWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.loadprogramimportwin',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 400,
    bodyPadding: 5,
    title: 'Импорт',
    trackResetOnLoad: true,
    resizable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: csv.<br>Максимальный размер файлов 10Mb</span>'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    fieldLabel: 'Файл',
                    labelWidth: 80,
                    itemId: 'fileImport'
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