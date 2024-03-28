Ext.define('B4.aspects.GkhGjiSignatureAndSendToEdmGridAspect', {
    extend: 'B4.aspects.GkhGjiNestedDigitalSignatureGridAspect',
    alias: 'widget.gkhgjisignatureandsendtoedmgridaspect',

    requires: [
        'B4.view.PdfWindow'
    ],

    sendedToEdm: false,
    ansRecord: null,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.gridSelector] = {
            'itemclick': {
                fn: this.itemClick,
                scope: this
            }
        };

        actions[this.gridSelector + ' #signAndSendToEdm'] = {
            'click': {
                fn: this.signBtnClick,
                scope: this
            }
        };

        actions['pdfWindow'] = {
            //createsignature: {
            //    fn: this.onCreateSignature,
            //    scope: this
            //},
            close: {
                fn: this.onPdfWindowClose,
                scope: this
            }
        };
        controller.control(actions);
    },

    signBtnClick: function () {
        var executor = this.ansRecord.get('Executor'),
            signer = this.ansRecord.get('Signer'),
            addressee = this.ansRecord.get('Addressee'),
            answerContent = this.ansRecord.get('AnswerContent'),
            typeAppealAnswer = this.ansRecord.get('TypeAppealAnswer'),
            documentNumber = this.ansRecord.get('DocumentNumber'),
            description = this.ansRecord.get('Description'),
            file = this.ansRecord.get('File'),
            documentDate = this.ansRecord.get('DocumentDate');

        if (executor == null || signer == null || addressee == null || answerContent == null || typeAppealAnswer == 0 || documentNumber == null || description == null || description == '' || file == null || documentDate == null) {
            Ext.Msg.alert('Внимание!', 'Перед отправкой в СЭД необходимо указать следующие поля: исполнитель, подписант, адресат, тип ответа, дата и номер доумента, файл pdf');
        }
        else {
            this.prepareData();
        }
    },

    itemClick: function (row, record) {
        var t = this.getGrid();
        this.ansRecord = record;
        var sended = record.get('SendedToEdm');
        var file = record.get('File' || 'FileDoc');
        var addresse = record.get('Addressee');

        if (t != null) {
            if (sended || addresse != 'Заявитель' || file == null) {
                try {
                    t.down('#signAndSendToEdm').disable();
                } catch (e) {
                    var f = e;
                }
            }
            else {
                try {
                    t.down('#signAndSendToEdm').enable();
                } catch (e) {
                    var f = e;
                }
            }
        }
        this.setId(record.get(this.idProperty || 'Id'));
    },

    onCreateSignature: function (win) {
        var me = this,
            id = this.getId(),
            grid = me.getGrid(),
            certField = win.down('combo'),
            val = certField.getValue();

        if (!val) {
            Ext.Msg.alert('Внимание!', 'Не выбран сертификат!');
            return false;
        }

        me.getGrid().mask('Подписывание', me.getGrid());

        var certData = me.getCertData(val);

        certData.then(
            function (result) {
                var subjectName = result.subjectName,
                    serialNumber = result.serialNumber,
                    validFromDate = result.validFromDate,
                    validToDate = result.validToDate;

                B4.Ajax.request({
                    url: B4.Url.action("GetPdfHash", me.controllerName),
                    params: {
                        Id: id,
                        SubjectName: subjectName,
                        SerialNumber: serialNumber,
                        ValidFromDate: validFromDate,
                        ValidToDate: validToDate
                    },
                    timeout: 9999999
                }).next(function (resp) {
                    var txt = resp.responseText,
                        decoded = Ext.decode(txt);
                    if (!decoded.success) {
                        B4.QuickMsg.msg('Ошибка!', decoded.message, 'error');
                        grid.unmask();
                        return;
                    }
                    me.setDataToSign(decoded.data.dataToSign);
                    me.setDataIds(decoded.data.xmlId);

                    var signature = me.signHash(me.getDataToSign(), val);

                    signature.then(
                        function (result) {
                            B4.Ajax.request({
                                url: B4.Url.action("SendToEdm", me.controllerName),
                                params: {
                                    Id: id,
                                    Signature: result
                                },
                                timeout: 9999999
                            }).next(function (resp) {
                                B4.QuickMsg.msg('Сообщение', 'Документ успешно подписан и отправлен', 'success');
                                me.setSignatureCreated(true);
                                me.getGrid().unmask();
                                me.getGrid().getStore().load();
                                Ext.ComponentQuery.query('pdfWindow')[0].close();
                            }).error(function (e) {
                                B4.QuickMsg.msg('Сообщение', 'Ошибка при подписании: ' + (e.message || e), 'error');
                                me.getGrid().unmask();
                                Ext.ComponentQuery.query('pdfWindow')[0].close();
                            })
                        }
                    );

                }).error(function (e) {
                    B4.QuickMsg.msg('Ошибка!', 'Не удалось получить данные для подписания!', 'error');
                    grid.unmask();
                });
            }
        );
    }
});