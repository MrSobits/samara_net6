Ext.define('B4.view.realityobj.decision_protocol.ProtocolEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.protocoledit',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.realityobj.decision_protocol.DecisionGrid',
        'B4.enums.YesNo',
        'B4.form.EnumCombo'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    width: 1000,
    bodyPadding: 5,
    height: 300,
    title: 'Сведения о помещениях',
    closeAction: 'hide',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'form',
                    height: 210,
                    defaults: {
                        margin: '5 5 5 5',
                        anchor: '100%',
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    name: 'Id'
                                },
                                {
                                    xtype: 'hiddenfield',
                                    name: 'RealityObject'
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Номер',
                                    name: 'DocumentNum',
                                    flex: 1,
                                    labelWidth: 98
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата',
                                    name: 'ProtocolDate',
                                    flex: 1,
                                    labelWidth: 80
                                },
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Файл протокола',
                                    name: 'File',
                                    flex: 3,
                                    labelWidth: 150 
                                }
                            ]
                        },
                        {
                            xtype: 'textareafield',
                            fieldLabel: 'Описание',
                            name: 'Description'
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Количественные характеристики',
                            defaults: {
                                flex: 1
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'numberfield',
                                        margin: '5 5 5 5',
                                        hideTrigger: true,
                                        labelWidth: 240,
                                        width: 320,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Количество голосов всего (кв. м.)',
                                            name: 'VotesTotalCount'
                                        },
                                        {
                                            fieldLabel: 'Количество голосов участвовало (кв. м.)',
                                            name: 'VotesParticipatedCount'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'numberfield',
                                        margin: '5 5 5 5',
                                        hideTrigger: true,
                                        labelWidth: 200,
                                        width: 300,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Наличие кворума',
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.YesNo',
                                            includeEmpty: false,
                                            enumItems: [],
                                            name: 'HasQuorum',
                                            hideTrigger: false
                                        },
                                        {
                                            fieldLabel: 'Доля принявших участие (%)',
                                            name: 'ParticipatedShare'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    defaults: {
                                        xtype: 'numberfield',
                                        margin: '5 5 5 5',
                                        hideTrigger: true,
                                        labelWidth: 205,
                                        width: 305,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Количество голосов "За" (кв. м.)',
                                            name: 'PositiveVotesCount'
                                        },
                                        {
                                            fieldLabel: 'Доля принявших решение (%)',
                                            name: 'DecidedShare'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
                //,
                //{
                //    flex: 1,
                //    xtype: 'protodecisiongrid',
                //    closable: false,
                //    disabled: true
                //}
            ]
            ,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' },
                                { xtype: 'button', text: 'Решения', action: 'nskdecision'}
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });


        me.callParent(arguments);
        //me.down('protodecisiongrid').getStore().on('beforeload', me.onBeforeDecisionStoreLoad, me);
        //me.down('protodecisiongrid').on('render', function (grid) {
        //    grid.getStore().load();
        //});
    },

    onBeforeDecisionStoreLoad: function (store, operation) {
        var me = this,
            protocolId = me.down('hiddenfield[name=Id]').getValue();

        operation.params = operation.params || {};

        operation.params.protocolId = protocolId;
    }
});