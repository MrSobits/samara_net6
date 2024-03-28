using Bars.B4.DataAccess;
using NHibernate.Linq;

namespace Bars.Gkh.Qa.Utils
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;

    using Castle.Windsor;

    public class FakeUserIdentity : IUserIdentity
    {
        private IWindsorContainer container = ApplicationContext.Current.Container;

        private long _userId;

        /// <summary>
        /// включить мок
        /// </summary>
        public bool Mock { get; set; }

        /// <summary>
        /// подменённый компонент
        /// </summary>
        public IUserIdentity PrevUserIdentity { get; set; }

        public string TrackId
        {
            get
            {
                if (!this.Mock)
                {
                    return this.PrevUserIdentity.TrackId;
                }

                return "Тестовый пользователь";
            }
        }

        public string Name
        {
            get
            {
                if (!this.Mock)
                {
                    return this.PrevUserIdentity.Name;
                }

                return "Тестовый пользователь";
            }
        }

        public string AuthenticationType
        {
            get
            {
                if (!this.Mock)
                {
                    return this.PrevUserIdentity.AuthenticationType;
                }

                return "тестовая аутентификация";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (!this.Mock)
                {
                    return this.PrevUserIdentity.IsAuthenticated;
                }

                return true;
            }
        }

        public long UserId
        {
            get
            {
                if (!this.Mock)
                {
                    return this.PrevUserIdentity.UserId;
                }

                if (this._userId == 0)
                {
                    this._userId = this.CreateSuperUserTransfer();
                }

                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        private long CreateSuperUserTransfer()
        {
            var sessionProvider = this.container.Resolve<ISessionProvider>();
            var session = sessionProvider.OpenStatelessSession();
            try
            {
                var user = new User { Name = "Тестовый пользователь", Login = Guid.NewGuid().ToString() };
                session.Insert(user);

                var role = new Role { Name = "Тестовая роль" };
                session.Insert(role);

                var userRole = new UserRole();
                userRole.User = user;
                userRole.Role = role;

                session.Insert(userRole);

                var allStates = session.Query<State>().OrderBy(x => x.Id).ToList();

                allStates.ForEach(
                    x => allStates.ForEach(
                        y => session.Insert(
                            new StateTransfer
                            {
                                CurrentState = x,
                                NewState = y,
                                Role = role,
                                TypeId = y.TypeId
                            })));

                return user.Id;
            }
            finally
            {
                this.container.Release(session);
            }
        }
    }
}
