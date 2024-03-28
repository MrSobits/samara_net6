Ext.define('B4.view.licensing.formgovernmentservice.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    resizable: false,

    title: 'Добавить новый',
    closeAction: 'destroy',

    requires: [
        'B4.form.ComboBox',

        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.Quarter',
        'B4.enums.FormGovernmentServiceType'
    ],

    alias: 'widget.formgovernmentserviceaddwindow',

    initComponent: function () {
        var me = this,
            years = [],
            i,
            currentYear = new Date().getFullYear();

        for (i = -1; i < 10; i++) {
            years.push([currentYear + i, currentYear + i]);
        }

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'GovernmentServiceType',
                    fieldLabel: 'Государственная услуга',
                    items: B4.enums.FormGovernmentServiceType.getItems(),
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margin: '0 0 10px 0',
                    defaults: {
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'Year',
                            labelAlign: 'right',
                            labelWidth: 150,
                            fieldLabel: 'Отчётный период',
                            flex: 1,
                            items: years
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'Quarter',
                            items: B4.enums.Quarter.getItems(),
                            flex: 0.7,
                            margin: '0 0 0 15px',
                            editable: false
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