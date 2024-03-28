Ext.define('B4.view.regop.cproc.types.PaymentDownloadWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.cprocdownloadfile',

    layout: 'fit',
    modal: true,
    taskId: null,

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            width: 200,
            height: 100,
            style: 'background: none repeat scroll 0 0 #DFE9F6',
            items: [
                {
                    xtype: 'button',
                    text: 'Скачать файл',
                    action: 'DownloadFile',
                    handler: function (b) {
                        B4.Ajax.request({
                            url: B4.Url.action('GetData', 'ComputingProcess', { Id: me.taskId })
                        }).next(function(resp) {
                            var decoded = Ext.JSON.decode(resp.responseText);

                            var fileId = decoded.data || decoded.Data;

                            if (!fileId) {
                                Ext.Msg.alert('Ошибка', "Файл не найден!");
                                return;
                            }

                            b.up('window').close();

                            var frame = Ext.get('paymentIframe');
                            if (frame != null) {
                                Ext.destroy(frame);
                            }

                            Ext.DomHelper.append(document.body, {
                                tag: 'iframe',
                                id: 'paymentIframe',
                                frameBorder: 0,
                                width: 0,
                                height: 0,
                                css: 'display:none;visibility:hidden;height:0px;',
                                src: B4.Url.action('Download', 'FileUpload', { Id: fileId })
                            });
                        }).error(function(err) {
                            Ext.Msg.alert('Ошибка', err.message || Ext.JSON.decode(err.responseText).message || err);
                        });
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});