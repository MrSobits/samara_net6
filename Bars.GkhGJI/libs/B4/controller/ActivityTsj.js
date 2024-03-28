Ext.define('B4.controller.ActivityTsj', {
    /*
    * Контроллер реестра деятельность ТСЖ
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ActivityTsj'
    ],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    models: [
        'ActivityTsj'
    ],
    stores: [
        'ActivityTsj'
    ],
    views: [
        'activitytsj.AddWindow',
        'activitytsj.Grid'
    ],

    mainView: 'activitytsj.Grid',
    mainViewSelector: 'activityTsjGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'activityTsjGrid'
        }
    ],

    /*
    * Аспект взаимодействия формы редактирования и таблицы деятельности ТСЖ
    */
    aspects: [
        {
            //аспект прав доступа
            xtype: 'activitytsjperm'
        },
        
        {
            xtype: 'b4buttondataexportaspect',
            name: 'activityTsjButtonExportAspect',
            gridSelector: 'activityTsjGrid',
            buttonSelector: 'activityTsjGrid #btnExport',
            controllerName: 'ActivityTsj',
            actionName: 'Export'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'activityTsjGridWindowAspect',
            gridSelector: 'activityTsjGrid',
            editFormSelector: '#activityTsjAddWindow',
            storeName: 'ActivityTsj',
            modelName: 'ActivityTsj',
            editWindowView: 'activitytsj.AddWindow',
            controllerEditName: 'B4.controller.activitytsj.Navigation',

            otherActions: function(actions) {
                actions[this.editFormSelector + ' #sfTsj'] = {
                    'change': { fn: this.onChangeTsj, scope: this },
                    'beforerender': { fn: this.onBeforeRenderTsj, scope: this }
                };
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта ManagingOrganization
            // передать только Id на сохранение, поскольку если на сохранение уйдет ManagingOrganization целиком,
            //то это поле тоже сохраниться и поля для записи ManagingOrganization будут потеряны
            getRecordBeforeSave: function (record) {

                var manorg = record.get('ManagingOrganization');
                if (manorg && manorg.Id > 0) {
                    record.set('ManagingOrganization', manorg.Id);
                }

                return record;
            },

            //При смене ТСЖ подгружаем ее данные в readOnly поля
            onChangeTsj: function(obj, newValue) {
                var addWindow = this.getForm(),
                    tfJuridicalAddress = addWindow.down('#tfJuridicalAddress'),
                    tfMailingAddress = addWindow.down('#tfMailingAddress'),
                    tfInn = addWindow.down('#tfInn'),
                    tfKpp = addWindow.down('#tfKpp');

                if (!Ext.isEmpty(newValue)) {
                    tfJuridicalAddress.setValue(newValue.ContragentJuridicalAddress);
                    tfMailingAddress.setValue(newValue.ContragentMailingAddress);
                    tfInn.setValue(newValue.ContragentInn);
                    tfKpp.setValue(newValue.ContragentKpp);
                } else {
                    tfJuridicalAddress.setValue(null);
                    tfMailingAddress.setValue(null);
                    tfInn.setValue(null);
                    tfKpp.setValue(null);
                }
            },

            //после рендеринга поля ТСЖ вешаемся на событие beforeload у его стора для показа упр орг с типом ТСЖ или ЖСК
            onBeforeRenderTsj: function(obj) {
                var store = obj.getStore();
                if (store) {
                    store.on('beforeload', this.controller.onBeforeLoad, this, 'ManOrg');
                }
            }
        }
    ],

    onBeforeLoad: function(store, operation, type) {
        operation.params.jskTsjOnly = true;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('activityTsjGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ActivityTsj').load();
    }
});