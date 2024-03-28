Ext.define('B4.controller.manorglicense.LicenseGis', {
    extend: 'B4.base.Controller',

    requires: [
       
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.YesNo'
    ],

    morgContract: null,
    ownerProtocol: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'manorglicense.LicenseGis',
        'manorglicense.LicensePrescription',
        'manorglicense.LicenseNotificationGis'
    ],

    models: [
        'manorglicense.LicenseGis',
        'manorglicense.LicensePrescription',
        'manorglicense.LicenseNotificationGis'
    ],

    views: [
        'manorglicense.LicenseGridGis',
        'manorglicense.LicenseGisEditWindow',
        'manorglicense.LicensePrescriptionGrid',
        'manorglicense.LicensePrescriptionEditWindow',
        'manorglicense.LicenseNotificationGisGrid',
        'manorglicense.LicenseNotificationGisEditWindow'
    ],

    mainView: 'manorglicense.LicenseGridGis',
    mainViewSelector: 'manorglicensegridgis',

    refs: [
        {
            ref: 'mainView',
            selector: 'manorglicensegridgis'
        }
    ],

    aspects: [
     
        {
            xtype: 'grideditwindowaspect',
            name: 'manorglicenseGisgridWindowAspect',
            gridSelector: 'manorglicensegridgis',
            editFormSelector: 'licensegiseditwindow',
            modelName: 'manorglicense.LicenseGis',
            editWindowView: 'manorglicense.LicenseGisEditWindow',
           listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    morgContract = rec.getId();
                   var grid = form.down('licenseprescriptiongrid'),
                   store = grid.getStore();
                   store.filter('parentId', rec.getId());
                   var notifgrid = form.down('licenseprescriptiongrid'),
                       notifstore = notifgrid.getStore();
                   notifstore.filter('mcid', rec.getId());
                }
            },
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowOnly2andMore'] = { 'change': { fn: this.cbShowOnly2andMore, scope: this } };
                actions[this.gridSelector + ' #showall'] = { 'change': { fn: this.showall, scope: this } };
                actions[this.gridSelector + ' #cbShowEnded'] = { 'change': { fn: this.showEnded, scope: this } };
            },



            cbShowOnly2andMore: function () {
                this.controller.getStore('manorglicense.LicenseGis').load();
            },
            showEnded: function () {
                this.controller.getStore('manorglicense.LicenseGis').load();
            },
            showall: function () {
                this.controller.getStore('manorglicense.LicenseGis').load();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licensePrescriptionGriddWindowAspect',
            gridSelector: 'licenseprescriptiongrid',
            editFormSelector: '#licensePrescriptionEditWindow',
            modelName: 'manorglicense.LicensePrescription',
            editWindowView: 'manorglicense.LicensePrescriptionEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.MorgContractRO = morgContract;
                    }
                }
            }

        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licenseNotificationGisGridWindowAspect',
            gridSelector: 'manorglicensnotificationgisgrid',
            editFormSelector: 'manorglicensenotificationgiseditwindow',
            modelName: 'manorglicense.LicenseNotificationGis',
            editWindowView: 'manorglicense.LicenseNotificationGisEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.ManagingOrgRealityObject = morgContract;
                    }
                },
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                }
            }
        }
        
    ],

    init: function () {
        this.getStore('manorglicense.LicenseGis').on('beforeload', this.onBeforeLoad, this);
        debugger;
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('manorglicensegridgis');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore('manorglicense.LicenseGis').load();
    },

    onBeforeLoad: function (store, operation) {
        var mainView = this.getMainView();
        if (mainView) {
            operation.params.cbShowOnly2andMore = mainView.down('#cbShowOnly2andMore').checked;
            operation.params.showall = mainView.down('#showall').checked;
            operation.params.showEnded = mainView.down('#cbShowEnded').checked;
        }
    },

    getGrid: function () {
        return Ext.ComponentQuery.query('manorglicensegridgis')[0];
    },
});