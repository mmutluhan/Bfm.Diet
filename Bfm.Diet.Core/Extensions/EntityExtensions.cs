using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Bfm.Diet.Core.Base;
using Microsoft.AspNetCore.Http;

namespace Bfm.Diet.Core.Extensions
{
    public static class EntityExtensions
    {
        public static TEntity SetAudit<TEntity>(this TEntity entity, IHttpContextAccessor httpContextAccessor)
            where TEntity : ModelBase<int>
        {
            var uid = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Sid);
            if (uid == null)
                throw new Exception("Session does not contains user information");

            if (!int.TryParse(uid.Value, out var userId))
                throw new Exception("User Id is not correct format.");

            if (entity.KayitTarihi.HasValue)
                entity.GuncellemeTarihi = DateTime.Now;
            else
                entity.KayitTarihi = DateTime.Now;


            if (entity.Kaydeden.HasValue && entity.Kaydeden > 0)
                entity.Guncelleyen = userId;
            else
                entity.Kaydeden = userId;

            SetAuditRecursively(entity, userId);

            return entity;
        }

        private static void SetAuditRecursively(object entity, int userId)
        {
            var props = entity.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.CanWrite).ToList();

            var auditProps = props.Where(x =>
                new[] {"CreationDate", "Kaydeden", "GuncellemeTarihi", "Guncelleyen"}.Contains(x.Name)).ToList();
            auditProps.AddRange(props
                .Where(x => x.CanWrite && x.PropertyType.IsAbstract && x.PropertyType.IsGenericType).ToList());


            foreach (var property in auditProps)
            {
                if (property.CanWrite && property.PropertyType.IsAbstract && property.PropertyType.IsGenericType)
                    if (property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                        if (property.GetValue(entity) is ICollection items)
                            foreach (var col in items)
                                SetAuditRecursively(col, userId);


                switch (property.Name)
                {
                    case "KayitTarihi":
                    {
                        var val = property.GetValue(entity);
                        if (val == null)
                        {
                            property.SetValue(entity, DateTime.Now);
                        }
                        else
                        {
                            var updDtProp = auditProps.FirstOrDefault(x => x.Name == "GuncellemeTarihi");
                            if (updDtProp != null) updDtProp.SetValue(entity, DateTime.Now);
                        }

                        break;
                    }
                    case "Kaydeden":
                    {
                        var val = property.GetValue(entity);
                        if (val == null)
                        {
                            property.SetValue(entity, userId);
                        }
                        else
                        {
                            var updbyProp = auditProps.FirstOrDefault(x => x.Name == "Guncelleyen");
                            if (updbyProp != null) updbyProp.SetValue(entity, userId);
                        }

                        break;
                    }
                }
            }
        }
    }
}