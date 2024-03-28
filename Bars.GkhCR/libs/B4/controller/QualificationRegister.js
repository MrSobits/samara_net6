Ext.define('B4.controller.QualificationRegister', {
    /*
    * Контроллер реестра Сметы кр
    */
    extend: 'B4.base.Controller',
    params: null,
    requires:
    [
        'B4.aspects.GridEditWindow',
         'B4.Ajax',
        'B4.Url',
        'B4.aspects.ButtonDataExport'
    ],

    models:
    [
        'ObjectCr',
        'QualificationRegister'
    ],

    stores: ['QualificationRegister'],

    views:
    [
        'qualificationregister.Grid'
    ],


    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'qualificationregister.Grid',
    mainViewSelector: 'qualificationRegisterGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'qualificationRegisterGrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'qualificationButtonExportAspect',
            gridSelector: 'qualificationRegisterGrid',
            buttonSelector: 'qualificationRegisterGrid #btnQualificationExport',
            controllerName: 'Qualification',
            actionName: 'Export'
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'qualificationRegisterGridEditWindowAspect',
            gridSelector: 'qualificationRegisterGrid',
            storeName: 'QualificationRegister',
            modelName: 'QualificationRegister',
            editRecord: function(rec) {
                var me = this;

                var portal, model;
                if (rec != undefined) {
                    var objCrId = rec.get('Id');

                    if (objCrId) {
                        Ext.History.add('objectcredit/' + objCrId + '/qualification');
                    }

                }
            }
        }
    ],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget('qualificationRegisterGrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.params = {};
        me.getStore('QualificationRegister').load();
    }
});