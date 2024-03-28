Ext.define('B4.view.desktop.portlet.LoginForm', {
    alias: 'widget.esiaaddoperatorloginform',
    extend: 'Ext.form.Panel',
    bodyPadding: 10,
    width: 350,
    title: 'Введите логин и пароль от учетной записи ИАС "Мониторинг жилищного фонда"',
    layout: 'anchor',
    defaults: {
        anchor: '100%'
    },
 mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    items: [
        {
            xtype: 'textfield',
            fieldLabel: 'Логин',
            name: 'login',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            fieldLabel: 'Пароль',
            inputType: 'password',
            name: 'password',
            itemId: 'sfPassword',
            inputType: 'password',
            allowBlank: false
        },
        {
            xtype: 'label',
            name: 'error',
            style: {
                color: 'red'
            },
            componentCls: 'errorMessage'
        },
        //отображение пароля по чекеру
        {
            xtype: 'checkbox',
            style: { marginRight: '5px' },
            itemId: 'cbShowPassword',
            boxLabel: 'Показать пароль',
            labelWidth: 300,
            handler: function (cmp, checked) {
                if (checked) {
                    cmp.up('container').down('#sfPassword').inputEl.dom.type = 'text';
                } else {
                    cmp.up('container').down('#sfPassword').inputEl.dom.type = 'password';
                }


            }
        }
        //отображение пароля по кнопке
        //{
        //    xtype: 'fieldcontainer',
        //    width: '100%',
        //    layout: {
        //        type: 'hbox'
        //    },
        //    items: [{
        //        width: '90%',
        //        xtype: 'textfield',
        //        name: 'password',
        //        itemId: 'sfPassword2',
        //        msgTarget: 'under',
        //        allowBlank: false,
        //        fieldLabel: 'Пароль2' + '<span style="color:red">*</span>',
        //        inputType: 'password'
        //    }, {
        //            xtype: 'button',
        //            icon: B4.Url.content('content/img/icons/bullet_edit.png'),
        //        iconCls: 'fa fa-eye',
        //        tooltip: 'Показать пароль',
        //        handler: function (button) {
        //            var isShowPassword = this.iconCls === 'fa fa-eye';
        //            debugger;
        //            this.setTooltip(isShowPassword ? 'Скрыть пароль' : 'Показать пароль');
        //            this.setIconCls(isShowPassword ? 'fa fa-eye-slash' : 'fa fa-eye');
        //            this.setIcon(isShowPassword ? B4.Url.content('content/img/icons/bullet_key.png') : B4.Url.content('content/img/icons/bullet_edit.png'));
        //            if(isShowPassword)
        //            {
        //                button.up('container').down('#sfPassword2').inputEl.dom.type = 'text'; 
        //            }
        //            else
        //            {
        //                button.up('container').down('#sfPassword2').inputEl.dom.type = 'password';
        //            }
        //        }
        //    }]

        //}
    ],

    buttons: [
     
        {
            text: 'Привязать учетную запись',
            formBind: true, //only enabled once the form is valid
            disabled: true,
            handler: function () {
                var form = this.up('form'),
                    login = form.down('[name=login]').getValue(),
                    password = form.down('[name=password]').getValue(),
                    errorLabel = form.down('label[name=error]');

                B4.Ajax.request({
                    url: B4.Url.action('LinkEsiaAccount', 'OAuthLogin'),
                    params: {
                        login: login,
                        password: password
                    }
                }).next(function () {
                    window.open(Ext.String.format("login?login={0}&password={1}", login, password), "_self");
                }).error(function (e) {
                    errorLabel.setText(e.message || 'Не удалось привязать оператора');
                });
            }
        },
        {
            text: 'Создать нового пользователя',
          //  formBind: true, //only enabled once the form is valid
            disabled: false,
            handler: function () {
 var form = this.up('form');
var me = this;
me.mask;
 me.mask('Сохранение', form);
                B4.Ajax.request({
                    url: B4.Url.action('CreateNewEsiaOperator', 'EsiaOperator'),
                    params: {
                      
                    }
                }).next(function (response) {
                    debugger;
                    var resp = Ext.JSON.decode(response.responseText);
 me.unmask();
                    debugger;
                    window.open(Ext.String.format("login?login={0}&password={1}", resp.data.usrlogin, resp.data.usrpass), "_self");
                }).error(function (e) {
 me.unmask();
                    errorLabel.setText(e.message || 'Не удалось создать оператора');
                });
            }
        }],

    renderTo: Ext.getBody()
});