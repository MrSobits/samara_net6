Ext.define('B4.view.manorglicense.LicenseDocEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorglicensedoceditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 180,
    maxHeight: 180,
    bodyPadding: 5,
    title: 'Документ',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.TypeManOrgTypeDocLicense'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Наименование документа',
                    store: B4.enums.TypeManOrgTypeDocLicense.getStore(),
                    displayField: 'Display',
                    allowBlank: false,
                    flex: 1,
                    valueField: 'Value',
                    name: 'DocType'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocNumber',
                            fieldLabel: 'Номер',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocDate',
                            fieldLabel: 'Дата',
                            allowBlank: false,
                            flex: 1,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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