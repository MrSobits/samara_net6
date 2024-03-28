Ext.define('B4.view.gisrealestatetype.EditPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.gisrealestatetypeeditpanel',

    requires: [
        'B4.ux.button.Save',
        'Ext.tab.Panel',
        'Ext.form.field.Text',
        'Ext.form.field.Hidden',
        'Ext.form.Panel',
        'B4.form.SelectField',

        'B4.view.gisrealestatetype.CommonParamGrid',
        'B4.view.gisrealestatetype.IndicatorGrid'
    ],

    // Id редактируемого типа дома
    realEstateTypeId: 0,

    title: 'Редактирование типа дома',
    closable: true,

    initComponent: function () {
        var me = this,
            groupStore = Ext.create('B4.store.gisrealestate.realestatetype.RealEstateTypeGroup');

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },
            items: [
                {
                    xtype: 'form',
                    border: 0,
                    defaults: {
                        width: 600,
                        labelWidth: 200,
                        labelAlign: 'right',
                        margin: '5 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'hiddenfield',
                            name: 'Id'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    selectionMode: 'SINGLE',
                                    store: groupStore,
                                    modalWindow: true,
                                    labelWidth: 200,
                                    labelAlign: 'right',
                                    name: 'Group',
                                    flex: 1,
                                    fieldLabel: 'Наименование группы',
                                    editable: false,
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    margin: '0 0 0 10',
                                    iconCls: 'icon-add',
                                    name: 'AddGroup'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false
                        }
                    ],
                    tbar: {
                        items: [
                            {
                                xtype: 'b4savebutton'
                            }
                        ]
                    }
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            title: 'Общие характеристики',
                            xtype: 'giscommonparameditgrid'
                        },
                        {
                            title: 'Индикаторы',
                            xtype: 'gisrealestateindicatorgrid',
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});