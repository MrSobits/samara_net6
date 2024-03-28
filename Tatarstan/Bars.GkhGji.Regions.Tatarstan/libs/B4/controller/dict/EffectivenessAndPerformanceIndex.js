Ext.define('B4.controller.dict.EffectivenessAndPerformanceIndex', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.EffectivenessAndPerformanceIndex'],
    stores: ['dict.EffectivenessAndPerformanceIndex'],

    views: ['dict.effectivenessandperformanceindex.Grid'],

    mainView: 'dict.effectivenessandperformanceindex.Grid',
    mainViewSelector: 'effectivenessandperformanceindexgrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'effectivenessandperformanceindexgrid',
            permissionPrefix: 'GkhGji.Dict.EffectivenessAndPerformanceIndex'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'effectivenessandperformanceindexgrid',
            storeName: 'dict.EffectivenessAndPerformanceIndex',
            modelName: 'dict.EffectivenessAndPerformanceIndex',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validName = true,
                        validCode = true,
                        validParameterName = true,
                        validUnitMeasure = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validName && !me.validate(rec, 'Name')) {
                            validName = false;
                        }
                        if (validCode && !me.validate(rec, 'Code')) {
                            validCode = false;
                        }
                        if (validParameterName && !me.validate(rec, 'ParameterName')) {
                            validParameterName = false;
                        }
                        if (validUnitMeasure && !me.validate(rec, 'UnitMeasure')) {
                            validUnitMeasure = false;
                        }
                    });
                    
                    if (!(validName && validCode && validParameterName && validUnitMeasure)) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validCode ? '' : '<b>Код:</b>' + commonMessagePart) +
                            (validName ? '' : '<b>Наименование показателя:</b>' + commonMessagePart) +
                            (validParameterName ? '' : '<b>Наименование параметра:</b>' + commonMessagePart) +
                            (validUnitMeasure ? '' : '<b>Единица измерения параметра:</b>' + commonMessagePart);

                        Ext.Msg.alert('Ошибка сохранения!', errormessage);
                        return false;
                    }

                    return true;
                }
            },

            validate: function (rec, field) {
                return !Ext.isEmpty(rec.get(field));
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('dict.EffectivenessAndPerformanceIndex').load();
    }
});