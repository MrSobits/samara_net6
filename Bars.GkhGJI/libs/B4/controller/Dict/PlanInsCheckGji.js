Ext.define('B4.controller.dict.PlanInsCheckGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.PlanInsCheck'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.PlanInsCheckGji'],
    stores: ['dict.PlanInsCheckGji'],
    views: ['dict.planinscheckgji.EditWindow', 'dict.planinscheckgji.Grid'],

    mainView: 'dict.planinscheckgji.Grid',
    mainViewSelector: 'planInsCheckGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'planInsCheckGjiGrid'
        }
    ],

    aspects: [
        { xtype: 'planinscheckperm' },
        {
            /*
            Аспект взаимодействия таблицы планов проверки юр. лиц и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'planInsCheckGjiGridWindowAspect',
            gridSelector: 'planInsCheckGjiGrid',
            editFormSelector: '#planInsCheckGjiEditWindow',
            storeName: 'dict.PlanInsCheckGji',
            modelName: 'dict.PlanInsCheckGji',
            editWindowView: 'dict.planinscheckgji.EditWindow'
        }
    ],
    
    index: function () {
        var view = this.getMainView() || Ext.widget('planInsCheckGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PlanInsCheckGji').load();
    }
});