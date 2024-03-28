Ext.define('B4.controller.MVDLivingPlaceRegistration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    smevNDFL: null,
  
    models: [
        'smev.MVDLivingPlaceRegistration',
        'smev.MVDLivingPlaceRegistrationFile'
    ],
    stores: [
        'smev.MVDLivingPlaceRegistration',
        'smev.MVDLivingPlaceRegistrationFile',
        'smev.MVDLivingPlaceRegistrationAnswer'
    ],
    views: [
        'mvdlivingplacereg.Grid',
        'mvdlivingplacereg.EditWindow',
        'mvdlivingplacereg.FileInfoGrid',
        'mvdlivingplacereg.AnswerGrid'
    ],

    mainView: 'mvdlivingplacereg.Grid',
    mainViewSelector: 'mvdlivingplacereggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mvdlivingplacereggrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'mvdlivingplacereggridAspect',
            gridSelector: 'mvdlivingplacereggrid',
            editFormSelector: '#mvdlivingplaceregEditWindow',
            storeName: 'smev.MVDLivingPlaceRegistration',
            modelName: 'smev.MVDLivingPlaceRegistration',
            editWindowView: 'mvdlivingplacereg.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {                 
                    var grid = form.down('mvdlivingplaceregfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevMVD', record.getId());
                }
            }          
        }   
    ],   

    init: function () {
        this.control({
            'mvdlivingplacereggrid actioncolumn[action="openpassport"]': { click: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    sendRequest: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        smevCertInfo = rec.getId();
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'MVDLivingPlaceRegistrationExecute'),
            params: {
                taskId: smevCertInfo
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.MVDLivingPlaceRegistration').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.MVDLivingPlaceRegistration').load();
        });
    },
    /////////////////////
    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('mvdlivingplacereggridAspect').editRecord(this.params);
            this.params = null;
        }
    },
    ////////////////////

    index: function () {
        var view = this.getMainView() || Ext.widget('mvdlivingplacereggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.MVDLivingPlaceRegistration').load();
    }
});