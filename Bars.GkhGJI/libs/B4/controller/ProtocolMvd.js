Ext.define('B4.controller.ProtocolMvd', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ProtocolMvd',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['ProtocolMvd'],
    stores: ['ProtocolMvd'],
    views: [
        'protocolmvd.MainPanel',
        'protocolmvd.FilterPanel',
        'protocolmvd.AddWindow',
        'protocolmvd.Grid'
    ],

    mainView: 'protocolmvd.MainPanel',
    mainViewSelector: 'protocolMvdPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolMvdPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#protocolMvdAnnexGrid',
            controllerName: 'ProtocolMvdAnnex',
            name: 'protocolMvdAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * аспект прав доступа
            */
            xtype: 'protocolmvdperm'
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'protocolMvdStateTransferAspect',
            gridSelector: '#protocolMvdGrid',
            stateType: 'gji_document_protocolmvd',
            menuSelector: 'protocolMvdGridStateMenu'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'protocolMvdButtonExportAspect',
            gridSelector: '#protocolMvdGrid',
            buttonSelector: '#protocolMvdGrid #btnExport',
            controllerName: 'ProtocolMvd',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок протокола МВД с формой добавления и Панелью редактирования,
            открывающейся в боковой вкладке

            Тут мы перекрываем метод onSaveSuccess потому, что тут при добавлении необходимо подменить переменную Inspection
            поскольку после добавления там будет json объекта а нам нужен Id типа int
            Это сделано исключительно для работы с открывающейся панелью, которой нужно передать Id инспекции
            */
            xtype: 'gkhgrideditformaspect',
            name: 'protocolMvdGridWindowAspect',
            gridSelector: '#protocolMvdGrid',
            editFormSelector: '#protocolMvdAddWindow',
            storeName: 'ProtocolMvd',
            modelName: 'ProtocolMvd',
            editWindowView: 'protocolmvd.AddWindow',
            controllerEditName: 'B4.controller.protocolmvd.Navigation',
            otherActions: function (actions) {
                actions['#protocolMvdFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#protocolMvdFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#protocolMvdFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#protocolMvdFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('ProtocolMvd');
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
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var store = form.down('[name=TypeExecutant]').store;
                    form.down('[name=TypeExecutant]').setValue(store.getAt(0).get('Value'));
                }
            }
        }
    ],

    init: function () {
        this.getStore('ProtocolMvd').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('protocolMvdPanel');
        
        if(!me.params){
            me.params = {};
            me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
            me.params.dateEnd = new Date();
            me.params.realityObjectId = null;
        }
        
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ProtocolMvd').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.realityObjectId = this.params.realityObjectId;
        }
    }
});