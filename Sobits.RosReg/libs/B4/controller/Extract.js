Ext.define('B4.controller.Extract', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.enums.ExtractType'
    ],

    models: ['Extract'],
    stores: ['Extract'],
    views: [
        'Extract.Grid'
    ],
    parentId: null,
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'Extract.Grid',
    mainViewSelector: 'extractgrid',
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'extractGridWindowAspect',
            gridSelector: 'extractgrid',
            editFormSelector: '#extractEditWindow',
            storeName: 'Extract',
            modelName: 'Extract',
            editWindowView: 'Extract.EditWindow',
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    parentId = rec.getId();
                    extractId = rec.raw.ExtractId.Id;
                }
            }
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'extractgrid'
        }
    ],
    init: function() {
        var me = this,
            actions = [];

        actions['extractgrid actioncolumn[action=getExtract]'] = { click: me.getExtract, scope: me }

        me.control(actions);
        me.callParent(arguments);
    },
    getExtract: function (grid,row,col,p1,p2,record,asp) {
        var asp = this,
            rec = record.getId();

        //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
        B4.Ajax.request({
            url: B4.Url.action('DownloadExtract', 'ExtractActions'),
            params: {
                Id: rec
            }
        }).next(function (resp) {
            var tryDecoded;

            //asp.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;
            if (tryDecoded.success == false) {
                throw new Error(tryDecoded.message);
            }
            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });
            }
        }).error(function (err) {
            //asp.unmask();
            Ext.Msg.alert('Ошибка', err.message);
        });
    },
    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('extractgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});