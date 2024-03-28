Ext.define('B4.view.administration.executionaction.actionwithparams.SendToRisContractsChartersAction', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.enums.ChartersContractsRisExportType',
        'B4.form.EnumCombo'
    ],

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 230
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип выгрузки',
                                    enumName: 'B4.enums.ChartersContractsRisExportType',
                                    name: 'ChartersContractsRisExportType',
                                    allowBlank: false,
                                    listeners: {
                                        change: function (field, newValue, oldValue) {
                                            var periodField = field.up().down('[name=Period]');
                                            
                                            if(newValue === B4.enums.ChartersContractsRisExportType.ChangedChartersContracts){
                                                periodField.setDisabled(false);
                                            }
                                            else{
                                                periodField.setDisabled(true);
                                            }
                                        },
                                        scope: this
                                    }
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Удалить файлы 4.0 в ГИС МЖФ после выгрузки',
                                    name: 'CleanFiles',
                                    flex: 1
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Период за который внесены изменения (дней)',
                                    name: 'Period',
                                    allowBlank: false,
                                    flex: 1
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