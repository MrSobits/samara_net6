Ext.define('B4.controller.EmailNewsletterController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    emailNewsletterId: null,

    stores: [
        'emailnewsletter.EmailNewsletter',
        'emailnewsletter.EmailNewsletterLog'
    ],
    views: [
        'emailnewsletter.Grid',
        'emailnewsletter.EditWindow',
        'emailnewsletter.LogGrid'
    ],
    models: [
        'emailnewsletter.EmailNewsletter',
        'emailnewsletter.EmailNewsletterLog'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'emailnewslettergrid'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            '#emailNewsletterEditWindow #sendEmailButton': { click: { fn: me.onSendEmailClick, scope: this } },
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('emailnewslettergrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore('emailnewsletter.EmailNewsletter').load();
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'emailNewsletterGridWindowAspect',
            gridSelector: 'emailnewslettergrid',
            editFormSelector: '#emailNewsletterEditWindow',
            storeName: 'emailnewsletter.EmailNewsletter',
            modelName: 'emailnewsletter.EmailNewsletter',
            editWindowView: 'emailnewsletter.EditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.emailNewsletterId = record.getId();
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    if (record.getId()) {
                        asp.controller.emailNewsletterId = record.getId();
                    }
                    
                    var grid = form.down('emailnewsletterLogGrid'),
                        store = grid.getStore(),
                        success = record.get('Success'),
                        sendBtn = form.down('#sendEmailButton'),
                        tfHeader = form.down('#tfHeader'),
                        tfBody = form.down('#tfBody'),
                        tfDestinations = form.down('#tfDestinations'),
                        tfAttachment = form.down('#tfAttachment'),
                        saveBtn = form.down('#saveBtn');

                    if (record.getId()) {
                        asp.controller.emailNewsletterId = record.getId();
                        sendBtn.enable();
                    }

                    if (record.phantom) {
                        sendBtn.disable();
                    }

                    if (success) {
                        sendBtn.disable();
                        tfHeader.disable();
                        tfBody.disable();
                        tfDestinations.disable();
                        tfAttachment.disable();
                        saveBtn.disable();
                    }
                    
                    store.clearFilter(true);
                    store.filter('emailNewsletterId', record.getId());
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'emailNewsletterLogGridInlineAspect',
            storeName: 'emailnewsletter.EmailNewsletterLog',
            modelName: 'emailnewsletter.EmailNewsletterLog',
            gridSelector: '#emailNewsletterLogGrid'
        }
    ],
    onSendEmailClick: function (btn) {
        
        var me = this,
            form = btn.up('#emailNewsletterEditWindow'),
            sendBtn = form.down('#sendEmailButton'),
            tfHeader = form.down('#tfHeader'),
            tfBody = form.down('#tfBody'),
            tfDestinations = form.down('#tfDestinations'),
            tfAttachment = form.down('#tfAttachment'),
            saveBtn = form.down('#saveBtn');

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('SendEmails', 'EmailNewsletter'),
            timeout: 5 * 60 * 1000,
            params: {
                emailNewsletterId: this.emailNewsletterId,
            }
        }).next(function (response) {
            
            sendBtn.disable();
            tfHeader.disable();
            tfBody.disable();
            tfDestinations.disable();
            tfAttachment.disable();
            saveBtn.disable();
            me.unmask();
            var result = Ext.JSON.decode(response.responseText);
            Ext.Msg.alert('Информация', result.message);
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Внимание', e.message || e);
        });
    }
});