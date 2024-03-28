Ext.define('B4.controller.dict.ControlType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.form.SelectWindow'
    ],

    mixins:{
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.ControlType',
        'dict.ControlTypeInspectorPos',
        'dict.ControlTypeRiskIndicators'
    ],
    stores: [
        'dict.ControlType',
        'dict.ControlTypeInspectorPos',
        'dict.ControlTypeRiskIndicators'
    ],

    views: [
        'dict.ControlType.Grid',
        'dict.ControlType.EditWindow',
        'dict.ControlType.InspectorPositionsGrid',
        'dict.ControlType.RiskIndicatorsGrid',
    ],

    mainView: 'dict.ControlType.Grid',
    mainViewSelector: 'controltypegrid',

    aspects: [
        /* Аспект прав доступа */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Dict.ControlType.Create', applyTo: 'b4addbutton', selector: 'controltypegrid' },
                {
                    name: 'GkhGji.Dict.ControlType.Delete', applyTo: 'b4deletecolumn', selector: 'controltypegrid',
                    applyBy: function (component, allowed) {
                        this.setVisible(component, allowed);
                    }
                },
                {
                    name: 'GkhGji.Dict.ControlType.Edit', applyTo: 'b4editcolumn', selector: 'controltypegrid',
                    applyBy: function (component, allowed) {
                        this.setVisible(component, allowed);
                    }
                }
            ]
        },
        /* Аспект взаимодействия Таблицы Видов Контроля и формы редактирования */
        {
            xtype: 'grideditwindowaspect',
            name: 'controlTypeGridWindowAspect',
            gridSelector: 'controltypegrid',
            editFormSelector: 'controltypeeditwindow',
            modelName: 'dict.ControlType',
            storeName: 'dict.ControlType',
            editWindowView: 'dict.ControlType.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        recordId = record.getId(),
                        store = asp.getForm().down('controltypeinspectorposgrid').getStore();

                    me.controller.params = me.controller.params || {};
                    me.controller.params.controlTypeId = recordId;

                    store.on('beforeload', me.onBeforeLoad, me);
                    store.load();
                }
            },
            onBeforeLoad: function (store, operation) {
                operation.params.controlTypeId = this.controller.params.controlTypeId;
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            storeName: 'dict.ControlTypeInspectorPos',
            modelName: 'dict.ControlTypeInspectorPos',
            gridSelector: 'controltypeinspectorposgrid',
            saveButtonSelector: '[name=inspPosGridSaveButton]',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this,
                        modifRecords = store.getModifiedRecords();

                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('ControlType', me.controller.params.controlTypeId);
                        }
                    });

                    Ext.Array.each(modifRecords, function (rec) {
                        if(rec.data['InspectorPosition']){
                            rec.data['InspectorPosition'] = rec.data['InspectorPosition'].Id;
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            storeName: 'dict.ControlTypeRiskIndicators',
            modelName: 'dict.ControlTypeRiskIndicators',
            gridSelector: 'controltyperiskindicatorsgrid',
            saveButtonSelector: '[name=RiskIndicatorsSaveButton]',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this;

                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('ControlType', me.controller.params.controlTypeId);
                        }
                    });
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'controltypegrid [name=ControlTypeGuid]': {
                'afterrender': {
                    fn: function (field){
                        me.setGuidField(field, 'ControlTypeId');
                    }
                }},
            'controltypeinspectorposgrid [name=InspectorPosGuid]': {
                'afterrender': {
                    fn: function (field){
                        me.setGuidField(field, 'InspectorPositionId');
                    }
                }},
            'controltyperiskindicatorsgrid' : {
                afterrender: function (cmp) {
                    var store = cmp.getStore();
                    store.on('beforeload', function (s, operation) {
                        operation.params.controlTypeId = me.params.controlTypeId;
                    });
                    store.load();
                }
            },
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    /* Получаем  и вставляем в поле идентифиактор справочника */
    setGuidField: function (field, cfgFieldName) {
        B4.Ajax.request({
            url: B4.Url.action('ListItems', 'GkhConfig'),
            params: {
                parent: 'ErknmIntegrationConfig'
            }
        })
        .next(function (response) {
            var res = Ext.JSON.decode(response.responseText),
                cfgGuid = res.data.find(x => x.id == `ErknmIntegrationConfig.${cfgFieldName}`)?.value;
            field.setValue(cfgGuid);
        });
    }
});