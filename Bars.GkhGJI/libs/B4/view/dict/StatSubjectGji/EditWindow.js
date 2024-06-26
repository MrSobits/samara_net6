﻿Ext.define('B4.view.dict.statsubjectgji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'statSubjectGjiEditWindow',
    title: 'Тематика',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.dict.statsubjectgji.SubsubjectGrid',
        'B4.store.dict.StatSubjectGji',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'SSTUCode',
                    fieldLabel: 'Код ССТУ',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'SSTUName',
                    fieldLabel: 'Наименование ССТУ',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            itemId: 'cbISSOPR',
                            name: 'ISSOPR',
                            fieldLabel: 'СОПР',
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbTrackAppealCits',
                            margin: '0 0 0 40',
                            name: 'TrackAppealCits',
                            fieldLabel: 'Отслеживать обращение',
                        },
                    ]
                },

                {
                    //грид подтематик
                    xtype: 'statSubjectSubsubjectGrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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