Ext.define('B4.view.publicservorg.ContractServiceMainInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractservicemaininfopanel',

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.dict.Service',
        'B4.enums.SchemeConnectionType',
        'B4.enums.HeatingSystemType'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 3,

    closeAction: 'hide',

    closable: false,
    title: 'Основные сведения',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    trackResetOnLoad: true,
    header: true,
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Service',
                            fieldLabel: 'Услуга',
                            textProperty: 'Name',
                            store: 'B4.store.dict.Service',
                            editable: false,
                            columns: [
                                { text: 'Код', dataIndex: 'Code', flex: 1},
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ],
                            onStoreBeforeLoad: function (store, operation) {
                                var me = this, options = {};
                                options.params = operation.params || {};
                                operation.params.typeGroupServiceDi = 10;
                                me.fireEvent('beforeload', me, options, store);
                                Ext.apply(operation, options);
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'CommunalResource',
                            itemId: 'communalResource',
                            fieldLabel: 'Коммунальный ресурс',
                            textProperty: 'Name',
                            editable: false,
                            store: 'B4.store.dict.CommunalResource',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'HeatingSystemType',
                            items: B4.enums.HeatingSystemType.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Тип системы теплоснабжения',
                            editable: false,
                            flex: 1
                        },
                        {
                            xtype: 'b4combobox',
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'SchemeConnectionType',
                            items: B4.enums.SchemeConnectionType.getItemsWithEmpty([null, ' - ']),
                            fieldLabel: 'Схема присоединения',
                            editable: false,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала предоставления услуги',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания предоставления услуги',
                            format: 'd.m.Y'
                        }
                        
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});