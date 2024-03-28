Ext.define('B4.aspects.GkhDigitalSignatureGridAspect', {
    extend: 'B4.base.Aspect',
    alias: 'widget.gkhdigitalsignaturegridaspect',
    
    requires: [
        'B4.view.PdfWindow'
    ],

    gridSelector: null,
    
    controllerName: null,
    
    idProperty: null,
    
    dataAction: 'getdata',
    
    pdfAction: 'getpdf',
    
    signatureCreated: false,

    init: function(controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.gridSelector] = {
            'itemclick': {
                fn: this.itemClick,
                scope: this
            }
        };

        actions[this.gridSelector + ' b4signbutton'] = {
            'click': {
                fn: this.signBtnClick,
                scope: this
            }
        };

        actions['pdfWindow'] = {
            createsignature: {
                fn: this.onCreateSignature,
                scope: this
            },
            close: {
                fn: this.onPdfWindowClose,
                scope: this
            }
        };

        controller.control(actions);
    },
    
    getGrid: function () {
        return this.controller.getGrid();
    },

    signBtnClick: function() {
        this.prepareData();
    },
    
    itemClick: function (row, record) {
        this.getGrid().down('b4signbutton').enable();
        this.setId(record.get(this.idProperty || 'Id'));
    },
    
    getId: function() {
        return this.controller.entityId;
    },
    
    setId: function(id) {
        this.controller.entityId = id;
    },
    
    getDataToSign: function() {
        return this.controller.dataToSign;
    },
    
    setDataToSign: function(data) {
        this.controller.dataToSign = data;
    },
    
    setDataIds: function(data) {
        this.controller.data = data;
    },
    
    getDataIds: function() {
        return this.controller.data;
    },
    
    setSignatureCreated: function(val) {
        this.controller.signatureCreated = val;
    },
    
    getSignatureCreated: function() {
        return this.controller.signatureCreated === true;
    },
    
    prepareData: function() {
        var id = this.getId();
        var me = this,
            grid = me.getGrid(),
            dataUrl = B4.Url.action(Ext.String.format("/{0}/{1}?Id={2}", this.controllerName, this.dataAction, id));

        if (grid.mask) {
            grid.mask('Подготовка данных...', Ext.getBody());
        }
        B4.Ajax.request({
            url: dataUrl,
            params: { Id: id },
            timeout: 9999999
        }).next(function(resp) {
            var txt = resp.responseText,
                decoded = Ext.decode(txt);

            if (!decoded.success) {
                B4.QuickMsg.msg('Ошибка!', decoded.message, 'error');
                grid.unmask();
                return;
            }
            me.setDataToSign(decoded.data.dataToSign);
            me.setDataIds(decoded.data);

            grid.unmask();

            var pdfUrl = B4.Url.action(Ext.String.format("/{0}/{1}?pdfId={2}", me.controllerName, me.pdfAction, decoded.data.pdfId));
            me.showFrame(pdfUrl);
        }).error(function(e) {
            B4.QuickMsg.msg('Ошибка!', 'Не удалось получить данные для подписания!', 'error');
        });
    },
    
    showFrame: function(url) {
        var pdfWindow = Ext.create('widget.pdfWindow', {
            layout: 'fit',
            items: [
                {
                    xtype: 'component',
                    autoEl: {
                        tag: 'iframe',
                        style: 'height: 100%; width: 100%; border: none',
                        src: url
                    }
                }
            ],
            constrain: true
        });

        this.getGrid().up().getActiveTab().add(pdfWindow);

        pdfWindow.show();
    },
    
    onCreateSignature: function (win) {
        var me = this,
            id = this.getId();
        
        var certField = win.down('combo'),
            val = certField.getValue();
        
        if (!val) {
            Ext.Msg.alert('Внимание!', 'Не выбран сертификат!');
            return false;
        }

        try {
            var cert = Crypto.getCertificate(Crypto.getCurrentUserMyStore(), Crypto.constants.findType.CAPICOM_CERTIFICATE_FIND_SHA1_HASH, val);

            var certBase64 = cert.Export(Crypto.constants.encodingType.CAPICOM_ENCODE_BASE64);

            me.checkCertificate(certBase64).next(function () {
                var sign = Crypto.sign(me.getDataToSign(), cert);

                var url = B4.Url.action(Ext.String.format('/{0}/sign', me.controllerName));

                var data = me.getDataIds();

                me.getGrid().mask('Подписывание', me.getGrid());
                B4.Ajax.request({
                    url: url,
                    params: {
                        Id: id,
                        xmlId: data.xmlId,
                        pdfId: data.pdfId,
                        sign: sign,
                        certificate: certBase64
                    }
                }).next(function () {
                    B4.QuickMsg.msg('Сообщение', 'Паспорт успешно подписан', 'success');
                    me.setSignatureCreated(true);
                    me.getGrid().unmask();
                    me.getGrid().getStore().load();
                    Ext.ComponentQuery.query('pdfWindow')[0].close();
                }).error(function (e) {
                    B4.QuickMsg.msg('Сообщение', 'Ошибка при подписании: ' + (e.message || e), 'error');
                    me.getGrid().unmask();
                    Ext.ComponentQuery.query('pdfWindow')[0].close();
                });
            }).error(function(resp) {
                B4.QuickMsg.msg('Ошибка', 'Ошибка проверки корретности сертификата:' + (resp.message || resp), 'error');
                Ext.ComponentQuery.query('pdfWindow')[0].close();
            });
        } catch(e) {
            B4.QuickMsg.msg('Ошибка', e, 'error');
            Ext.ComponentQuery.query('pdfWindow')[0].close();
        }
    },
    
    onPdfWindowClose: function () {
        this.deleteTmpDocs();
    },
    
    deleteTmpDocs: function() {
        var me = this,
            data = me.getDataIds();

        if (me.getSignatureCreated() === false && data) {
            B4.Ajax.request({
                url: B4.Url.action(Ext.String.format('/{0}/deletedocs', this.controllerName)),
                params: {
                    xmlId: data.xmlId,
                    pdfId: data.pdfId
                }
            });
            me.setDataIds(null);
        }
    },
    
    checkCertificate: function(cert) {
        return B4.Ajax.request({
            url: B4.Url.action('/certificate/validate'),
            params: {
                certificate: cert
            }
        });
    }
});