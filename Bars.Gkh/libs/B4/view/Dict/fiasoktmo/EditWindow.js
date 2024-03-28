Ext.define('B4.view.dict.fiasoktmo.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'fiasoktmoEditWindow',
    title: 'Привязка населенного пункта к МО',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.TreeSelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.MunicipalityTree',
        'B4.form.SelectField',
        'B4.store.FiasPlace',
        'B4.model.Fias'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
                labelAlign: 'right'
            },
            items: [
                 {
                     xtype: 'b4selectfield',
                     allowBlank: false,
                     editable: false,
                     itemId: 'sfFiasLevel3',
                     name: 'FiasGuid',
                     fieldLabel: 'Населенный пункт',
                     store: 'B4.store.FiasPlace',
                     model: 'B4.model.Fias',
                     textProperty: 'OffName',
                     idProperty: 'AOGuid',
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
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'fiasMunicipalitiesTrigerField',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выбор муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    allowBlank: false,
                    editable: false
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