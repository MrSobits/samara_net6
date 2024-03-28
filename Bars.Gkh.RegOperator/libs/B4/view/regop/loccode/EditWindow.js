Ext.define('B4.view.regop.loccode.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.loccodewindow',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.regop.Fias',
        'B4.model.regop.Fias',
        'B4.form.TreeSelectField',
        'B4.store.dict.MunicipalityTree',
        'B4.store.dict.municipality.MoArea'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 625,
    height: 180,
    bodyPadding: 5,
    title: 'Редактирование',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                }
            },
            items: [
                {
                    defaults: {
                        margins: '6 6 6 6',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'FiasLevel1',
                            itemId: 'sfMunicipality',
                            fieldLabel: 'Муниципальный район',
                            store: 'B4.store.dict.municipality.MoArea',
                            editable: false,
                            allowBlank: false,
                            labelWidth: 180,
                            columns: [
                                { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'label',
                            html: '<div class="icon-link" style="width: 16px; height: 16px; vertical-align: middle;"></div>',
                            margins: '10 3 0 3'
                        },
                        {
                            xtype: 'textfield',
                            maxLength: 2,
                            allowBlank: false,
                            name: 'CodeLevel1',
                            fieldLabel: 'Код',
                            labelWidth: 30
                        }
                    ]
                },
                {
                    defaults: {
                        margins: '6 6 6 6',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'treeselectfield',
                            name: 'FiasLevel2',
                            itemId: 'sfFiasLevel2',
                            fieldLabel: 'Муниципальное образование',
                            editable: false,
                            titleWindow: 'Выбор муниципального образования',
                            store: 'B4.store.dict.MunicipalityTree',
                            allowBlank: false,
                            labelWidth: 180
                        },
                        {
                            xtype: 'label',
                            html: '<div class="icon-link" style="width: 16px; height: 16px; vertical-align: middle;"></div>',
                            margins: '10 3 0 3'
                        },
                        {
                            xtype: 'textfield',
                            maxLength: 2,
                            allowBlank: false,
                            name: 'CodeLevel2',
                            fieldLabel: 'Код',
                            labelWidth: 30
                        }
                    ]
                },
                {
                    defaults: {
                        margins: '6 6 6 6',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            allowBlank: false,
                            itemId: 'sfFiasLevel3',
                            isGetOnlyIdProperty: false,
                            name: 'FiasLevel3',
                            fieldLabel: 'Поселение',
                            store: 'B4.store.regop.Fias',
                            model: 'B4.model.regop.Fias',
                            idProperty: 'AOGuid',
                            textProperty: 'FormalName',
                            labelWidth: 180,
                            columns: [
                                {
                                    header: 'Тип',
                                    flex: 1,
                                    dataIndex: 'ShortName',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                },
                                {
                                    header: 'Наименование',
                                    flex: 3,
                                    dataIndex: 'FormalName',
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'label',
                            html: '<div class="icon-link" style="width: 16px; height: 16px; vertical-align: middle;"></div>',
                            margins: '10 3 0 3'
                        },
                        {
                            xtype: 'textfield',
                            maxLength: 2,
                            allowBlank: false,
                            name: 'CodeLevel3',
                            fieldLabel: 'Код',
                            labelWidth: 30
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