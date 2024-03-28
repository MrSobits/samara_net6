namespace Bars.B4.Modules.ESIA.Auth.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.dk.nita.saml20;
    using Bars.B4.Modules.ESIA.dk.nita.saml20.Actions;
    using Bars.B4.Modules.ESIA.dk.nita.saml20.Protocol;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.Operator;

    /// <summary>
    /// Авторизация через ЕСИА
    /// </summary>
    public class ESIAuth : IAction
    {
        public void LoginAction(AbstractEndpointHandler handler, HttpContext context, Saml20Assertion assertion)
        {
            var contragentDomain = ApplicationContext.Current.Container.ResolveDomain<Contragent>();
            var userManager = ApplicationContext.Current.Container.Resolve<IGkhUserManager>();
            var esiaOperatorDomain = ApplicationContext.Current.Container.ResolveDomain<EsiaOperator>();
            var logManager = ApplicationContext.Current.Container.Resolve<ILogManager>();

            try
            {
                var esiaOperator = new EsiaOperator();
                foreach (var attribute in assertion.Attributes)
                {
                    var name = attribute.FriendlyName;
                    var prop = typeof(EsiaOperator).GetProperty(
                        name,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (prop == null)
                    {
                        continue;
                    }

                    prop.SetValue(esiaOperator, attribute.AttributeValue[0], null);
                }

                this.ProcessOrgAddresses(esiaOperator);

                if (esiaOperator.OrgOgrn.IsEmpty())
                {
                    context.Response.Cookies.Add(new HttpCookie("esiaError")
                    {
                        Value = "В ЕСИА не привязана организация",
                        Expires = DateTime.Now.AddMinutes(5)
                    });
                    return;
                }

                var existsEsiaOperator = esiaOperatorDomain.GetAll()
                    .FirstOrDefault(x => x.UserId == esiaOperator.UserId);

                // 1 случай, данные учетки ЕСИА уже есть в ЖКХ
                if (existsEsiaOperator != null)
                {
                    this.Login(context, existsEsiaOperator);
                    return;
                }

                // 2 случай, под этой учеткой ЕСИА входят первый раз

                // 2.1 удалось найти контрагента в системе ЖКХ по ОГРН, полученному от ЕСИА
                var contragentByOgrn = contragentDomain.GetAll().FirstOrDefault(x => x.Ogrn == esiaOperator.OrgOgrn);
                if (contragentByOgrn != null)
                {
                    esiaOperator.Operator = userManager.GetBaseOperatorForEsia();
                    esiaOperatorDomain.Save(esiaOperator);

                    this.Login(context, esiaOperator);
                    return;
                }

                // 2.2 удалось найти контрагента в системе ЖКХ по ИНН + КПП, полученными от ЕСИА
                // проставляем сразу ОГРН из ЕСИА
                var contragentByInnKpp =
                    contragentDomain.GetAll()
                        .FirstOrDefault(x => x.Inn == esiaOperator.OrgInn && x.Kpp == esiaOperator.OrgKpp);
                if (contragentByInnKpp != null)
                {
                    contragentByInnKpp.Ogrn = esiaOperator.OrgOgrn;
                    contragentDomain.Save(contragentByInnKpp);

                    esiaOperator.Operator = userManager.GetBaseOperatorForEsia();
                    esiaOperatorDomain.Save(esiaOperator);

                    this.Login(context, esiaOperator);
                    return;
                }

                // 3 случай, не нашли в системе контрагента ни по ОГРН, ни по КПП
                context.Response.Cookies.Add(new HttpCookie("esiaError")
                {
                    Value = "ОГРН не найден в системе",
                    Expires = DateTime.Now.AddMinutes(5)
                });
            }
            catch (Exception exception)
            {
                logManager.Error("Непредвиденная ошибка", exception);
                EmailSender.Instance.TrySendIfLogEnabled(exception.Message, exception.InnerException + "\r\n" + exception.StackTrace);
                context.Response.Cookies.Add(new HttpCookie("esiaError")
                {
                    Value = "Непредвиденная ошибка",
                    Expires = DateTime.Now.AddMinutes(5)
                });
            }
            finally
            {
                ApplicationContext.Current.Container.Release(contragentDomain);
                ApplicationContext.Current.Container.Release(userManager);
                ApplicationContext.Current.Container.Release(esiaOperatorDomain);
            }
        }

        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context, bool IdPInitiated)
        {
            FormsAuthentication.SignOut();
        }

        public string Name { get; set; }

        public void Login(HttpContext context, EsiaOperator esiaOperator)
        {
            var userId = esiaOperator.Operator.User.Id;
            var userName = esiaOperator.UserName;

            var account = new DynamicDictionary();
            account.AsDynamic.UserId = userId;
            account.AsDynamic.UserName = userName;
            account.AsDynamic.EsiaAuth = true;

            var json = JsonNetConvert.SerializeObject(ApplicationContext.Current.Container, account);
            var auth = new FormsAuthenticationTicket(2,
                userId.ToString(CultureInfo.InvariantCulture),
                DateTime.Now,
                DateTime.Now.AddHours(12),
                true,
                json);

            var ticket = FormsAuthentication.Encrypt(auth);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket)
            {
                Expires = auth.Expiration
            };

            context.Response.Cookies.Add(cookie);

            context.Response.Cookies.Add(new HttpCookie("esiaSuccess")
            {
                Value = "success",
                Expires = DateTime.Now.AddMinutes(5)
            });
        }
       
        private void ProcessOrgAddresses(EsiaOperator esiaOperator)
        {
            var xml = esiaOperator.OrgAddresses;

            if (xml.IsEmpty())
            {
                return;
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var resultAddress = new List<string>();
            var nodes = new[] { "index", "region", "city", "street", "house" };

            foreach (var node in nodes)
            {
                var element = xmlDoc.GetElementsByTagName(node)[0];
                if (element.IsNotNull())
                {
                    resultAddress.Add(element.InnerText);
                }
            }

            esiaOperator.OrgAddresses = string.Join(", ", resultAddress);
        }
    }
}