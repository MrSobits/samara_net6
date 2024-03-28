Ext.define('B4.view.prescription.CancelEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 500,
    bodyPadding: 5,
    itemId: 'prescriptionCancelEditWindow',
    title: 'Решение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypePrescriptionCancel',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.prescription.CancelEditGeneralInfoTab',
        'B4.view.prescription.CancelEditViolCancelTab'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeCancel',
                    fieldLabel: 'Тип решения',
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false,
                    items: B4.enums.TypePrescriptionCancel.getItems()
                },
                {
                    xtype: 'tabpanel',
                    padding: 0,
                    border: false,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    frame: true,
                    itemId: 'prescriptionCancelEditTabPanel',
                    items: [
                        {
                            // Таб панель "Основная информация"
                            xtype: 'canceleditgeneralinfotab',
                            border: false
                        },
                        {
                            // Таб панель "Отмененные нарушения" или "Продление срока исполнения нарушений"
                            xtype: 'canceleditviolcanceltab',
                            bodyStyle: 'background-color:transparent;'
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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