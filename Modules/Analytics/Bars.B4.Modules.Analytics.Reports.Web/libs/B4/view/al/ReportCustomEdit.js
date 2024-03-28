Ext.define('B4.view.al.ReportCustomEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.reportcustomedit',
    width: 500,
    layout: 'form',
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.ReportCustomSelect'
    ],
    modal: true,
    title: 'Редактирование шаблона',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                labelAlign: 'right',
                margin: 7
            },
            items: [
             {
                 xtype: 'b4selectfield',
                 store: 'B4.store.ReportCustomSelect',
                 name: 'CodedReportKey',
                 editable: false,
                 fieldLabel: 'Базовый отчет',
                 textProperty: 'Name',
                 idProperty: 'Key',
               //  isGetOnlyIdProperty: true,
                 columns: [
                     { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                 ]
             },
           
                {
                    xtype: 'hidden',
                    margin: 0,
                    name: 'Template'
                },
                {
                    xtype: 'b4filefield',
                    name: 'TemplateFile',
                    fieldLabel: 'Файл шаблона',
                    allowBlank: false,
                    flex: 1,
                    editable: false,
                    possibleFileExtensions: 'mrt',
                    getFileUrl: function (id) {
                        return B4.Url.content(Ext.String.format('{0}/{1}?CodedReportKey={2}', 'CodedReportManager', 'DownloadTemplate', id));
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-disk',
                                    text: 'Скачать',
                                    menu: [
                                        {
                                            text: 'Скачать оригинал',
                                            action: 'DownloadOrigin',
                                            iconCls: 'icon-script-lightning',
                                            disabled: true
                                        },
                                        {
                                            text: 'Скачать пустой шаблон',
                                            iconCls: 'icon-script',
                                            action: 'DownloadEmpty',
                                            disabled: true
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                         {
                                             xtype: 'button',
                                             text: 'Редактировать в онлайн-редакторе',
                                             iconCls: 'icon-page-edit',
                                             action: 'OpenStimulDesigner'
                                         }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
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
            ]
        });

        me.callParent(arguments);
    }
});