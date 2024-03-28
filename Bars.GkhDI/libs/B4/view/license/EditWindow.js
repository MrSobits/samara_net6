Ext.define('B4.view.license.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.disinfolicenseeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    width: 500,
    height: 250,
    bodyPadding: 5,
    trackResetOnLoad: true,
    title: 'Добавление/редактирование лицензии',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'LicenseNumber',
                    fieldLabel: 'Номер Лицензии',
                    maxLength: 200,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateReceived',
                    fieldLabel: 'Дата получения',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'LicenseOrg',
                    fieldLabel: 'Орган выдавший лицензию',
                    maxLength: 300,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            labelWidth: 150,
                            labelAlign: 'right',
                            editable: false,
                            name: 'LicenseDoc',
                            fieldLabel: 'Документ лицензии',
                            possibleFileExtensions: 'odt,ods,odp,doc,docx,xls,xlsx,ppt,tif,tiff,pptx,txt,dat,jpg,jpeg,png,pdf,gif,rtf',
                            maxFileSize: 15*1024*1024
                        },
                        {
                            xtype: 'label',
                            width: '340',
                            html: '<div style="padding-left:150px">'
                                + '* Допустимые расширения файлов: .odt, .ods, .odp, .doc, .docx, .xls, .xlsx, .ppt, .tif, .tiff, .pptx, .txt, .dat, .jpg, .jpeg, .png, .pdf, .gif, .rtf'
                                + '<div/>'
                        },
                        {
                            xtype: 'label',
                            html: '<div style="padding-left:150px">'
                                + '* Максимальный размер файла: 15Мб'
                                + '<div/>'
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
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
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
