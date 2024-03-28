Ext.define('B4.view.realityobj.ServiceOrgEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjserorgeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 330,
    bodyPadding: 5,
    title: 'Поставщик услуг',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.store.Contragent',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.TypeServiceOrg'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 155,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'combobox', editable: false,
                    name: 'TypeServiceOrg',
                    fieldLabel: 'Тип поставщика услуг',
                    displayField: 'Display',
                    store: B4.enums.TypeServiceOrg.getStore(),
                    valueField: 'Value',
                    itemId: 'cbTypeServOrg'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Organization',
                    fieldLabel: 'Поставщик',
                   

                    store: 'B4.store.Contragent',
                    allowBlank: false,
                    editable: false,
                    itemId: 'sfContragentOrganization',
                    columns: [
                        { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование документа',
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNum',
                    fieldLabel: 'Номер документа',
                    maxLength: 50
                },
                {
                    xtype: 'datefield',
                    width: 250,
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100% -158',
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