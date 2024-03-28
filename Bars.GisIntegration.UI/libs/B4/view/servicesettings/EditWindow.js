Ext.define('B4.view.servicesettings.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.servicesettingseditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    itemId: 'servicesettingsEditWindow',
    title: 'Настройки сервиса',
    closeAction: 'hide',

    requires: [
        'B4.form.ComboBox',
        'B4.enums.IntegrationService',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this,
            enumStore = B4.enums.IntegrationService.getStore();

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'IntegrationService',
                    enumName: 'B4.enums.IntegrationService',
                    storeAutoLoad: false,
                    fieldLabel: 'Сервис интеграции',
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false,
                    url: '/ServiceSettings/GetMissedSettings',
                    setValue: function(value) {
                        var idx = enumStore.find(this.displayField, value);
                        if (idx < 0) {
                            idx = enumStore.find(this.valueField, value);
                        }

                        if (idx >= 0) {
                            value = enumStore.getAt(idx);
                        }

                        return Ext.form.ComboBox.prototype.setValue.apply(this, [value]);
                    }
                },
                {
                    xtype: 'textfield',
                    name: 'ServiceAddress',
                    fieldLabel: 'Адрес сервиса',
                    allowBlank: true,
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    name: 'AsyncServiceAddress',
                    fieldLabel: 'Адрес асинхронного сервиса',
                    allowBlank: true,
                    maxLength: 255
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});