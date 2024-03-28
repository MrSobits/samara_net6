Ext.define('B4.controller.ProtocolRSO', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ProtocolRSO',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['ProtocolRSO'],
    stores: ['ProtocolRSO'],
    views: [
        'protocolrso.MainPanel',
        'protocolrso.FilterPanel',
        'protocolrso.AddWindow',
        'protocolrso.Grid'
    ],

    mainView: 'protocolrso.MainPanel',
    mainViewSelector: 'protocolRSOPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolRSOPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#protocolRSOAnnexGrid',
            controllerName: 'ProtocolRSOAnnex',
            name: 'protocolRSOAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * аспект прав доступа
            */
            xtype: 'protocolrsoperm'
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'protocolRSOStateTransferAspect',
            gridSelector: '#protocolRSOGrid',
            stateType: 'gji_document_protocolrso',
            menuSelector: 'protocolRSOGridStateMenu'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'protocolRSOButtonExportAspect',
            gridSelector: '#protocolRSOGrid',
            buttonSelector: '#protocolRSOGrid #btnExport',
            controllerName: 'ProtocolRSO',
            actionName: 'Export'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'protocolRSOGridWindowAspect',
            gridSelector: '#protocolRSOGrid',
            editFormSelector: '#protocolRSOAddWindow',
            storeName: 'ProtocolRSO',
            modelName: 'ProtocolRSO',
            editWindowView: 'protocolrso.AddWindow',
            controllerEditName: 'B4.controller.protocolrso.Navigation',
            otherActions: function (actions) {
                actions['#protocolRSOFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#protocolRSOFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#protocolRSOFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#protocolRSOFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('ProtocolRSO');
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
                debugger;
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
        this.getStore('ProtocolRSO').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('protocolRSOPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date();
        me.params.realityObjectId = null;
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ProtocolRSO').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.realityObjectId = this.params.realityObjectId;
        }
    }
});