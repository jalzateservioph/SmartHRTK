﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class Employee : BaseModel, IComparable<Employee>
    {
        private string _biometricsId;
        private string _employeeCode;
        private string _password;
        private string _fullName;
        private DateTime? _terminationDate;
        private string _jobGradeBand;

        public string EmployeeCode
        {
            get => _employeeCode;
            set
            {
                _employeeCode = value;
                NotifyOfPropertyChange();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange();
            }
        }
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                NotifyOfPropertyChange();
            }
        }
        public string BiometricsId
        {
            get => _biometricsId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ApplicationException($"{nameof(BiometricsId)} should not be empty");
                }
                else
                {
                    _biometricsId = value;
                    IsDirty = true;
                }
            }
        }
        public DateTime? TerminationDate
        {
            get => _terminationDate;
            set
            {
                _terminationDate = value;
                NotifyOfPropertyChange();
            }
        }
        public string JobGradeBand
        {
            get => _jobGradeBand;
            set
            {
                _jobGradeBand = value;
                NotifyOfPropertyChange();
            }
        }

        public int CompareTo(Employee other)
        {
            return BiometricsId.CompareTo(other.BiometricsId);
        }

        public override string ToString()
        {
            return EmployeeCode + " - " + FullName;
        }
    }
}
