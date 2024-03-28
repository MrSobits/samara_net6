Ext.define('B4.view.publicservorg.ContractMainInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractmaininfopanel',

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.view.realityobj.Grid',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.enums.ResOrgReason',
        'B4.store.publicservorg.ByPublicServOrg',
        'B4.view.publicservorg.ContractServiceGrid'
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

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'ResOrgReason',
                    displayField: 'Display',
                    valueField: 'Value',
                    items: B4.enums.ResOrgReason.getItemsWithEmpty([null, ' - ']),
                    fieldLabel: 'Основание',
                    allowBlank: false,
                    editable: false,
                    flex: 1
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContractNumber',
                            fieldLabel: 'Номер',
                            labelWidth: 150,
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'ContractDate',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            maxWidth: 150,
                            labelWidth: 50
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelAlign: 'right',
                        flex: 1,
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            labelWidth: 150,
                            allowBlank: false
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания',
                            labelWidth: 170
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Файл',
                    labelWidth: 150,
                    labelAlign: 'right'
                },
                {
                    xtype: 'textarea',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    height: 60,
                    labelWidth: 150,
                    maxLength: 300
                },
                {
                    xtype: 'publicservorgcontractservicegrid',
                    height: 300
                }
            ]
        });

        me.callParent(arguments);
    }
});