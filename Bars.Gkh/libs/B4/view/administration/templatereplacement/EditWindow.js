Ext.define('B4.view.administration.templatereplacement.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'templateReplacementEditWindow',
    title: 'Замена шаблона',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'button',
                    text: 'Скачать исходный шаблон',
                    itemId: 'btnGetBaseTemplate'
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Код шаблона',
                    name: 'Code',
                    readOnly: true
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Описание',
                    name: 'Description',
                    readOnly: true,
                    itemId: 'taDescription'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Шаблон замены (только .xls, .doc  или mrt)',
                    editable: false,
                    allowBlank: false,
                    getMessage: function(fileName, errorExtension, needExtensions) {
                        var msg = Ext.String.format('Новый шаблон не соответствует формату исходного шаблона. <br>' +
                            'Исходный шаблон имеет расширение .{0} <br> Новый шаблон имеет расширение .{1}', needExtensions, errorExtension);
                        return msg;
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});