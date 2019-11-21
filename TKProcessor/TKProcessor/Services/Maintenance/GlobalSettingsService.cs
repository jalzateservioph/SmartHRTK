﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Models.DP;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class GlobalSettingsService : TKService<GlobalSetting>
    {
        readonly DPContext dPContext;

        public GlobalSettingsService() : base()
        {
            dPContext = new DPContext();
        }

        public override IEnumerable<GlobalSetting> List()
        {
            return Context.GlobalSetting
                            .Include(i => i.PayrollCodeMappings)
                            .Include(i => i.PayPackageMappings)
                            .Include(i => i.AutoApproveDTRFieldsList)
                            .ToList();

        }

        public override void Save(GlobalSetting entity)
        {
            try
            {
                var existing = Context.GlobalSetting.Include(i => i.PayrollCodeMappings)
                                                    .Include(i => i.PayPackageMappings)
                                                    .Include(i => i.AutoApproveDTRFieldsList).FirstOrDefault();

                if (existing == default(GlobalSetting))
                {
                    Context.GlobalSetting.Add(entity);
                }
                else
                {
                    entity.Id = existing.Id;

                    // Update parent
                    Context.Entry(existing).CurrentValues.SetValues(entity);

                    // PayrollCodeMappings
                    if (existing.PayrollCodeMappings == null)
                        existing.PayrollCodeMappings = new List<Mapping>();

                    // Delete children
                    foreach (var existingChild in existing.PayrollCodeMappings.ToList())
                    {
                        if (!entity.PayrollCodeMappings.Any(c => c.Id == existingChild.Id))
                            Context.Mapping.Remove(existingChild);
                    }

                    // Update and Insert children
                    foreach (var childentity in entity.PayrollCodeMappings)
                    {
                        var existingChild = existing.PayrollCodeMappings
                            .Where(c => c.Id == childentity.Id)
                            .SingleOrDefault();

                        if (existingChild != null)
                            // Update child
                            Context.Entry(existingChild).CurrentValues.SetValues(childentity);
                        else
                        {
                            // Insert child
                            var newChild = new Mapping
                            {
                                Id = childentity.Id,
                                Target = childentity.Target,
                                Source = childentity.Source,
                                Order = childentity.Order
                            };
                            existing.PayrollCodeMappings.Add(newChild);
                        }
                    }

                    // PayPackageMappings
                    if (existing.PayPackageMappings == null)
                        existing.PayPackageMappings = new List<Mapping>();

                    // Delete children
                    foreach (var existingChild in existing.PayPackageMappings.ToList())
                    {
                        if (!entity.PayPackageMappings.Any(c => c.Id == existingChild.Id))
                            Context.Mapping.Remove(existingChild);
                    }

                    // Update and Insert children
                    foreach (var childentity in entity.PayPackageMappings)
                    {
                        var existingChild = existing.PayPackageMappings
                            .Where(c => c.Id == childentity.Id)
                            .SingleOrDefault();

                        if (existingChild != null)
                            // Update child
                            Context.Entry(existingChild).CurrentValues.SetValues(childentity);
                        else
                        {
                            // Insert child
                            var newChild = new Mapping
                            {
                                Id = childentity.Id,
                                Target = childentity.Target,
                                Source = childentity.Source
                            };
                            existing.PayPackageMappings.Add(newChild);
                        }
                    }

                    // AutoApproveList
                    if (existing.AutoApproveDTRFieldsList == null)
                        existing.AutoApproveDTRFieldsList = new List<SelectionSetting>();

                    // Delete children
                    foreach (var existingChild in existing.AutoApproveDTRFieldsList.ToList())
                    {
                        if (!entity.AutoApproveDTRFieldsList.Any(c => c.Id == existingChild.Id))
                            Context.SelectionSetting.Remove(existingChild);
                    }

                    // Update and Insert children
                    foreach (var childentity in entity.AutoApproveDTRFieldsList)
                    {
                        var existingChild = existing.AutoApproveDTRFieldsList
                            .Where(c => c.Id == childentity.Id)
                            .SingleOrDefault();

                        if (existingChild != null)
                            // Update child
                            Context.Entry(existingChild).CurrentValues.SetValues(childentity);
                        else
                        {
                            // Insert child
                            var newChild = new SelectionSetting
                            {
                                Id = childentity.Id,
                                DisplayOrder = childentity.DisplayOrder,
                                Name = childentity.Name,
                                IsSelected = childentity.IsSelected
                            };
                            existing.AutoApproveDTRFieldsList.Add(newChild);
                        }
                    }
                }

                if (AutoSaveChanges)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
