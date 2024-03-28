Ext.define('B4.view.package.PackageGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.packagegrid',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    columnLines: true,

    viewConfig: {
        markDirty: false
    },

    closable: false,

    needSign: true,

    signer: undefined,

    bubbleEvents: ['signingStart', 'signingComplete', 'packageSigning'],

    initComponent: function () {
        var me = this;

        me.store = Ext.create('B4.store.RisPackage');

        me.columns = me.getColumnsCfg(me.needSign);

        if (me.needSign) {

            me.signer = new XadesSigner();

            me.dockedItems = [
                me.getSignToolbarCfg()
            ];
        }

        this.addEvents('signingStart', 'signingComplete', 'packageSigning');

        me.callParent(arguments);
    },

    getColumnsCfg: function (needSign) {
        var me = this,
            result = [
            {
                xtype: 'gridcolumn',
                flex: 1,
                align: 'center',
                text: 'Наименование',
                dataIndex: 'Name'
            },
            {
                xtype: 'actioncolumn',
                flex: 1,
                align: 'center',
                text: 'Неподписанная XML',
                //dataIndex: 'NotSignedDataLength',
                //dataIndex: 'Signed',
                name: 'NotSignedData',
                renderer: function (val) {
                   // if (val > 0) {
                        return '<a href="javascript:void(0)" style="color: black;">Просмотр</a>';
                   // }
                   // return null;
                },
                listeners: {
                    click: me.showNotSignedData,
                    scope: me
                }
            }
            ];

        if (needSign === true) {
            result.push({
                xtype: 'actioncolumn',
                flex: 1,
                align: 'center',
                text: 'Подписанная XML',
                //dataIndex: 'SignedDataLength',
                dataIndex: 'Signed',
                name: 'SignedData',
                renderer: function (val) {
                    //if (val > 0) {
                    if (val === true) {
                        return '<a href="javascript:void(0)" style="color: black;">Просмотр</a>';
                    }

                    return null;
                },
                listeners: {
                    click: me.showSignedData,
                    scope: me
                }
            });
        }

        return result;
    },

    getSignToolbarCfg: function () {
        var me = this;
        return {
            xtype: 'toolbar',
            dock: 'top',
            items: [
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        {
                            xtype: 'combobox',
                            emptyItem: { SubjectName: '-' },
                            displayField: 'SubjectName',
                            valueField: 'Certificate',
                            editable: false,
                            queryMode: 'local',
                            fieldLabel: 'Сертификат для подписи',
                            width: 590,
                            labelWidth: 135,
                            labelAlign: 'right',
                            name: 'Certificate',
                            listeners: {
                                expand: me.updateCertificatesCombo,
                                change: me.setSignerCertificate,
                                scope: me
                            }
                        },
                        {
                            xtype: 'button',
                            name: 'Sign',
                            text: 'Подписать',
                            iconCls: 'icon-accept',
                            handler: me.onSignButtonClick,
                            scope: me
                        }
                    ]
                }
            ]
        };
    },

    showNotSignedData: function (gridView, rowIndex, colIndex, item, e, record) {
        var me = this;
        me.showXmlData(record, false);
    },

    showSignedData: function (gridView, rowIndex, colIndex, item, e, record) {
        var me = this;
        me.showXmlData(record, true);
    },

    showXmlData: function (packageStoreRecord, signed) {
        var me = this;

        B4.Ajax.request({
            url: B4.Url.action('GetPackageXmlData', 'Package'),
            params: {
                //package_Ids: packageStoreRecord.get('Id'),
                packageId: packageStoreRecord.get('Id'),
                packageType: packageStoreRecord.get('Type'),
                forPreview: true,
                signed: signed
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText).data;

            //me.openXmlPreviewWindow(packageStoreRecord, data[0].Data, signed);
            me.openXmlPreviewWindow(packageStoreRecord, data, signed);
        }, me).error(function (e) {
            Ext.Msg.alert('Ошибка!', 'Не получены xml данные' + '<br>' + (e.message || e));
        }, me);
    },

    openXmlPreviewWindow: function (packageStoreRecord, xmlData, signed) {

        var packageName = packageStoreRecord.get('Name') || 'Пакет',
            dataName = signed ? 'подписанные данные' : 'неподписанные данные',

            xmlPreviewWin = Ext.create('B4.view.integrations.gis.PackageDataPreviewWindow',
            {
                xmlData: xmlData,
                title: packageName + ' - ' + dataName
            });

        xmlPreviewWin.show();
    },

    updateCertificatesCombo: function (field, eOpts) {
        var me = this;

        me.signer.getCertificates(2, 'My', me.fillCertificatesCombo, function (e) {
            Ext.Msg.alert('Ошибка получения списка сертификатов!', 'Не удалось получить сертификаты' + '<br>' + (e.message || e));
        }, me);
    },

    fillCertificatesCombo: function (certificates) {
        var me = this,
            certCombo = me.down('combobox[name=Certificate]'),
            certComboStore = certCombo.getStore();

        certCombo.clearValue();
        certComboStore.removeAll();

        Ext.each(certificates, function (cert) {
            var certificateRec = Ext.create('B4.model.RisCertificate', {
                SubjectName: cert.subjectName,
                Certificate: cert
            });

            certComboStore.add(certificateRec);
        });
    },

    setSignerCertificate: function (field, newValue, oldValue, eOpts) {
        var me = this;

        me.signer.setCertificate(newValue);
    },

    onSignButtonClick: function (button, e) {
        var me = this,
            packageStore = me.getStore(),
            certCombo = me.down('combobox[name=Certificate]'),
            packageIdsToSign = [];

        if (!certCombo.value) {
            Ext.Msg.alert('Ошибка!', 'Выберите сертификат для подписи');
            return;
        }

        packageStore.each(function (rec) {
            packageIdsToSign.push(rec.get('Id'));
            /*var notSignedDataLength = rec.get('NotSignedDataLength');

            if (notSignedDataLength && notSignedDataLength > 0) {
                packageIdsToSign.push(rec.get('Id'));
            } else {
                var errorMessage = 'Нет неподписанных данных на клиенте'
                + '<br><br>'
                + 'Наименование пакета: ' + rec.get('Name')
                + '<br><br>'
                + 'Идентификатор пакета: ' + rec.get('Id');
                Ext.Msg.alert('Ошибка!', errorMessage);
                return false;
            }*/
        });

        if (packageIdsToSign.length === packageStore.getCount()) {
            me.fireEvent('signingStart');
            me.signNextPackage(packageIdsToSign);
        }
    },

    signNextPackage: function (packageIdsToSign, previousSigningResult) {

        var me = this,
            packageStore = me.getStore();

        if (previousSigningResult && previousSigningResult.success === false) {
            var errorMessage = (previousSigningResult.message || 'Ошибка при подписывании пакета')
                + '<br><br>'
                + 'Наименование пакета: ' + previousSigningResult.packageName
                + '<br><br>'
                + 'Идентификатор пакета: ' + previousSigningResult.packageId;

            Ext.Msg.alert('Ошибка!', errorMessage);

            me.fireEvent('signingComplete');
            return;
        }

        if (!packageIdsToSign || packageIdsToSign.length === 0) {
            me.fireEvent('signingComplete');
            return;
        }

        var nextRecordToSign = packageStore.findRecord('Id', packageIdsToSign[0]);

        packageIdsToSign = packageIdsToSign.slice(1);

        me.signPackage(nextRecordToSign, packageIdsToSign);
    },

    signPackage: function (storeRecord, packageIdsToSign) {
        var me = this,
            packageId = storeRecord.get('Id'),
            packageType = storeRecord.get('Type'),
            packageName = storeRecord.get('Name'),
            signingResult = {
                success: true,
                message: '',
                packageId: packageId,
                packageName: packageName
            };

        me.fireEvent('packageSigning', {
            id: packageId,
            name: packageName
        });

        B4.Ajax.request({
            url: B4.Url.action('GetPackageXmlData', 'Package'),
            params: {
                //package_Ids: [packageId],
                packageId: packageId,
                packageType: packageType,
                signed: false
            },
            timeout: 9999999
        }).next(function (response) {
            //var notSignedPackages = Ext.decode(response.responseText).data;
            //var notSignedData = notSignedPackages[0].Data;

            var notSignedData = Ext.decode(response.responseText).data;

            if (!notSignedData || notSignedData.length === 0) {
                signingResult.success = false;
                signingResult.message = 'Не получены неподписанные данные с сервера';

                me.signNextPackage(packageIdsToSign, signingResult);
            }
            debugger;
            me.signer.signXml(notSignedData, function (xml) {
               /* var dataToSend = [
                {
                    Id: packageId,
                    SignedData: encodeURI(xml)
                }];*/

                B4.Ajax.request({
                    url: B4.Url.action('SaveSignedData', 'Package'),
                    params: {
                        //packages: Ext.JSON.encode(dataToSend)
                        packageId: packageId,
                        packageType: packageType,
                        signedData: encodeURI(xml)
                    },
                    timeout: 9999999

                }).next(function (response) {
                    //var signedPackages = Ext.decode(response.responseText).data;
                    //var signedDataLength = signedPackages[0].SignedDataLength;
                    //var signed = signedPackages[0].Signed;

                    var data = Ext.decode(response.responseText).data;
                    var signed = data.Signed;

                    //storeRecord.set('SignedDataLength', signedDataLength);
                    storeRecord.set('Signed', signed);
                    me.signNextPackage(packageIdsToSign, signingResult);

                }, me).error(function (e) {
                    signingResult.success = false;
                    signingResult.message = 'Не удалось сохранить подписанные данные на сервере' + '<br>' + (e.message || e);

                    me.signNextPackage(packageIdsToSign, signingResult);
                }, me);

            }, function (e) {
                signingResult.success = false;
                signingResult.message = 'Не удалось подписать данные на клиенте' + '<br>' + (e.message || e);

                me.signNextPackage(packageIdsToSign, signingResult);
            }, me);

        }, me).error(function (e) {
            signingResult.success = false;
            signingResult.message = 'Не получены неподписанные данные с сервера' + '<br>' + (e.message || e);

            me.signNextPackage(packageIdsToSign, signingResult);

        }, me);
    }
});
