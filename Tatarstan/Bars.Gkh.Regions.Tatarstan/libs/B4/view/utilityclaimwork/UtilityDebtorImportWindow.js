Ext.define('B4.view.utilityclaimwork.UtilityDebtorImportWindow', {
    extend: 'B4.form.Window',

    requires: [
		'B4.form.FileField'
    ],

    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 200,
    bodyPadding: 5,
    closable: true,

    title: 'Импорт неплательщиков ЖКУ',
    alias: 'widget.utilitydebtorimportwindow',  

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                allowBlank: false,
                labelAlign: 'right',
                labelWidth: 50
            },
            items: [
				{
				    xtype: 'container',
				    itemId: 'ctnText',
				    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
				    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: xls,xlsx.'
				},
				{
				    xtype: 'form',
				    border: false,
				    bodyStyle: Gkh.bodyStyle,
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
						    width: 500,
						    flex: 1,
						    itemId: 'fileImport',
						    possibleFileExtensions: 'xls,xlsx'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Загрузить'
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