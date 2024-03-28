Ext.define('B4.controller.dict.EnergyEfficiencyClasses', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'energyefficiencyclassesgrid',
            permissionPrefix: 'Gkh.Dictionaries.EnergyEfficiencyClasses'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'energyEfficiencyClassesGridAspect',
            modelName: 'dict.EnergyEfficiencyClasses',
            gridSelector: 'energyefficiencyclassesgrid',
            listeners: {
                beforesave: function (asp, rec) {
                    var errorProps = [],
                        errorMsg = 'Не заполнены обязательные поля:',
                        modifiedRecs = rec.getModifiedRecords(),
                        anyEmptyName = false,
                        anyEmptyDesignation = false,
                        anyEmptyDeviationValue = false;

                    modifiedRecs.forEach(function (item) {
                        if (item.data.Name.length === 0) {
                            anyEmptyName = true;
                        }
                        if (item.data.Designation.length === 0) {
                            anyEmptyDesignation = true;
                        }
                        if (item.data.DeviationValue.length === 0) {
                            anyEmptyDeviationValue = true;
                        }
                    });

                    if (anyEmptyName) {
                        errorProps.push(' Наименование');
                    }
                    
                    if (anyEmptyDesignation) {
                        errorProps.push(' Обозначение класса');
                    }

                    if (anyEmptyDeviationValue) {
                        errorProps.push(' Величина отклонения значения');
                    }

                    if (errorProps.length > 0) {
                        B4.QuickMsg.msg('Внимание', errorMsg + errorProps, 'warning', 5000);
                        return false;
                    }
                    return true;
                }
            }
        }
    ],
    
    models: ['dict.EnergyEfficiencyClasses'],
    stores: ['dict.EnergyEfficiencyClasses'],
    views: ['dict.energyefficiencyclasses.Grid'],

    mainView: 'dict.energyefficiencyclasses.Grid',
    mainViewSelector: 'energyefficiencyclassesgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'energyefficiencyclassesgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('energyefficiencyclassesgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});