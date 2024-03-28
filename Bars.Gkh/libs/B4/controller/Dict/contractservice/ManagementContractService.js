Ext.define('B4.controller.dict.contractservice.ManagementContractService', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.ux.button.Update'
    ],

    models: [
        'dict.contractservice.ManagementContractService',
        'dict.contractservice.AdditionalContractService',
        'dict.contractservice.AgreementContractService',
        'dict.contractservice.CommunalContractService'
    ],

    stores: ['dict.contractservice.ManagementContractService'],

    views: [
        'dict.contractservice.Grid',
        'dict.contractservice.EditWindow'
    ],

    mainView: 'dict.contractservice.Grid',
    mainViewSelector: 'contractservicegrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contractservicegrid'
        },
        {
            ref: 'serviceType',
            selector: 'contractserviceeditwindow b4enumcombo[name=ServiceType]'
        },
        {
            ref: 'contractServiceIdField',
            selector: 'contractserviceeditwindow hiddenfield[name=Id]'
        }
    ],

    aspects: [
         {
             xtype: 'grideditctxwindowaspect',
             name: 'contractServiceCtxWindowAspect',
             gridSelector: 'contractservicegrid',
             editFormSelector: 'contractserviceeditwindow',
             storeName: 'dict.contractservice.ManagementContractService',
             editWindowView: 'dict.contractservice.EditWindow',
             getModel: function (rec) {
                 var me = this,
                     type = rec ? rec.get('ServiceType') : 0;
                 switch (type) {
                     case 0:
                         return me.controller.getModel('dict.contractservice.CommunalContractService');
                     case 1:
                         return me.controller.getModel('dict.contractservice.AdditionalContractService');
                     case 2:
                         return me.controller.getModel('dict.contractservice.AgreementContractService');
                 }
                 return me.callParent(arguments);
             },
             saveRecordHasNotUpload: function (rec) {
                 var me = this;
                 var frm = me.getForm();
                 me.mask('Сохранение', frm);
                 rec.save({ id: rec.getId() })
                     .next(function (result) {
                         me.unmask();
                         me.updateGrid();

                         var model = me.getModel(rec);

                         if (result.responseData && result.responseData.data) {
                             var data = result.responseData.data;
                             if (data.length > 0) {
                                 var id = data[0] instanceof Object ? data[0].Id : data[0];
                                 model.load(id, {
                                     success: function (newRec) {
                                         me.setFormData(newRec);
                                         me.fireEvent('savesuccess', me, newRec);
                                     }
                                 });
                             } else {
                                 me.fireEvent('savesuccess', me);
                             }
                         } else {
                             me.fireEvent('savesuccess', me);
                         }
                     }, this)
                     .error(function (result) {
                         me.unmask();
                         me.fireEvent('savefailure', result.record, result.responseData);

                         Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                     }, this);
             },
             onSaveSuccess: function (asp, record) {
                 var form = asp.getForm();
                 if (form) {
                     form.close();
                     asp.controller.getMainView().getStore().load();
                 }
             },
             editRecord: function (record, editMode) {
                 var me = this,
                     id = record ? record.getId() : null,
                     model;

                 model = this.getModel(record);

                 id ? model.load(id, {
                     success: function (rec) {
                         me.setFormData(rec);
                     },
                     scope: this
                 }) : this.setFormData(new model({ Id: 0 }));

                 this.getForm().getForm().isValid();
                 this.getForm().editMode = editMode;
             },
             listeners: {
                aftersetformdata: function (asp, record, form) {
                    var id = +(asp.controller.getContractServiceIdField().getValue()),
                        serviceTypeField,
                        value,
                        nameField;

                    asp.controller.getServiceType().setDisabled(id > 0);

                    if (record.getId() == 0) {
                        // Если добавляют новую запись проставляем тип услуги=коммунальный
                        serviceTypeField = form.down('b4enumcombo[name="ServiceType"]');
                        value = serviceTypeField.getStore().find('Name', 'Communal');

                        if (value != -1) {
                            serviceTypeField.setValue(value);
                        }
                    }
                }
             }
         }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('contractservicegrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    edit: function (id, serviceType) {
        var me = this,
            view = this.getMainView() || Ext.widget('contractservicegrid');

        if (view && !view.rendered) {
            me.bindContext(view);
            me.application.deployView(view);
        }
        me.getAspect('contractServiceCtxWindowAspect').editRecord(Ext.create('B4.model.dict.contractservice.ManagementContractService', { Id: id, ServiceType: +serviceType }), true);
    }
});