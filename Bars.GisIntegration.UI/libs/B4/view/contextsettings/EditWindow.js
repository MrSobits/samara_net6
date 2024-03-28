Ext.define('B4.view.contextsettings.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.contextsettingseditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    itemId: 'contextsettingsEditWindow',
    title: 'Настройки подсистемы',
    closeAction: 'hide',

    requires: [
        'B4.form.ComboBox',
        'B4.enums.FileStorageName',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this,
            enumStore = B4.enums.FileStorageName.getStore();

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'FileStorageName',
                    enumName: 'B4.enums.FileStorageName',
                    storeAutoLoad: false,
                    fieldLabel: 'Наименование подсистемы',
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false,
                    url: '/ContextSettings/GetMissedSettings',
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
                    name: 'Context',
                    fieldLabel: 'Контекст',
                    allowBlank: false,
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