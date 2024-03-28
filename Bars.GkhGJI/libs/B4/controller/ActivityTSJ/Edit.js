Ext.define('B4.controller.activitytsj.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel'
    ],

    models: ['ActivityTsj'],

    views: ['activitytsj.EditPanel'],

    mainView: 'activitytsj.EditPanel',
    mainViewSelector: '#activityTsjEditPanel',
    aspects: [
        
        {
            xtype: 'gkheditpanel',
            name: 'activityTsjEditPanelAspect',
            editPanelSelector: '#activityTsjEditPanel',
            modelName: 'ActivityTsj',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #sfTsj'] = {
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
                var editPanel = this.getPanel();

                var tfJuridicalAddress = editPanel.down('#tfJuridicalAddress');
                var tfMailingAddress = editPanel.down('#tfMailingAddress');
                var tfInn = editPanel.down('#tfInn');
                var tfKpp = editPanel.down('#tfKpp');

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
                    store.on('beforeload', this.controller.onBeforeLoad, this);
                }
            }
        }
    ],

    onBeforeLoad: function(store, operation) {
        operation.params.jskTsjOnly = true;
    },

    onLaunch: function() {
        if (this.params) {
            this.getAspect('activityTsjEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});