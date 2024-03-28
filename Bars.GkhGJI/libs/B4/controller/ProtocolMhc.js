Ext.define('B4.controller.ProtocolMhc', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ProtocolMhc',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['ProtocolMhc'],
    stores: ['ProtocolMhc'],
    views: [
        'protocolmhc.MainPanel',
        'protocolmhc.FilterPanel',
        'protocolmhc.AddWindow',
        'protocolmhc.Grid'
    ],

    mainView: 'protocolmhc.MainPanel',
    mainViewSelector: 'protocolMhcPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolMhcPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#protocolMhcAnnexGrid',
            controllerName: 'ProtocolMhcAnnex',
            name: 'protocolMhcAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            * аспект прав доступа
            */
            xtype: 'protocolmhcperm'
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'protocolMhcStateTransferAspect',
            gridSelector: '#protocolMhcGrid',
            stateType: 'gji_document_protocolmhc',
            menuSelector: 'protocolMhcGridStateMenu'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'protocolMhcButtonExportAspect',
            gridSelector: '#protocolMhcGrid',
            buttonSelector: '#protocolMhcGrid #btnExport',
            controllerName: 'ProtocolMhc',
            actionName: 'Export'
        },
        {
            
            xtype: 'gkhgrideditformaspect',
            name: 'protocolMhcGridWindowAspect',
            gridSelector: '#protocolMhcGrid',
            editFormSelector: '#protocolMhcAddWindow',
            storeName: 'ProtocolMhc',
            modelName: 'ProtocolMhc',
            editWindowView: 'protocolmhc.AddWindow',
            controllerEditName: 'B4.controller.protocolmhc.Navigation',
            otherActions: function (actions) {
                actions['#protocolMhcFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#protocolMhcFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#protocolMhcFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#protocolMhcFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('ProtocolMhc');
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
        this.getStore('ProtocolMhc').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('protocolMhcPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date();
        me.params.realityObjectId = null;
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ProtocolMhc').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.realityObjectId = this.params.realityObjectId;
        }
    }
});