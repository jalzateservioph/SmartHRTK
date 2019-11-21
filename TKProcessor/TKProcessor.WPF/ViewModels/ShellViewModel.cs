using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class ShellViewModel : Conductor<Screen>, IDisposable, IHandle<NewMessageEvent>, IHandle<LoginEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;

        private SolidColorBrush _messageColor;
        private SolidColorBrush _messageFontColor;
        private System.Timers.Timer t;
        private bool verifying = true;
        private string _message;
        private bool _hasMessage;
        private bool _isLoggedIn;
        private bool _isVerifyingDB;
        private string _startupMessage;

        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;

            eventAggregator.Subscribe(this);

            t = new System.Timers.Timer() { Interval = 5000 };
            t.Elapsed += (o, e) => { HasMessage = false; };

            ActivateItem(new LoginViewModel(eventAggregator, windowManager));

            verifying = true;

            Task.Run(() =>
            {
                try
                {
                    string customStartupMessage = "";

                    HasStartupMessage = true;

                    var t = Task.Run(() =>
                    {
                        while (verifying)
                        {
                            if (string.IsNullOrEmpty(StartupMessage) || StartupMessage.Contains("...")) StartupMessage = customStartupMessage;
                            else if (StartupMessage.Contains("..")) StartupMessage = $"{customStartupMessage}...";
                            else if (StartupMessage.Contains(".")) StartupMessage = $"{customStartupMessage}..";
                            else StartupMessage = $"{customStartupMessage}.";

                            Thread.Sleep(300);
                        }
                    });

                    customStartupMessage = "Verifying SmartHR Database";

                    if (!DatabaseService.TryConnectSHR())
                        throw new Exception("Cannot connect to SmartHR database.");

                    customStartupMessage = "Verifying DynamicPay Database";

                    if (!DatabaseService.TryConnectDP())
                        throw new Exception("Cannot connect to DynamicPay database.");

                    customStartupMessage = "Creating/Updating Timekeeping Database";

                    DatabaseService.Migrate();

                    customStartupMessage = "Successfully verified databses.";

                    Thread.Sleep(500);

                    HasStartupMessage = false;
                }
                catch (Exception ex)
                {
                    verifying = false;

                    StartupMessage = ex.Message;
                }
            });

        }

        public void Handle(NewMessageEvent message)
        {
            Message = message.Message;

            MessageFontColor = new SolidColorBrush(Colors.White);

            if (message.MessageType == MessageType.Warning)
            {
                MessageColor = new SolidColorBrush(Colors.Orange);
            }
            else if (message.MessageType == MessageType.Error)
            {
                MessageColor = new SolidColorBrush(Colors.Red);
            }
            else if (message.MessageType == MessageType.Success)
            {
                MessageColor = new SolidColorBrush(Colors.Green);
            }
            else
            {
                MessageColor = new SolidColorBrush(Colors.DarkCyan);
            }

            HasMessage = true;

            if (message.Duration >= 0)
            {
                t.Stop();
                t.Start();
            }
        }

        public void ShowEmployees()
        {
            ChangeActiveItem(new EmployeeViewModel(eventAggregator, windowManager), true);
        }

        public void ShowShifts()
        {
            ChangeActiveItem(new ShiftViewModel(eventAggregator, windowManager), true);
        }

        public void ShowWorkSchedules()
        {
            ChangeActiveItem(new WorkScheduleViewModel(eventAggregator, windowManager), true);
        }

        public void ShowHolidays()
        {
            ChangeActiveItem(new HolidayViewModel(eventAggregator, windowManager), true);
        }

        public void ShowRawData()
        {
            ChangeActiveItem(new RawDataViewModel(eventAggregator, windowManager), true);
        }

        public void ShowDailyTransactionRecords()
        {
            ChangeActiveItem(new DailyTransactionRecordViewModel(eventAggregator, windowManager), true);
        }

        public void ShowDashboard()
        {
            ChangeActiveItem(new DashboardViewModel(), true);
        }

        public void ShowGlobalSettings()
        {
            ChangeActiveItem(new GlobalSettingsViewModel(eventAggregator, windowManager), true);
        }

        public void ShowAuditLogs()
        {
            ChangeActiveItem(new AuditLogsViewModel(eventAggregator, windowManager), true);
        }

        public void ShowErrorLogs()
        {
            ChangeActiveItem(new ErrorLogsViewModel(eventAggregator, windowManager), true);
        }

        public void ExitApplication()
        {
            App.Current.Shutdown();
        }

        public void ShowReports()
        {
            ChangeActiveItem(new ReportViewModel(), true);
        }

        public void ShowAbsenceReport()
        {
            ChangeActiveItem(new ReportAbsenceViewModel(), true);
        }

        public void Handle(LoginEvent message)
        {
            IsLoggedIn = (message.CurrentUser != default(User));

            if (IsLoggedIn)
            {
                verifying = false;

                Session.Default.CurrentUser = message.CurrentUser;

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Logged in as {Session.Default.CurrentUser.Name}"));

                ShowDashboard();
            }
        }

        public void Dispose()
        {
            if (ActiveItem is IDisposable)
                (ActiveItem as IDisposable).Dispose();
        }

        public bool HasMessage
        {
            get => _hasMessage;
            set
            {
                _hasMessage = value;
                NotifyOfPropertyChange();
            }
        }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange();
            }
        }
        public SolidColorBrush MessageColor
        {
            get => _messageColor;
            set
            {
                _messageColor = value;
                NotifyOfPropertyChange();
            }
        }
        public SolidColorBrush MessageFontColor
        {
            get => _messageFontColor;
            set
            {
                _messageFontColor = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                NotifyOfPropertyChange();
            }
        }
        public string StartupMessage
        {
            get => _startupMessage;
            set
            {
                _startupMessage = value;
                NotifyOfPropertyChange();
            }
        }
        public bool HasStartupMessage
        {
            get => _isVerifyingDB;
            set
            {
                _isVerifyingDB = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
