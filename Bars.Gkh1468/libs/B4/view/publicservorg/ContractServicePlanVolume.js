Ext.define('B4.view.publicservorg.ContractServicePlanVolume', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractserviceplanvolumepanel',

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 3,

    closeAction: 'hide',

    closable: false,
    title: 'Плановый объём и режим подачи за год',
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
                    xtype: 'container',
                    padding: '0 5 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'PlanVolume',
                            fieldLabel: 'Плановый объём',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'UnitMeasure',
                            fieldLabel: 'Единица измерения',
                            textProperty: 'Name',
                            store: 'B4.store.dict.UnitMeasure',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'ServicePeriod',
                    fieldLabel: 'Режим подачи',
                    maxLength: 255,
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});