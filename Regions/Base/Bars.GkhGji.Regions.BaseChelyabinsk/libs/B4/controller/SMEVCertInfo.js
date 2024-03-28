Ext.define('B4.controller.SMEVCertInfo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    smevCertInfo: null,
  
    models: [
        'smev.SMEVCertInfo',
        'smev.SMEVCertInfoFile'
    ],
    stores: [
        'smev.SMEVCertInfoFile',
        'smev.SMEVCertInfo'
    ],
    views: [

        'smevcertinfo.Grid',
        'gisGkh.SignWindow',
        'smevcertinfo.EditWindow',
        'smevcertinfo.FileInfoGrid'

    ],

    aspects: [
          
        {
            xtype: 'grideditwindowaspect',
            name: 'smevcertinfoGridAspect',
            gridSelector: 'smevcertinfogrid',
            editFormSelector: '#smevCertInfoEditWindow',
            storeName: 'smev.SMEVCertInfo',
            modelName: 'smev.SMEVCertInfo',
            editWindowView: 'smevcertinfo.EditWindow',
            //roId: null,
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {

            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevCertInfo = record.getId();
                    var grid = form.down('smevcertinfofileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevCertInfo', record.getId());
                }
            }
        }
    ],

    mainView: 'smevcertinfo.Grid',
    mainViewSelector: 'smevcertinfogrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevcertinfogrid'
        },
        {
            ref: 'smevcertinfoFileInfoGrid',
            selector: 'smevcertinfofileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevcertinfogrid actioncolumn[action="openpassport"]': { click: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    sendRequest: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        smevCertInfo = rec.getId();
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVCertInfoExecute'),
            params: {
                taskId: smevCertInfo
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVCertInfo').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVCertInfo').load();
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevcertinfogrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVCertInfo').load();
    }
});