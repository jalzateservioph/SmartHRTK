using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TKProcessor.Services;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKProcessor.WPF.Views;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase<User>
    {
        UserService service;
        IMapper mapper;
        private bool _isInvalid;
        private string _messsage;

        public LoginViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new UserService()
            {
                UseDefaultUser = false
            };

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TK.User, User>();
                cfg.CreateMap<User, TK.User>();
            }).CreateMapper();

            ActivateItem(new User());

            IsInvalid = false;
        }

        public void PreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        public void Login()
        {
            try
            {
                if (service.List().Count() == 0)
                {
                    Messsage = "There are no users in the database";
                    IsInvalid = true;
                }
                else
                {
                    if (!service.TryLogin(ActiveItem.Username, (ActiveItem.Password = ConvertToUnsecureString((Views.First().Value as IHavePassword).Get())), out TK.User user))
                    {
                        Messsage = "Invalid Username or Password";
                        IsInvalid = true;
                    }
                    else
                    {
                        eventAggregator.PublishOnUIThread(new LoginEvent() { CurrentUser = mapper.Map<User>(user) });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public bool IsInvalid
        {
            get => _isInvalid;
            set
            {
                _isInvalid = value;
                NotifyOfPropertyChange();
            }
        }

        public string Messsage
        {
            get => _messsage;
            set
            {
                _messsage = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
