Ext.define('B4.controller.dict.GkuTarifGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.GkuTarifGji'],
    stores: ['dict.GkuTarifGji'],
    views: ['dict.gkutarif.EditWindow', 'dict.gkutarif.Grid'],

    mainView: 'dict.gkutarif.Grid',
    mainViewSelector: 'gkutarifGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'gkutarifGrid'
        }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия таблицы тарифов ЖКУ с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'gkutarifGridWindowAspect',
            gridSelector: 'gkutarifGrid',
            editFormSelector: '#gkutarifEditWindow',
            storeName: 'dict.GkuTarifGji',
            modelName: 'dict.GkuTarifGji',
            editWindowView: 'dict.gkutarif.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('gkutarifGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.GkuTarifGji').load();
    }
});