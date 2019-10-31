﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.DP;

namespace TKProcessor.Services
{
    public class JobGradeBandService
    {
        readonly SHRContext context;

        public JobGradeBandService()
        {
            context = new SHRContext();
        }

        public IList<string> List()
        {
            return context.JobGradeBandLu.Select(i => i.JobGradeBand).Distinct().ToList();
        }
    }

    public class PayrollCodeService
    {
        readonly DPContext context;

        public PayrollCodeService()
        {
            context = new DPContext();
        }

        public List<PayrollCode> List()
        {
            return context.PayrollCode.ToList();
        }
    }

    public class PayPackageService
    {
        readonly DPContext context;

        public PayPackageService()
        {
            context = new DPContext();
        }

        public List<PayPackage> List()
        {
            return context.PayPackage.ToList();
        }
    }
}