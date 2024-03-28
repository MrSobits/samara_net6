Ext.define('B4.view.dict.municipalitytree.MunicipalityFiasOktmoEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.municipalityfiasoktmoeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Добавление населенного пункта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.FiasPlace',
        'B4.model.Fias'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'FiasGuid',
                    fieldLabel: 'Населенный пункт',
                    allowBlank: false,
                    editable: false,
                    store: 'B4.store.FiasPlace',
                    model: 'B4.model.Fias',
                    textProperty: 'OffName',
                    idProperty: 'AOGuid',
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
                    xtype: 'numberfield',
                    name: 'Oktmo',
                    fieldLabel: 'OKTMO',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    minValue: 0,
                    minLength: 11,
                    maxLength: 11
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