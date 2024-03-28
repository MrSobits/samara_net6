Ext.define('B4.view.dict.contractservice.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.contractserviceeditwindow',
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.TypeCommunalResource',
        'B4.enums.ManagementContractServiceType',
        'B4.enums.WorkAssignment',
        'B4.enums.TypeWork',
        'B4.store.dict.UnitMeasure'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    minWidth: 600,
    height: 300,
    minHeight: 300,
    maxHeight: 600,
    bodyPadding: 5,
    title: 'Форма добавления / редактирования услуг',

    _COM: 0,
    _ADD: 1,
    _AGR: 2,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    height: 120,
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'container',
                            defaults: { padding: '5 0 5 0' },
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    name: 'Id'
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                        allowBlank: false,
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            minValue: 1,
                                            allowDecimals: false,
                                            name: 'Code',
                                            fieldLabel: 'Код услуги'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'ServiceType',
                                            fieldLabel: 'Вид услуги',
                                            enumName: 'B4.enums.ManagementContractServiceType',
                                            includeEmpty: false,
                                            enumItems: [],
                                            listeners: {
                                                change: me.onServiceTypeChange,
                                                scope: me
                                            }
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'UnitMeasure',
                                            fieldLabel: 'Единица измерения',
                                            anchor: '100%',
                                            store: 'B4.store.dict.UnitMeasure',
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            allowBlank: false,
                                            fieldLabel: 'Наименование'
                                        }
                                    ]
                                },

                                //коммунальные услуги
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    serviceType: me._COM,
                                    child: true,
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'CommunalResource',
                                            fieldLabel: 'Вид коммунального ресурса',
                                            enumName: 'B4.enums.TypeCommunalResource',
                                            includeEmpty: false,
                                            allowBlank: false,
                                            hideTrigger: false
                                        },
                                        {
                                            fieldLabel: 'Услуга предоставляется на ОДН',
                                            xtype: 'checkbox',
                                            name: 'IsHouseNeeds'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    serviceType: me._COM,
                                    child: true,
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'SortOrder',
                                            allowBlank: true,
                                            flex: 1,
                                            fieldLabel: 'Порядок сортировки'
                                        }
                                    ]
                                },
                               
                                //Услуга/Работа по ДУ
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    serviceType: me._AGR,
                                    child: true,
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'WorkAssignment',
                                            fieldLabel: 'Назначение работ',
                                            enumName: 'B4.enums.WorkAssignment',                                  
                                            includeEmpty: false,
                                            allowBlank: false,
                                            hideTrigger: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        flex: 1
                                    },
                                    serviceType: me._AGR,
                                    child: true,
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'TypeWork',
                                            fieldLabel: 'Тип работ',
                                            enumName: 'B4.enums.TypeWork',
                                            allowBlank: false,
                                            includeEmpty: false,
                                            hideTrigger: false
                                        }
                                    ]
                                }                               
                            ]
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
    },

    onServiceTypeChange: function (combo, newVal, oldVal) {   
        var me = this,
            model,
            win = combo.up('contractserviceeditwindow'),
            form = win.getForm(),
            record = form.getRecord(),
            serviceType = newVal,          
            communal = Ext.ComponentQuery.query(Ext.String.format('container[serviceType={0}]', me._COM), win),
            additional = Ext.ComponentQuery.query(Ext.String.format('container[serviceType={0}]', me._ADD), win),
            agreement = Ext.ComponentQuery.query(Ext.String.format('container[serviceType={0}]', me._AGR), win),
            containers = Ext.ComponentQuery.query('container[child=true]', win);

        Ext.each(communal, function (item) {
            item.setDisabled(newVal !== me._COM);
        }, me);

        Ext.each(additional, function (item) {
            item.setDisabled(newVal !== me._ADD);
        }, me);

        Ext.each(agreement, function (item) {
            item.setDisabled(newVal !== me._AGR);
        }, me);

        Ext.each(containers, function (item) {
            item.setVisible(newVal === item.serviceType);
        }, me);

        if (oldVal != newVal) {
            if (newVal == me._COM) {
                model = b4app.getController('dict.contractservice.ManagementContractService').getModel('dict.contractservice.CommunalContractService');             
            } else if (newVal == me._ADD) {
                model = b4app.getController('dict.contractservice.ManagementContractService').getModel('dict.contractservice.AdditionalContractService');
            } else if (newVal == me._AGR) {
                model = b4app.getController('dict.contractservice.ManagementContractService').getModel('dict.contractservice.AgreementContractService');
            }

            record = new model(record.raw);
            record.set('ServiceType', serviceType);
            form.loadRecord(record);
        }
    }
});