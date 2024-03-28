Ext.define('B4.view.manorg.DocumentationEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 450,
    bodyPadding: 5,
    itemId: 'manorgDocumentationEditWindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    margin: '-2 0 0 0',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            labelAlign: 'right',
                            anchor: '100%',
                            flex: 1,
                            allowBlank: false,
                            maxLength: 50
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'от',
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            allowBlank: false,
                            width: 150,
                            labelWidth: 50
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    anchor: '100%'
                },
                {
                    xtype: 'textarea',
                    padding: '10 0 0 0',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100%',
                    maxLength: 500
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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