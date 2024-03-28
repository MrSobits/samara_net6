Ext.define('B4.view.dpkrdocument.FileUploadWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.dpkrdocumentfileuploadwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    title: 'Загрузка файла',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 5px; padding: 5px 5px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" ' +
                        'style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        'Выберите файл с данными для загрузки. Допустимые типы файлов: .xls, .xlsx</span>'
                },
                {
                    xtype: 'form',
                    margin: '10 5 5 5',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'importForm',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileImport',
                                    fieldLabel: 'Файл',
                                    labelWidth: 40,
                                    allowBlank: false,
                                    editable: false,
                                    labelAlign: 'right',
                                    flex: 1,
                                    itemId: 'fileImport'
                                }
                            ]
                        },
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    margin: '10 5 5 5',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Загрузить'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ],


        });
        me.callParent(arguments);
    }
});