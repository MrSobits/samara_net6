Ext.define('B4.controller.ResolPros', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ResolPros',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['ResolPros'],
    stores: ['ResolPros'],
    views: [
        'resolpros.MainPanel',
        'resolpros.FilterPanel',
        'resolpros.AddWindow',
        'resolpros.Grid'
    ],

    mainView: 'resolpros.MainPanel',
    mainViewSelector: 'resolProsPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'resolProsPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#resolProsAnnexGrid',
            controllerName: 'ResolProsAnnex',
            name: 'resolProsAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * аспект прав доступа
            */
            xtype: 'resolprosperm'
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'resolProsStateTransferAspect',
            gridSelector: '#resolProsGrid',
            stateType: 'gji_document_resolpros',
            menuSelector: 'resolProsGridStateMenu'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'resolProsButtonExportAspect',
            gridSelector: '#resolProsGrid',
            buttonSelector: '#resolProsGrid #btnExport',
            controllerName: 'ResolPros',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок постановления прокуратуры с формой добавления и Панелью редактирования,
            открывающейся в боковой вкладке

            Тут мы перекрываем метод onSaveSuccess потому, что тут при добавлении необходимо подменить переменную Inspection
            поскольку после добавления там будет json объекта а нам нужен Id типа int
            Это сделано исключительно для работы с открывающейся панелью, которой нужно передать Id инспекции
            */
            xtype: 'gkhgrideditformaspect',
            name: 'resolProsGridWindowAspect',
            gridSelector: '#resolProsGrid',
            editFormSelector: '#resolProsAddWindow',
            storeName: 'ResolPros',
            modelName: 'ResolPros',
            editWindowView: 'resolpros.AddWindow',
            controllerEditName: 'B4.controller.resolpros.Navigation',
            otherActions: function (actions) {
                actions['#resolProsFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this} };
                actions['#resolProsFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this} };
                actions['#resolProsFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this} };
                actions['#resolProsFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this} };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('ResolPros');
                str.currentPage = 1;
                str.load();

            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },
            onChangeRealityObject: function (field, newValue, oldValue) {
                if (this.controller.params && newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                aspect.getForm().close();
                
                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);

                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                    },
                    scope: this
                });
            }
        }
    ],

    init: function () {
        this.getStore('ResolPros').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('resolProsPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date();
        me.params.realityObjectId = null;
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ResolPros').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.realityObjectId = this.params.realityObjectId;
        }
    }
});